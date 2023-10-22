using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialToServer
{
    public partial class JSonConfigurador : Form
    {
        List<Parameter> parameters;
        public struct ControlValue
        {
            public string TypeName;
            public double MinValue;
            public double MaxValue;
            public double RangeFraction;

            public override string ToString()
            {
                return TypeName;;
            }
        }

        public struct Parameter
        {
            public string ServerName;
            public string SimulatorName;
            public double? ServerMin;
            public double? ServerMax;
            public double? SimulatorMin;
            public double? SimulatorMax;
            public bool Write;

            public override string ToString()
            {
                return ServerName;
            }
        }

        public JSonConfigurador()
        {
            InitializeComponent();
            List<ControlValue> values = GetValues(); // Obtener la lista de valores desde la URL
            for (int i = 0; i < values.Count; i++)
            {
                lboxParametros.Items.Add(values[i]);
            }
        }
        List<ControlValue> GetValues()
        {
            var request = WebRequest.Create("http://localhost:2150/API/CABCONTROLS");
            request.Method = "GET";
            return JsonConvert.DeserializeObject<List<ControlValue>>(new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd());
        }

        void readParametersJSON()
        {
            // Leer el archivo JSON
            string jsonString = File.ReadAllText("parameters.json");

            // Convertir la cadena JSON en una lista de objetos
            parameters = JsonConvert.DeserializeObject<List<Parameter>>(jsonString);

            // Agregar los objetos al ListBox y establecer la propiedad DisplayMember
            //lboxJSON.DataSource = parameters;
            // Limpiar la ListBox antes de agregar elementos
            lboxJSON.Items.Clear();

            // Agregar elementos a la ListBox
            foreach (var parameter in parameters)
            {
                lboxJSON.Items.Add(parameter);
            }
        }

        private void JSonConfigurador_Load(object sender, EventArgs e)
        {
            //// Leer el archivo JSON
            //string jsonString = File.ReadAllText("../Debug/parameters.json");

            //// Convertir la cadena JSON en una lista de objetos
            //parameters = JsonConvert.DeserializeObject<List<Parameter>>(jsonString);

            //// Agregar los objetos al ListBox y establecer la propiedad DisplayMember
            //lboxJSON.DataSource = parameters;
            readParametersJSON();
        }

        private void lboxParametros_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lboxParametros.SelectedItem != null && lboxParametros.SelectedItem is ControlValue valorSeleccionado)
    {
                tbParameter.Text = valorSeleccionado.TypeName.ToString();
                tbMin.Text = valorSeleccionado.MinValue.ToString();
                tbMax.Text = valorSeleccionado.MaxValue.ToString();
                tbRange.Text = valorSeleccionado.RangeFraction.ToString();
            }
        }

        private void lboxJSON_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Verificar si hay un elemento seleccionado
            if (lboxJSON.SelectedItem != null)
            {
                // Obtener el objeto seleccionado en el ListBox
                Parameter p = (Parameter)lboxJSON.SelectedItem;

                // Actualizar los valores de los TextBox con los valores del objeto seleccionado
                //tbServerName.Text = p.ServerName;
                tbParameter.Text = p.SimulatorName;
                tbServer.Text = p.ServerName;
                tbServerMin.Text = p.ServerMin.ToString();
                tbServerMax.Text = p.ServerMax.ToString();
                tbSimulatorMin.Text = p.SimulatorMin.ToString();
                tbSimulatorMax.Text = p.SimulatorMax.ToString();
                cbWrite.Checked = p.Write;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Leer el archivo JSON
            string jsonString = File.ReadAllText("parameters.json");

            // Convertir la lista de objetos de vuelta en una cadena JSON
            jsonString = JsonConvert.SerializeObject(parameters, Formatting.Indented);

            // Escribir la cadena en el archivo JSON
            File.WriteAllText("parameters.json", jsonString);

            readParametersJSON();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string jsonString = File.ReadAllText("parameters.json");
            // Crear un nuevo objeto con los valores de los TextBox
            Parameter newObject = new Parameter();
            newObject.ServerName = tbServer.Text;
            newObject.SimulatorName = tbParameter.Text;
            if (tbServerMin.Text != "") newObject.ServerMin = double.Parse(tbServerMin.Text);
            if (tbServerMax.Text != "") newObject.ServerMax = double.Parse(tbServerMax.Text);
            if (tbSimulatorMin.Text != "") newObject.SimulatorMin = double.Parse(tbSimulatorMin.Text);
            if (tbSimulatorMax.Text != "") newObject.SimulatorMax = double.Parse(tbSimulatorMax.Text);
            newObject.Write = cbWrite.Checked;

            // Agregar el nuevo objeto a la lista
            parameters.Add(newObject);

            // Convertir la lista de objetos de vuelta en una cadena JSON
            jsonString = JsonConvert.SerializeObject(parameters, Formatting.Indented);

            // Escribir la cadena en el archivo JSON
            File.WriteAllText("parameters.json", jsonString);

            readParametersJSON();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lboxJSON.SelectedIndex != -1)
            {
                DialogResult result = MessageBox.Show("¿Está seguro de que desea eliminar este elemento?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Obtener el índice del elemento seleccionado y eliminarlo de la lista
                    int index = lboxJSON.SelectedIndex;
                    lboxJSON.Items.RemoveAt(index);

                    // Leer el archivo JSON
                    string jsonString = File.ReadAllText("parameters.json");

                    // Eliminar el objeto de la lista
                    parameters.RemoveAt(index);

                    // Convertir la lista de objetos de vuelta en una cadena JSON
                    jsonString = JsonConvert.SerializeObject(parameters, Formatting.Indented);

                    // Escribir la cadena en el archivo JSON
                    File.WriteAllText("parameters.json", jsonString);
                }
            }
            else
            {
                MessageBox.Show("Seleccione un elemento para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            readParametersJSON();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (lboxJSON.SelectedItem != null)
            {
                // Obtener el objeto seleccionado en la ListBox
                Parameter selectedParameter = (Parameter)lboxJSON.SelectedItem;

                // Realizar las modificaciones en el objeto seleccionado
                selectedParameter.ServerName = tbServer.Text;
                selectedParameter.SimulatorName = tbParameter.Text;
                if (!string.IsNullOrEmpty(tbServerMin.Text))
                {
                    selectedParameter.ServerMin = double.Parse(tbServerMin.Text);
                }
                if (!string.IsNullOrEmpty(tbServerMax.Text))
                {
                    selectedParameter.ServerMax = double.Parse(tbServerMax.Text);
                }
                if (!string.IsNullOrEmpty(tbSimulatorMin.Text))
                {
                    selectedParameter.SimulatorMin = double.Parse(tbSimulatorMin.Text);
                }
                if (!string.IsNullOrEmpty(tbSimulatorMax.Text))
                {
                    selectedParameter.SimulatorMax = double.Parse(tbSimulatorMax.Text);
                }
                selectedParameter.Write = cbWrite.Checked;

                // Convertir la lista de objetos de vuelta en una cadena JSON
                string jsonString = JsonConvert.SerializeObject(parameters, Formatting.Indented);

                // Escribir la cadena en el archivo JSON
                File.WriteAllText("parameters.json", jsonString);

                // Actualizar la ListBox para reflejar los cambios
                lboxJSON.Refresh();
            }
        }

    }
}
