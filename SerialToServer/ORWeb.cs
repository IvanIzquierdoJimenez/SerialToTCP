using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Security.Policy;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using System.Data;
using System.Net.Sockets;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SerialToServer
{
    public abstract class SimulatorInterface
    {
        public class Parameter
        {
            public string ServerName;
            public string SimulatorName;
            public double? ServerMin;
            public double? ServerMax;
            public double? SimulatorMin;
            public double? SimulatorMax;
            public bool Write;
        }
        protected Dictionary<string, Parameter> TcpToSimulatorNames = new Dictionary<string, Parameter>();
        protected Dictionary<string, Parameter> SimulatorToTcpNames = new Dictionary<string, Parameter>();
        private TcpClient client;
        StreamReader reader;
        Stream stream;
        public bool Active = false;
        public bool Failed = false;
        public SimulatorInterface()
        {
            client = new TcpClient("127.0.0.1", 5090);
            stream = client.GetStream();
            reader = new StreamReader(stream);

            string file = File.ReadAllText("parameters.json");
            List<Parameter> parameters = JsonConvert.DeserializeObject<List<Parameter>>(file);
            SetupParameters(parameters);
            foreach (var parameter in parameters)
            {
                if (parameter.Write)
                {
                    TcpToSimulatorNames[parameter.ServerName] = parameter;
                }
                else
                {
                    SimulatorToTcpNames[parameter.SimulatorName] = parameter;
                }
                if (parameter.Write)
                {
                    //Console.WriteLine("register("+parameter.ServerName+")\n");
                    if (!TcpToSimulatorNames.ContainsKey(parameter.ServerName)) TcpToSimulatorNames[parameter.ServerName] = parameter;
                    var data = Encoding.UTF8.GetBytes("register("+parameter.ServerName+")\n");
                    stream.Write(data, 0, data.Length);
                }
            }
            new Thread(TCPToOR).Start();
        }
        public abstract void SendSimulator(Parameter parameter, double value);
        public abstract void SetupParameters(List<Parameter> parameters);
        public void SendServer(Parameter parameter, double value)
        {
            if (parameter.ServerMin.HasValue)
            {
                if (parameter.SimulatorMin.HasValue)
                {
                    value = (value - parameter.SimulatorMin.Value)/(parameter.SimulatorMax.Value-parameter.SimulatorMin.Value);
                }
                value = parameter.ServerMin.Value + value * (parameter.ServerMax.Value - parameter.ServerMin.Value);
            }
            SendServer(parameter.ServerName, value.ToString().Replace(',','.'));
        }
        public void SendServer(string parameter, string value)
        {
            var data = Encoding.UTF8.GetBytes(parameter+"="+value+"\n");
            stream.Write(data, 0, data.Length);
        }
        public void SendServer(string line)
        {
            var data = Encoding.UTF8.GetBytes(line+"\n");
            stream.Write(data, 0, data.Length);
        }
        void TCPToOR()
        {
            while (!Active)
            {
                Thread.Sleep(1000);
            }
            while (Active)
            {
                string line = reader.ReadLine();
                //Console.WriteLine(line);
                if (line.StartsWith("register("))
                {
                    
                }
                else if (line.Contains('='))
                {
                    var spl = line.Split('=');
                    if (TcpToSimulatorNames.TryGetValue(spl[0], out var parameter) && double.TryParse(spl[1], NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out double value))
                    {
                        if (parameter.ServerMin.HasValue)
                        {
                            value = (value - parameter.ServerMin.Value)/(parameter.ServerMax.Value - parameter.ServerMin.Value);
                            if (parameter.SimulatorMin.HasValue)
                            {
                                value = parameter.SimulatorMin.Value + value * (parameter.SimulatorMax.Value-parameter.SimulatorMin.Value);
                            }
                        }
                        SendSimulator(parameter, value);
                    }
                }
            }
            try
            {
                client.Close();
            }
            catch (Exception e)
            {
                
            }
        }
    }
    public class OrWeb : SimulatorInterface
    {
        /// <summary>
        /// Contains information about a single cab control.
        /// </summary>
        public struct ControlValue
        {
            public string TypeName;
            public double MinValue;
            public double MaxValue;
            public double RangeFraction;
        }

        /// <summary>
        /// Contains a posted value for a single cab control.
        /// </summary>
        public struct ControlValuePost
        {
            public string TypeName;
            public int ControlIndex;
            public double Value;
        }
        public OrWeb()
        {
            new Thread(Read);
        }
        public override void SetupParameters(List<Parameter> parameters)
        {
            var indexes = new Dictionary<string, int>();
            List<ControlValue> values = GetValues();
            while(values == null)
            {
                Thread.Sleep(5000);
                values = GetValues();
            }
            Active = true;
            foreach (var cv in values)
            {
                int index = 0;
                if (indexes.ContainsKey(cv.TypeName)) index = indexes[cv.TypeName] + 1;
                indexes[cv.TypeName] = index;
                string name = (cv.TypeName+":"+index);
                foreach (var parameter in parameters)
                {
                    if ((parameter.SimulatorName == cv.TypeName && index == 0) || parameter.SimulatorName == name)
                    {
                        parameter.SimulatorName = name;
                        if (!parameter.SimulatorMin.HasValue)
                        {
                            parameter.SimulatorMin = cv.MinValue;
                            parameter.SimulatorMax = cv.MaxValue;
                        }
                    }
                }
            }         
        }

        private void Read()
        {
            while (!Active)
            {
                Thread.Sleep(2000);
            }
            while (Active)
            {
                var indexes = new Dictionary<string, int>();
                var values = GetValues();
                if (values == null)
                {
                    Active = false;
                    Failed = true;
                    return;
                }
                foreach (var cv in values)
                {
                    int index = 0;
                    if (indexes.ContainsKey(cv.TypeName)) index = indexes[cv.TypeName] + 1;
                    indexes[cv.TypeName] = index;
                    if (SimulatorToTcpNames.TryGetValue(cv.TypeName+':'+index, out var parameter))
                    {
                        SendServer(parameter, cv.MinValue + cv.RangeFraction * (cv.MaxValue - cv.MinValue));
                    }
                }
                Thread.Sleep(250);
            }
        }
        public override void SendSimulator(Parameter parameter, double value)
        {
            var spl = parameter.SimulatorName.Split(':');
            ControlValuePost cv = new ControlValuePost();
            cv.TypeName = spl[0];
            if (spl.Length > 1) cv.ControlIndex = int.Parse(spl[1]);
            cv.Value = value;
            string json = JsonConvert.SerializeObject(new ControlValuePost[]{cv});
            var bytes = Encoding.UTF8.GetBytes(json);
            try
            {
                var request = WebRequest.Create("http://localhost:2150/API/CABCONTROLS");
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = bytes.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                    using (var response = request.GetResponse()) {}
                }
            }
            catch (Exception e)
            {
                Active = false;
                Failed = true;
                Console.WriteLine(e);
            }
        }
        List<ControlValue> GetValues()
        {
            try
            {
                var request = WebRequest.Create("http://localhost:2150/API/CABCONTROLS");
                request.Method = "GET";
                return JsonConvert.DeserializeObject<List<ControlValue>>(new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd());
            }
            catch (Exception e)
            {
                if (Active) Console.WriteLine(e);
                return null;
            }
        }
    }
    public class RwDll : SimulatorInterface
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetDllDirectory(string lpPathName);
        [DllImport("RailDriver64.dll")]
        internal static extern void SetControllerValue(int Control, float Value);
        [DllImport("RailDriver64.dll")]
        internal static extern Single GetControllerValue(int Control, int Mode);
        [DllImport("RailDriver64.dll")]
        internal static extern void SetRailDriverConnected(bool Value);
        [DllImport("RailDriver64.dll")]
        internal static extern void SetRailSimConnected(bool Value);
        [DllImport("RailDriver64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr GetLocoName();
        [DllImport("RailDriver64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr GetControllerList();
        Dictionary<Parameter, int> ParameterIndex = new Dictionary<Parameter, int>();
        Dictionary<Parameter, double> LastValues = new Dictionary<Parameter, double>();
        public string RWPath = "";
        public RwDll()
        {
            Time = DateTime.UtcNow;
            
            new Thread(Read).Start();

            new Thread(Antenna).Start();
        }
        int SpeedometerControlIndex;
        double distance;
        double sentDistance;
        DateTime Time;
        private void Read()
        {
            while (!Active)
            {
                Thread.Sleep(1000);
            }
            while (Active)
            {
                foreach (var kvp in ParameterIndex)
                {
                    var parameter = kvp.Key;
                    if (!parameter.Write)
                    {
                        double value = GetControllerValue(kvp.Value, 0);
                        if (!LastValues.TryGetValue(parameter, out double val) || Math.Abs(value-val) > 0.01f)
                        {
                            LastValues[parameter] = value;
                            SendServer(parameter, value);
                        }
                    }
                }
                double speed = GetControllerValue(SpeedometerControlIndex, 0)/3.6f;
                distance += Math.Abs(speed)*(DateTime.UtcNow-Time).TotalSeconds;
                Time = DateTime.UtcNow;
                if (distance - sentDistance > 1)
                {
                    SendServer("distance", distance.ToString().Replace(',','.'));
                    sentDistance = distance;
                }
                Thread.Sleep(100);
            }
        }
        public override void SetupParameters(List<Parameter> parameters)
        {
            RWPath = File.ReadAllLines("railworks_path.txt")[0];
            SetDllDirectory(Path.Combine(RWPath, "plugins"));
            string loco = null;
            while (loco == null || loco == "")
            {
                SetRailDriverConnected(true);
                SetRailSimConnected(true);
                loco = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(GetLocoName());
                Thread.Sleep(500);
            }
            Active = true;
            Console.WriteLine(loco);
            string tmp = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(GetControllerList());
            Console.WriteLine(tmp);
            var controls = tmp.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (var parameter in parameters)
            {
                int index = controls.IndexOf(parameter.SimulatorName);
                ParameterIndex[parameter] = index;
                if (!parameter.SimulatorMin.HasValue)
                {
                    parameter.SimulatorMin = GetControllerValue(index, 1);
                    parameter.SimulatorMax = GetControllerValue(index, 2);
                }
            }
            SpeedometerControlIndex = controls.IndexOf("SpeedometerKPH");
        }
        public override void SendSimulator(Parameter parameter, double value)
        {
            if (ParameterIndex.TryGetValue(parameter, out int index))
            {
                SetControllerValue(index, (float)value);
            }
        }
        public void Antenna()
        {
            if (!File.Exists(Path.Combine(RWPath, "plugins", "ServerData.txt"))) return;
            var wh = new AutoResetEvent(false);
            var fsw = new FileSystemWatcher(Path.Combine(RWPath, "plugins"));
            fsw.Filter = "ServerData.txt";
            fsw.EnableRaisingEvents = true;
            fsw.Changed += (s,e) => wh.Set();
            using (var fs = new FileStream(Path.Combine(RWPath, "plugins", "ServerData.txt"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            using (var reader = new StreamReader(fs))
            {
                fs.Seek(fs.Length, SeekOrigin.Begin);
                while (true)
                {
                    var line = reader.ReadLine();

                    if (!String.IsNullOrWhiteSpace(line)) SendServer(line);
                    else wh.WaitOne(100);
                }
            }
        }
    }
}
