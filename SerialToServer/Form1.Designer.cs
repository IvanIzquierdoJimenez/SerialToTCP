namespace SerialToServer
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpArduino = new System.Windows.Forms.TabPage();
            this.btnOpenServer = new System.Windows.Forms.Button();
            this.btnRefres = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxAllPorts = new System.Windows.Forms.CheckBox();
            this.lbxConnected = new System.Windows.Forms.ListBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lbxPortsDisp = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tpArduino.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpArduino);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(3, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(794, 448);
            this.tabControl1.TabIndex = 0;
            // 
            // tpArduino
            // 
            this.tpArduino.Controls.Add(this.btnOpenServer);
            this.tpArduino.Controls.Add(this.btnRefres);
            this.tpArduino.Controls.Add(this.textBox2);
            this.tpArduino.Controls.Add(this.label2);
            this.tpArduino.Controls.Add(this.textBox1);
            this.tpArduino.Controls.Add(this.label1);
            this.tpArduino.Controls.Add(this.cbxAllPorts);
            this.tpArduino.Controls.Add(this.lbxConnected);
            this.tpArduino.Controls.Add(this.btnDelete);
            this.tpArduino.Controls.Add(this.btnAdd);
            this.tpArduino.Controls.Add(this.lbxPortsDisp);
            this.tpArduino.Location = new System.Drawing.Point(4, 29);
            this.tpArduino.Name = "tpArduino";
            this.tpArduino.Padding = new System.Windows.Forms.Padding(3);
            this.tpArduino.Size = new System.Drawing.Size(786, 415);
            this.tpArduino.TabIndex = 0;
            this.tpArduino.Text = "Arduino";
            this.tpArduino.UseVisualStyleBackColor = true;
            // 
            // btnOpenServer
            // 
            this.btnOpenServer.Location = new System.Drawing.Point(16, 35);
            this.btnOpenServer.Name = "btnOpenServer";
            this.btnOpenServer.Size = new System.Drawing.Size(136, 41);
            this.btnOpenServer.TabIndex = 14;
            this.btnOpenServer.Text = "Start Server";
            this.btnOpenServer.UseVisualStyleBackColor = true;
            this.btnOpenServer.Click += new System.EventHandler(this.btnOpenServer_Click);
            // 
            // btnRefres
            // 
            this.btnRefres.Location = new System.Drawing.Point(332, 209);
            this.btnRefres.Name = "btnRefres";
            this.btnRefres.Size = new System.Drawing.Size(114, 39);
            this.btnRefres.TabIndex = 13;
            this.btnRefres.Text = "Refrescar";
            this.btnRefres.UseVisualStyleBackColor = true;
            this.btnRefres.Click += new System.EventHandler(this.btnRefres_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(326, 359);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(116, 26);
            this.textBox2.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(290, 362);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "ID:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(107, 359);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(148, 26);
            this.textBox1.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 362);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "Fabricante:";
            // 
            // cbxAllPorts
            // 
            this.cbxAllPorts.AutoSize = true;
            this.cbxAllPorts.Location = new System.Drawing.Point(128, 307);
            this.cbxAllPorts.Name = "cbxAllPorts";
            this.cbxAllPorts.Size = new System.Drawing.Size(264, 24);
            this.cbxAllPorts.TabIndex = 8;
            this.cbxAllPorts.Text = "Conectar todo automáticamente";
            this.cbxAllPorts.UseVisualStyleBackColor = true;
            // 
            // lbxConnected
            // 
            this.lbxConnected.FormattingEnabled = true;
            this.lbxConnected.ItemHeight = 20;
            this.lbxConnected.Location = new System.Drawing.Point(452, 15);
            this.lbxConnected.Name = "lbxConnected";
            this.lbxConnected.Size = new System.Drawing.Size(159, 264);
            this.lbxConnected.TabIndex = 7;
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(351, 138);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 50);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "<";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(351, 73);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 49);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = ">";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lbxPortsDisp
            // 
            this.lbxPortsDisp.FormattingEnabled = true;
            this.lbxPortsDisp.ItemHeight = 20;
            this.lbxPortsDisp.Location = new System.Drawing.Point(167, 15);
            this.lbxPortsDisp.Name = "lbxPortsDisp";
            this.lbxPortsDisp.Size = new System.Drawing.Size(159, 264);
            this.lbxPortsDisp.TabIndex = 4;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(786, 415);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "GUI";
            this.tabControl1.ResumeLayout(false);
            this.tpArduino.ResumeLayout(false);
            this.tpArduino.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpArduino;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox lbxConnected;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListBox lbxPortsDisp;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbxAllPorts;
        private System.Windows.Forms.Button btnRefres;
        private System.Windows.Forms.Button btnOpenServer;
    }
}

