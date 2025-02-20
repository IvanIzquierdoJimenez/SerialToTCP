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
using System.Data.SqlTypes;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;

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
        protected StreamReader Reader;
        protected StreamWriter Writer;
        public bool Active = false;
        public bool Disconnected = false;
        public bool UnrecoverableFault = false;
        Task SetupTask;
        protected List<Parameter> parameters;
        public SimulatorInterface(string filename)
        {
            string file = File.ReadAllText(filename);
            parameters = JsonConvert.DeserializeObject<List<Parameter>>(file);
            SetupTask = SetupAsync();
        }
        public virtual void Disconnect()
        {
            if (Disconnected) return;
            Active = false;
            Disconnected = true;
            try
            {
                Reader.Dispose();
                Writer.Dispose();
                client.Close();
            }
            catch(Exception)
            {
            }
        }

        async Task SetupAsync()
        {
            client = new TcpClient();
            await client.ConnectAsync("127.0.0.1", 5090);
            var stream = client.GetStream();
            Reader = new StreamReader(stream);
            Writer = new StreamWriter(stream);
            await SetupParameters();
        }

        Task ServerReadTask;
        Task SimReadTask;
        public async Task UpdateAsync()
        {
            if (SetupTask != null)
            {
                await SetupTask;
                SetupTask = null;
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
                        if (!TcpToSimulatorNames.ContainsKey(parameter.ServerName)) TcpToSimulatorNames[parameter.ServerName] = parameter;
                        await SendServer("register("+parameter.ServerName+')');
                    }
                }
                await Writer.FlushAsync();
            }
            if (!Active) return;
            if (ServerReadTask == null || ServerReadTask.IsCompleted) ServerReadTask = ReadServer();
            if (SimReadTask == null || SimReadTask.IsCompleted) SimReadTask = ReadSimulator();
            await Task.WhenAny(ServerReadTask, SimReadTask);
            if (ServerReadTask.IsFaulted) throw ServerReadTask.Exception;
            if (SimReadTask.IsFaulted) throw SimReadTask.Exception;
        }
        public async Task ReadServer()
        {
            string line = await Reader.ReadLineAsync();
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
                    await SendSimulator(parameter, value);
                }
            }
        }
        public abstract Task ReadSimulator();
        public abstract Task SendSimulator(Parameter parameter, double value);
        public abstract Task SetupParameters();
        public async Task SendServer(Parameter parameter, double value)
        {
            if (parameter.ServerMin.HasValue)
            {
                if (parameter.SimulatorMin.HasValue)
                {
                    value = (value - parameter.SimulatorMin.Value)/(parameter.SimulatorMax.Value-parameter.SimulatorMin.Value);
                }
                value = parameter.ServerMin.Value + value * (parameter.ServerMax.Value - parameter.ServerMin.Value);
            }
            await SendServer(parameter.ServerName, value.ToString().Replace(',','.'));
        }
        public async Task SendServer(string parameter, string value)
        {
            await SendServer(parameter+'='+value);
        }
        public async Task SendServer(string line)
        {
            await Writer.WriteLineAsync(line);
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
        public OrWeb() : base("parameters_or.json")
        {
        }
        public override async Task SetupParameters()
        {
            var indexes = new Dictionary<string, int>();
            List<ControlValue> values = await GetValues();
            while(values == null)
            {
                await Task.Delay(5000);
                values = await GetValues();
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

        public override async Task ReadSimulator()
        {
            var indexes = new Dictionary<string, int>();
            var values = await GetValues();
            foreach (var cv in values)
            {
                int index = 0;
                if (indexes.ContainsKey(cv.TypeName)) index = indexes[cv.TypeName] + 1;
                indexes[cv.TypeName] = index;
                if (SimulatorToTcpNames.TryGetValue(cv.TypeName+':'+index, out var parameter))
                {
                    double v = cv.MinValue + cv.RangeFraction * (cv.MaxValue - cv.MinValue);
                    if (cv.MaxValue - cv.MinValue == 0) v = cv.RangeFraction;
                    await SendServer(parameter, v);
                }
            }
            await Writer.FlushAsync();
            await Task.Delay(250);
        }
        public override async Task SendSimulator(Parameter parameter, double value)
        {
            var spl = parameter.SimulatorName.Split(':');
            ControlValuePost cv = new ControlValuePost
            {
                TypeName = spl[0],
                Value = value
            };
            if (spl.Length > 1) cv.ControlIndex = int.Parse(spl[1]);
            string json = JsonConvert.SerializeObject(new ControlValuePost[]{cv});
            var bytes = Encoding.UTF8.GetBytes(json);
            var request = WebRequest.Create("http://localhost:2150/API/CABCONTROLS");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = bytes.Length;
            using (var stream = request.GetRequestStream())
            {
                await stream.WriteAsync(bytes, 0, bytes.Length);
                using (var response = await request.GetResponseAsync()) {}
            }
        }
        async Task<List<ControlValue>> GetValues()
        {
            try
            {
                var request = WebRequest.Create("http://localhost:2150/API/CABCONTROLS");
                request.Method = "GET";
                var response = await request.GetResponseAsync();
                return JsonConvert.DeserializeObject<List<ControlValue>>(await new StreamReader(response.GetResponseStream()).ReadToEndAsync());
            }
            catch (Exception e)
            {
                if (Active) throw e;
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
        internal static extern float GetControllerValue(int Control, int Mode);
        [DllImport("RailDriver64.dll")]
        internal static extern void SetRailDriverConnected(bool Value);
        [DllImport("RailDriver64.dll")]
        internal static extern void SetRailSimConnected(bool Value);
        [DllImport("RailDriver64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool GetRailSimConnected();
        [DllImport("RailDriver64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr GetLocoName();
        [DllImport("RailDriver64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr GetControllerList();
        [DllImport("RailDriver64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool GetRailSimLocoChanged();
        Dictionary<Parameter, int> ParameterIndex = new Dictionary<Parameter, int>();
        Dictionary<Parameter, double> LastValues = new Dictionary<Parameter, double>();
        public string RWPath = "";
        public RwDll() : base("parameters_rw.json")
        {
            //Time = DateTime.UtcNow;
        }
        /*int SpeedometerControlIndex;
        double distance;
        double sentDistance;
        DateTime Time;*/
        string LocoName;
        public override async Task ReadSimulator()
        {
            if (Marshal.PtrToStringAnsi(GetLocoName()) != LocoName)
            {
                Disconnect();
                return;
            }
            foreach (var kvp in ParameterIndex)
            {
                var parameter = kvp.Key;
                if (!parameter.Write)
                {
                    double value = GetControllerValue(kvp.Value, 0);
                    if (!LastValues.TryGetValue(parameter, out double val) || Math.Abs(value-val) > 0.01f)
                    {
                        LastValues[parameter] = value;
                        await SendServer(parameter, value);
                    }
                }
            }
            /*double speed = GetControllerValue(SpeedometerControlIndex, 0)/3.6f;
            distance += Math.Abs(speed)*(DateTime.UtcNow-Time).TotalSeconds;
            Time = DateTime.UtcNow;
            if (distance - sentDistance > 1)
            {
                await SendServer("distance", distance.ToString().Replace(',','.'));
                sentDistance = distance;
            }*/
            await Writer.FlushAsync();
            await Task.Delay(100);
        }
        public override async Task SetupParameters()
        {
            RWPath = File.ReadAllLines("railworks_path.txt")[0];
            SetDllDirectory(Path.Combine(RWPath, "plugins"));
            string path = Path.Combine(RWPath, "plugins", "RailDriver64.dll");
            if (!File.Exists(path))
            {
                UnrecoverableFault = true;
                throw new FileNotFoundException("RailWorks path is invalid!", path);
            }
            LocoName = null;
            while (LocoName == null || LocoName == "")
            {
                SetRailDriverConnected(true);
                SetRailSimConnected(true);
                LocoName = Marshal.PtrToStringAnsi(GetLocoName());
                await Task.Delay(500);
            }
            Active = true;
            Console.WriteLine(LocoName);
            string tmp = Marshal.PtrToStringAnsi(GetControllerList());
            Console.WriteLine(tmp);
            var controls = tmp.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (var parameter in parameters)
            {
                int index = controls.IndexOf(parameter.SimulatorName);
                if (index < 0) continue;
                ParameterIndex[parameter] = index;
                if (!parameter.SimulatorMin.HasValue)
                {
                    parameter.SimulatorMin = GetControllerValue(index, 1);
                    parameter.SimulatorMax = GetControllerValue(index, 2);
                }
            }
            //SpeedometerControlIndex = controls.IndexOf("SpeedometerKPH");
        }
        public override async Task SendSimulator(Parameter parameter, double value)
        {
            if (ParameterIndex.TryGetValue(parameter, out int index))
            {
                SetControllerValue(index, (float)value);
            }
            await Task.CompletedTask;
        }
    }
}
