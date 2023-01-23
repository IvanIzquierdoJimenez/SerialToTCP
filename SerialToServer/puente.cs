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
        private NetworkStream network;
        private StreamReader sr;
        public bool Connected;
        public puente(string Port)
        {
            serial = new SerialPort(Port, 115200);
            c = new TcpClient("127.0.0.1", 5090);
            network = c.GetStream();
            sr = new StreamReader(network);
            serial.Open();
            Thread.Sleep(5000);
            Connected = true;
            Thread SerialTCP = new Thread(SerialToTCP);
            Thread TCPSerial = new Thread(TCPToSerial);
            SerialTCP.Start();
            TCPSerial.Start();
        }

        private void SerialToTCP()
        {
            while (Connected)
            {
                try
                { 
                    var data = Encoding.UTF8.GetBytes(serial.ReadLine()+'\n');
                    network.Write(data, 0, data.Length);
                }
                catch (Exception e)
                {
                    Connected = false;
                    Console.WriteLine(e.ToString());
                }
            }
        }

        private void TCPToSerial()
        {
            while (Connected)
            {
                try
                {
                    serial.WriteLine(sr.ReadLine());
                }
                catch (Exception e)
                {
                    Connected = false;
                    Console.WriteLine(e.ToString());
                }
            }
        }

        public void Disconnect()
        {
            if (!Connected) return;
            Connected = false;
            serial.Close();
            c.Close();
        }
    }
}
