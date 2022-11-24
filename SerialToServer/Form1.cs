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

namespace SerialToServer
{
    public partial class Form1 : Form
    { 

        SerialPort serial = new SerialPort();
        Dictionary<string, puente> puentes = new Dictionary<string, puente>();
        public Form1()
        {
            InitializeComponent();
            RefreshPorts();
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
            foreach (string port in SerialPort.GetPortNames())
            {
                bool exists = false;
                for (int i = 0; i < lbxConnected.Items.Count; i++)
                {
                    if (lbxConnected.Items[i].ToString() == port)
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists) lbxPortsDisp.Items.Add(port);
            }
        }
        
        private void btnRefres_Click(object sender, EventArgs e)
        {
            RefreshPorts();
        }
    }
}
