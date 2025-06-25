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
using System.Runtime.InteropServices;
using System.Management;
using System.Xml.Linq;
using Timer = System.Windows.Forms.Timer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SerialToServer
{
    public partial class Form1 : Form
    { 

        SerialPort serial = new SerialPort();
        Dictionary<string, puente> puentes = new Dictionary<string, puente>();
        Dictionary<string, Task> puentesTask = new Dictionary<string, Task>();
        Dictionary<string, string> detailsPorts = new Dictionary<string, string>();
        Process process = new Process();
        OrWeb orWeb;
        RwDll rwDll;
        public bool isOpen = false;
        Timer timer = new Timer();
        CancellationTokenSource Cancellation = new CancellationTokenSource();
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int AllocConsole();
        public static bool Mono = false;
        public Form1()
        {
#if DEBUG
            Mono = Type.GetType ("Mono.Runtime") != null;
            if (!Mono)
            {
                try
                {
                    AllocConsole();
                }
                catch(Exception e)
                {

                }
            }
#endif
            InitializeComponent();
            RefreshPorts();
            descriptPort();
            
            process.StartInfo.FileName = @"server.exe";
            process.StartInfo.UseShellExecute = false;
#if !DEBUG
            process.StartInfo.CreateNoWindow = true;
#endif
            process.Start();
            btnOpenServer.Text = "Stop Server";
            isOpen = true;

            AppDomain.CurrentDomain.ProcessExit += (s, a) => {
                if (isOpen)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch(Exception)
                    {
                        
                    }
                }
            };
            
            timer.Interval = 10000;
            timer.Tick += new EventHandler((s,a) => {
                RefreshPorts();
                bool cancel = false;
                if (orWeb != null && !orWeb.Active && orWeb.Disconnected)
                {
                    orWeb = new OrWeb();
                    cancel = true;
                }
                if (rwDll != null && !rwDll.Active && rwDll.Disconnected)
                {
                    rwDll = new RwDll();
                    cancel = true;
                }
                foreach (var key in puentes.Keys.ToList())
                {
                    if (puentes[key].Disconnected)
                    {
                        puentes[key] = new puente(key);
                        cancel = true;
                    }
                }
                if (cancel)
                {
                    var oldCancellation = Cancellation;
                    Cancellation = new CancellationTokenSource();
                    oldCancellation.Cancel();
                }
            });
            timer.Start();
            Task.Run(async () => {
                await Task.Delay(5000);
                cbEnableORTSTCP.Checked = true;
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                cbEnableRWTCP.Checked = true;
                cbxAllPorts.Checked = true;
            });
            _ = UpdateLoopAsync();
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (lbxPortsDisp.SelectedIndex < 0) return;
            lbxConnected.Items.Add((string)lbxPortsDisp.SelectedItem);
            puentes.Add((string)lbxPortsDisp.SelectedItem, new puente((string)lbxPortsDisp.SelectedItem));
            var oldCancellation = Cancellation;
            Cancellation = new CancellationTokenSource();
            oldCancellation.Cancel();
            try
            {
                lbxPortsDisp.Items.RemoveAt(lbxPortsDisp.SelectedIndex);
            }
            catch (Exception ex)
            {
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbxConnected.SelectedIndex < 0) return;
            lbxPortsDisp.Items.Add((string)lbxConnected.SelectedItem);
            puente p = puentes[(string)lbxConnected.SelectedItem];
            p.Disconnect();
            puentes.Remove((string)lbxConnected.SelectedItem);
            try
            {
                lbxConnected.Items.RemoveAt(lbxConnected.SelectedIndex);
            }
            catch (Exception ex)
            {
            }
        }

        void RefreshPorts()
        {
            var selDisp = lbxPortsDisp.SelectedItem;
            var selCon = lbxConnected.SelectedItem;
            lbxPortsDisp.Items.Clear();
            lbxConnected.Items.Clear();
            var ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                if (port == "COM1" || port == "COM2") continue;
                if (puentes.ContainsKey(port)) lbxConnected.Items.Add(port);
                else if (cbxAllPorts.Checked && !port.StartsWith("/dev/ttyS") && !port.StartsWith("COM1") && !port.StartsWith("COM2"))
                {
                    lbxConnected.Items.Add(port);
                    puentes.Add(port, new puente(port));
                    var oldCancellation = Cancellation;
                    Cancellation = new CancellationTokenSource();
                    oldCancellation.Cancel();
                }
                else lbxPortsDisp.Items.Add(port);
            }
            if (selDisp != null && lbxPortsDisp.Items.Contains(selDisp)) lbxPortsDisp.SelectedItem = selDisp;
            if (selCon != null && lbxConnected.Items.Contains(selCon)) lbxConnected.SelectedItem = selCon;
            foreach (var key in puentes.Keys.ToList())
            {
                bool exists = false;
                foreach (string port in ports)
                {
                    if (port == key)
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                {
                    puentes[key].Disconnect();
                    puentes.Remove(key);
                }
            }
        }

        Dictionary<string,Task> tasks = new Dictionary<string,Task>();
        public async Task UpdateLoopAsync()
        {
            while(true)
            {
                foreach (var puente in puentes)
                {
                    if (!tasks.ContainsKey(puente.Key) && !puente.Value.Disconnected) tasks[puente.Key] = puente.Value.UpdateAsync();
                }
                if (orWeb != null && orWeb.UnrecoverableFault)
                {
                    cbEnableORTSTCP.Checked = false;
                }
                if (rwDll != null && rwDll.UnrecoverableFault)
                {
                    cbEnableRWTCP.Checked = false;
                }
                if (!tasks.ContainsKey("RwDll") && rwDll != null && !rwDll.Disconnected) tasks["RwDll"] = rwDll.UpdateAsync();
                if (!tasks.ContainsKey("OrWeb") && orWeb != null && !orWeb.Disconnected) tasks["OrWeb"] = orWeb.UpdateAsync();
                if (!tasks.ContainsKey("Cancel")) tasks["Cancel"] = Task.Delay(-1, Cancellation.Token);
                await Task.WhenAny(tasks.Values);
                foreach (var key in tasks.Keys.ToList())
                {
                    var task = tasks[key];
                    if (task.IsCompleted) tasks.Remove(key);
                    if (task.IsFaulted)
                    {
                        switch (key)
                        {
                            case "OrWeb":
                                orWeb?.Disconnect();
                                break;
                            case "RwDll":
                                rwDll?.Disconnect();
                                break;
                            case "Cancel":
                                break;
                            default:
                                if (puentes.TryGetValue(key, out var puente))
                                {
                                    puente.Disconnect();
                                }
                                break;
                        }
                        Console.WriteLine(task.Exception);
                    }
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
            RefreshPorts();
        }
        private void btnOpenServer_Click(object sender, EventArgs e)
        {
            if (!isOpen)
            {
                try
                {
                    process.Start();
                }
                catch (Exception)
                {

                }
                btnOpenServer.Text = "Stop Server";
                isOpen = true;
            }
            else if (isOpen) 
            {
                try
                {
                    process.Kill();
                }
                catch (Exception ex)
                {

                }
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
                orWeb?.Disconnect();
                orWeb = new OrWeb();
                var oldCancellation = Cancellation;
                Cancellation = new CancellationTokenSource();
                oldCancellation.Cancel();
            }
            else
            {
                if (orWeb != null)
                {
                    orWeb.Disconnect();
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
                rwDll?.Disconnect();
                rwDll = new RwDll();
                var oldCancellation = Cancellation;
                Cancellation = new CancellationTokenSource();
                oldCancellation.Cancel();
            }
            else
            {
                if (rwDll != null)
                {
                    rwDll.Disconnect();
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
