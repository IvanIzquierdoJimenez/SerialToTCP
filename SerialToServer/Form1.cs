using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Management;
using System.Xml.Linq;
using Timer = System.Windows.Forms.Timer;

namespace SerialToServer
{
    public partial class Form1 : Form
    { 

        SerialPort serial = new SerialPort();
        Dictionary<string, puente> puentes = new Dictionary<string, puente>();
        Dictionary<string, string> detailsPorts = new Dictionary<string, string>();
        Process process = new Process();
        OrWeb orWeb;
        RwDll rwDll;
        public bool isOpen = false;
        Timer timer = new Timer();
        public Form1()
        {
            InitializeComponent();
            RefreshPorts();
            descriptPort();
            Control.CheckForIllegalCrossThreadCalls = false;
            
            //process.StartInfo.FileName = @"server.exe";
            //process.Start();
            btnOpenServer.Text = "Stop Server";
            isOpen = true;
            
            timer.Interval = (10000);
            timer.Tick += new EventHandler((s,a) => {
                RefreshPorts();
                new Thread(() => {
                    if (orWeb != null && !orWeb.Active && orWeb.Failed)
                        orWeb = new OrWeb();
                }).Start();
            });
            timer.Start();
            new Thread(() => {
                Thread.Sleep(5000);
                cbEnableORTSTCP.Checked = true;
                cbxAllPorts.Checked = false;
            }).Start();
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            lbxConnected.Items.Add((string)lbxPortsDisp.SelectedItem);
            //puente p = new puente((string)lbxPortsDisp.SelectedItem);
            puentes.Add((string)lbxPortsDisp.SelectedItem, new puente((string)lbxPortsDisp.SelectedItem));
            try
            {
                lbxPortsDisp.Items.RemoveAt(lbxPortsDisp.SelectedIndex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbxPortsDisp.SelectedIndex.ToString() != null)
            {
                lbxPortsDisp.Items.Add((string)lbxConnected.SelectedItem);
                puente p = puentes[(string)lbxConnected.SelectedItem];
                p.Disconnect();
                puentes.Remove((string)lbxConnected.SelectedItem);
                lbxConnected.Items.RemoveAt(lbxConnected.SelectedIndex);
            }
        }

        void RefreshPorts()
        {
            lbxPortsDisp.Items.Clear();
            for (int i = 0; i < lbxConnected.Items.Count; i++)
            {
                if (puentes.TryGetValue(lbxConnected.Items[i].ToString(), out var p))
                {
                    if (p.Connected) continue;
                    puentes.Remove(lbxConnected.Items[i].ToString());
                }
                lbxConnected.Items.RemoveAt(i);
            }
            foreach (string port in SerialPort.GetPortNames())
            {
                if (port == "COM1" || port == "COM2") continue;
                bool exists = false;
                for (int i = 0; i < lbxConnected.Items.Count; i++)
                {
                    if (lbxConnected.Items[i].ToString() == port)
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                {
                    if (cbxAllPorts.Checked)
                    {
                        lbxConnected.Items.Add(port);
                        puentes.Add(port, new puente(port));
                    }
                    else lbxPortsDisp.Items.Add(port);
                }
            }
        }

        private void descriptPort()
        {
            /*ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * from WIN32_SerialPort");
            foreach (ManagementObject port in searcher.Get())
            {
                detailsPorts.Add((string)port.GetPropertyValue("DeviceID"), (string)port.GetPropertyValue("Description"));
            }*/
        }

        private void btnRefres_Click(object sender, EventArgs e)
        {
            new Thread(RefreshPorts).Start();
            //using (var searcher = new ManagementObjectSearcher("SELECT DeviceID, Caption, Description FROM WIN32_SerialPort"))
            //{
            //    string[] portnames = SerialPort.GetPortNames();

            //    var ports = searcher.Get().Cast<ManagementBaseObject>()
            //        .ToDictionary(p => p["DeviceID"].ToString(), p => p["Description"]);


            //    foreach (string name in portnames)
            //    {
            //        if (ports.TryGetValue(name, out var Description))
            //        {
            //            MessageBox.Show($"{name} - {Description}");
            //        }
            //        else
            //        {
            //            MessageBox.Show(name);
            //        }
            //    }
            //}
            //ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * from WIN32_SerialPort");
            //foreach (ManagementObject port in searcher.Get())
            //{
            //    tbFabricante.Text = (string)port.GetPropertyValue("Description");
            //}
        }
        private void btnOpenServer_Click(object sender, EventArgs e)
        {
            if (!isOpen)
            {
                process.Start();
                btnOpenServer.Text = "Stop Server";
                isOpen = true;
            }
            else if (isOpen) 
            {
                process.Kill();
                btnOpenServer.Text = "Start Server";
                isOpen = false;
            }
        }

        private void lbxPortsDisp_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    ListBox lbx = (ListBox)sender;
            //    string puerto = lbx.SelectedItem.ToString();
            //    tbFabricante.Text = detailsPorts[puerto];
            //}
            //catch(Exception ex) { }
        }

        private void cbEnableORTSTCP_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            bool check = cb.Checked;
            if(check)
            {
                if (orWeb != null) orWeb.Active = false;
                new Thread(() => orWeb = new OrWeb()).Start();
            }
            else
            {
                if (orWeb != null)
                {
                    orWeb.Active = false;
                    orWeb = null;
                }
                lbxControllers.Items.Clear();
            }
        }

        private void cbEnableRWTCP_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            bool check = cb.Checked;
            if(check)
            {
                if (rwDll != null) rwDll.Active = false;
                new Thread(() => rwDll = new RwDll()).Start();
            }
            else
            {
                if (rwDll != null)
                {
                    rwDll.Active = false;
                    rwDll = null;
                }
                lbxControllers.Items.Clear();
            }
        }

        private void btnConfigParametros_Click(object sender, EventArgs e)
        {
            JSonConfigurador jc = new JSonConfigurador();
            jc.ShowDialog();
        }
    }
}
