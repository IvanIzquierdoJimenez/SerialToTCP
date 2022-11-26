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
using Timer = System.Windows.Forms.Timer;

namespace SerialToServer
{

    public class controllers
    {
        public string TypeName { get; set; }
        public string MinValue { get; set; }
        public string MaxValue { get; set; }
        public string RangeFraction { get; set; }

    }
    internal class ORWeb
    {
        private WebRequest request;
        private Thread t;
        private TcpClient c;
        private Timer timer = new Timer();
        private string dataRead;
        public ORWeb()
        {
            c = new TcpClient("127.0.0.1", 5090);
            request = WebRequest.Create("http://localhost:2150/API/CABCONTROLS");
            request.Method = "GET";
            timer.Interval = 250;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var webResponse = request.GetResponse();
            var webStream = webResponse.GetResponseStream();
            var reader = new StreamReader(webStream);
            dataRead = reader.ReadToEnd();
        }

        public List<controllers> readJsonTCP()
        {
            List<controllers> data = JsonConvert.DeserializeObject<List<controllers>>(dataRead);
            return data;
        }
    }
}
