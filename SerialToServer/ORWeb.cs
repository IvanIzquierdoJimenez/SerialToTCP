﻿using System;
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
using Timer = System.Windows.Forms.Timer;

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
        public SimulatorInterface()
        {
            client = new TcpClient("127.0.0.1", 5090);
            stream = client.GetStream();
            reader = new StreamReader(stream);

            string file = File.ReadAllText("parameters.json");
            List<Parameter> parameters = JsonConvert.DeserializeObject<List<Parameter>>(file);
            SetupParameters(parameters);
            Console.WriteLine("Parameters");
            Console.WriteLine("Size:" + parameters.Count);
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
                    Console.WriteLine("register("+parameter.ServerName+")\n");
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
            if (parameter.SimulatorMin.HasValue)
            {
                value = parameter.SimulatorMin.Value + value * (parameter.SimulatorMax.Value-parameter.SimulatorMin.Value);
            }
            if (parameter.ServerMin.HasValue)
            {
                value = (value - parameter.ServerMin.Value)*(parameter.ServerMax.Value - parameter.ServerMin.Value);
            }
            var data = Encoding.UTF8.GetBytes(parameter.ServerName+"="+value+"\n");
            stream.Write(data, 0, data.Length);
        }
        void TCPToOR()
        {
            while (true)
            {
                string line = reader.ReadLine();
                Console.WriteLine(line);
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
                        }
                        if (parameter.SimulatorMin != null)
                        {
                            value = parameter.SimulatorMin.Value + value * (parameter.SimulatorMax.Value-parameter.SimulatorMin.Value);
                        }
                        SendSimulator(parameter, value);
                    }
                }
            }
        }
    }
    public class ORWeb : SimulatorInterface
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
        Timer timer = new Timer();
        public ORWeb()
        {
            timer.Interval = 250;
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        public override void SetupParameters(List<Parameter> parameters)
        {
            var indexes = new Dictionary<string, int>();
            foreach (var cv in GetValues())
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

        private void Timer_Tick(object sender, EventArgs e)
        {
            var indexes = new Dictionary<string, int>();
            foreach (var cv in GetValues())
            {
                int index = 0;
                if (indexes.ContainsKey(cv.TypeName)) index = indexes[cv.TypeName] + 1;
                indexes[cv.TypeName] = index;
                if (SimulatorToTcpNames.TryGetValue(cv.TypeName+':'+index, out var parameter))
                {
                    SendServer(parameter, cv.RangeFraction);
                }
            }
        }
        public override void SendSimulator(Parameter parameter, double value)
        {
            var spl = parameter.SimulatorName.Split(':');
            ControlValuePost cv = new ControlValuePost();
            cv.TypeName = spl[0];
            if (spl.Length > 1) cv.ControlIndex = int.Parse(spl[1]);
            cv.Value = value;
            var request = WebRequest.Create("http://localhost:2150/API/CABCONTROLS");
            request.Method = "POST";
            request.ContentType = "application/json";
            string json = JsonConvert.SerializeObject(new ControlValuePost[]{cv});
            var bytes = Encoding.UTF8.GetBytes(json);
            request.ContentLength = bytes.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                using (var response = request.GetResponse()) {}
            }
        }
        List<ControlValue> GetValues()
        {
            var request = WebRequest.Create("http://localhost:2150/API/CABCONTROLS");
            request.Method = "GET";
            return JsonConvert.DeserializeObject<List<ControlValue>>(new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd());
        }
    }
    public class RWDll : SimulatorInterface
    {
        [DllImport("RailDriver64.dll")]
        internal static extern void SetControllerValue(int Control, float Value);
        [DllImport("RailDriver64.dll")]
        internal static extern Single GetControllerValue(int Control, int Mode);
        [DllImport("RailDriver64.dll")]
        internal static extern void SetRailDriverConnected(bool Value);
        [DllImport("RailDriver64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr GetLocoName();
        [DllImport("RailDriver64.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr GetControllerList();
        Timer timer = new Timer();
        Dictionary<Parameter, int> ParameterIndex = new Dictionary<Parameter, int>();
        Dictionary<Parameter, double> LastValues;
        public RWDll()
        {
            timer.Interval = 100;
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
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
        }
        public override void SetupParameters(List<Parameter> parameters)
        {
            string tmp = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(GetControllerList());
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
        }
        public override void SendSimulator(Parameter parameter, double value)
        {
            if (ParameterIndex.TryGetValue(parameter, out int index))
            {
                SetControllerValue(index, (float)value);
            }
        }
    }
}
