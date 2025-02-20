using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;
using System.Net.Sockets;
using System.IO;

namespace SerialToServer
{
    internal class puente
    {
        private SerialPort serial;
        private TcpClient c;
        private StreamReader srTcp;
        private StreamReader srSerial;
        private StreamWriter swTcp;
        private StreamWriter swSerial;
        public string Port;
        public bool Connected;
        public bool Disconnected;
        Task SetupTask;
        Task SerialToTcpTask;
        Task TcpToSerialTask;
        public puente(string port)
        {
            Console.WriteLine("Puente: "+port);
            Port = port;
            SetupTask = Setup();
        }

        public async Task Setup()
        {
            c = new TcpClient();
            await c.ConnectAsync("127.0.0.1", 5090);
            var stream = c.GetStream();
            srTcp = new StreamReader(stream);
            swTcp = new StreamWriter(stream);

            serial = new SerialPort(Port, 115200)
            {
                Encoding = Encoding.UTF8
            };
            serial.Open();
            serial.DtrEnable = true;
            serial.RtsEnable = true;
            await Task.Delay(1000);
            //serial.DtrEnable = false;
            serial.RtsEnable = false;
            srSerial = new StreamReader(serial.BaseStream);
            swSerial = new StreamWriter(serial.BaseStream);
            await Task.Delay(4000);

            Connected = true;
        }

        public async Task SerialToTcpAsync()
        {
            string line = await srSerial.ReadLineAsync();
            await swTcp.WriteLineAsync(line);
            await swTcp.FlushAsync();
        }

        public async Task TcpToSerialAsync()
        {
            string line = await srTcp.ReadLineAsync();
            if (Form1.Mono) serial.WriteLine(line);
            else await swSerial.WriteLineAsync(line);
            await swSerial.FlushAsync();
        }

        public async Task UpdateAsync()
        {
            if (SetupTask != null)
            {
                await SetupTask;
                SetupTask = null;
            }
            if (!Connected) return;
            if (SerialToTcpTask == null || SerialToTcpTask.IsCompleted) SerialToTcpTask = SerialToTcpAsync();
            if (TcpToSerialTask == null || TcpToSerialTask.IsCompleted) TcpToSerialTask = TcpToSerialAsync();
            await Task.WhenAny(SerialToTcpTask, TcpToSerialTask);
            if (SerialToTcpTask.IsFaulted) throw SerialToTcpTask.Exception;
            if (TcpToSerialTask.IsFaulted) throw TcpToSerialTask.Exception;
        }

        public void Disconnect()
        {
            Console.WriteLine("Disconnect: "+Port);
            if (Disconnected) return;
            Disconnected = true;
            Connected = false;
            try
            {
                srSerial.Close();
                srTcp.Close();
                swSerial.Close();
                swTcp.Close();
                serial.Close();
                c.Close();
            }
            catch (Exception)
            {
            }
        }
    }
}
