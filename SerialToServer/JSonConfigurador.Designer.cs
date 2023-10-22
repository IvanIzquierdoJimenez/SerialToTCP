namespace SerialToServer
{
    partial class JSonConfigurador
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JSonConfigurador));
            this.lboxParametros = new System.Windows.Forms.ListBox();
            this.tbParameter = new System.Windows.Forms.TextBox();
            this.tbMin = new System.Windows.Forms.TextBox();
            this.tbMax = new System.Windows.Forms.TextBox();
            this.tbRange = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbWrite = new System.Windows.Forms.CheckBox();
            this.tbServer = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.tbSimulatorMin = new System.Windows.Forms.TextBox();
            this.tbSimulatorMax = new System.Windows.Forms.TextBox();
            this.tbServerMax = new System.Windows.Forms.TextBox();
            this.tbServerMin = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lboxJSON = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnModificar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lboxParametros
            // 
            this.lboxParametros.FormattingEnabled = true;
            this.lboxParametros.Location = new System.Drawing.Point(8, 44);
            this.lboxParametros.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lboxParametros.Name = "lboxParametros";
            this.lboxParametros.Size = new System.Drawing.Size(349, 147);
            this.lboxParametros.TabIndex = 0;
            this.lboxParametros.SelectedIndexChanged += new System.EventHandler(this.lboxParametros_SelectedIndexChanged);
            // 
            // tbParameter
            // 
            this.tbParameter.Location = new System.Drawing.Point(8, 222);
            this.tbParameter.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbParameter.Name = "tbParameter";
            this.tbParameter.ReadOnly = true;
            this.tbParameter.Size = new System.Drawing.Size(271, 20);
            this.tbParameter.TabIndex = 1;
            // 
            // tbMin
            // 
            this.tbMin.Location = new System.Drawing.Point(289, 222);
            this.tbMin.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbMin.Name = "tbMin";
            this.tbMin.ReadOnly = true;
            this.tbMin.Size = new System.Drawing.Size(94, 20);
            this.tbMin.TabIndex = 2;
            // 
            // tbMax
            // 
            this.tbMax.Location = new System.Drawing.Point(386, 222);
            this.tbMax.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbMax.Name = "tbMax";
            this.tbMax.ReadOnly = true;
            this.tbMax.Size = new System.Drawing.Size(100, 20);
            this.tbMax.TabIndex = 3;
            // 
            // tbRange
            // 
            this.tbRange.Location = new System.Drawing.Point(489, 222);
            this.tbRange.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbRange.Name = "tbRange";
            this.tbRange.ReadOnly = true;
            this.tbRange.Size = new System.Drawing.Size(119, 20);
            this.tbRange.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(89, 204);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Nombre Simulador";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(323, 204);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Min";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(419, 204);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Max";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(526, 204);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Rango";
            // 
            // cbWrite
            // 
            this.cbWrite.AutoSize = true;
            this.cbWrite.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbWrite.Location = new System.Drawing.Point(672, 273);
            this.cbWrite.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbWrite.Name = "cbWrite";
            this.cbWrite.Size = new System.Drawing.Size(60, 21);
            this.cbWrite.TabIndex = 9;
            this.cbWrite.Text = "Write";
            this.cbWrite.UseVisualStyleBackColor = true;
            // 
            // tbServer
            // 
            this.tbServer.Location = new System.Drawing.Point(8, 275);
            this.tbServer.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbServer.Name = "tbServer";
            this.tbServer.Size = new System.Drawing.Size(238, 20);
            this.tbServer.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(69, 250);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "Nombre Server";
            // 
            // btnGuardar
            // 
            this.btnGuardar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuardar.Location = new System.Drawing.Point(489, 352);
            this.btnGuardar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(119, 35);
            this.btnGuardar.TabIndex = 12;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // tbSimulatorMin
            // 
            this.tbSimulatorMin.Location = new System.Drawing.Point(255, 275);
            this.tbSimulatorMin.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbSimulatorMin.Name = "tbSimulatorMin";
            this.tbSimulatorMin.Size = new System.Drawing.Size(93, 20);
            this.tbSimulatorMin.TabIndex = 13;
            // 
            // tbSimulatorMax
            // 
            this.tbSimulatorMax.Location = new System.Drawing.Point(359, 275);
            this.tbSimulatorMax.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbSimulatorMax.Name = "tbSimulatorMax";
            this.tbSimulatorMax.Size = new System.Drawing.Size(93, 20);
            this.tbSimulatorMax.TabIndex = 14;
            // 
            // tbServerMax
            // 
            this.tbServerMax.Location = new System.Drawing.Point(570, 275);
            this.tbServerMax.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbServerMax.Name = "tbServerMax";
            this.tbServerMax.Size = new System.Drawing.Size(93, 20);
            this.tbServerMax.TabIndex = 16;
            // 
            // tbServerMin
            // 
            this.tbServerMin.Location = new System.Drawing.Point(465, 275);
            this.tbServerMin.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbServerMin.Name = "tbServerMin";
            this.tbServerMin.Size = new System.Drawing.Size(93, 20);
            this.tbServerMin.TabIndex = 15;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdd.BackgroundImage")));
            this.btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.Location = new System.Drawing.Point(618, 204);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(40, 40);
            this.btnAdd.TabIndex = 17;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lboxJSON
            // 
            this.lboxJSON.FormattingEnabled = true;
            this.lboxJSON.Location = new System.Drawing.Point(369, 44);
            this.lboxJSON.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lboxJSON.Name = "lboxJSON";
            this.lboxJSON.Size = new System.Drawing.Size(377, 147);
            this.lboxJSON.TabIndex = 18;
            this.lboxJSON.SelectedIndexChanged += new System.EventHandler(this.lboxJSON_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(121, 16);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(133, 17);
            this.label6.TabIndex = 19;
            this.label6.Text = "Parametros OR/RW";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(500, 16);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 17);
            this.label7.TabIndex = 20;
            this.label7.Text = "Parametros JSon";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(252, 254);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(102, 17);
            this.label8.TabIndex = 21;
            this.label8.Text = "Simulacion Min";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(357, 254);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(105, 17);
            this.label9.TabIndex = 22;
            this.label9.Text = "Simulacion Max";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(479, 256);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 17);
            this.label10.TabIndex = 23;
            this.label10.Text = "Server Min";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(579, 256);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(79, 17);
            this.label11.TabIndex = 24;
            this.label11.Text = "Server Max";
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.Location = new System.Drawing.Point(706, 204);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(40, 40);
            this.btnDelete.TabIndex = 25;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(612, 352);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(119, 35);
            this.btnClose.TabIndex = 26;
            this.btnClose.Text = "Cerrar";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnModificar
            // 
            this.btnModificar.BackColor = System.Drawing.Color.Transparent;
            this.btnModificar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnModificar.FlatAppearance.BorderSize = 0;
            this.btnModificar.Location = new System.Drawing.Point(662, 204);
            this.btnModificar.Margin = new System.Windows.Forms.Padding(2);
            this.btnModificar.Name = "btnModificar";
            this.btnModificar.Size = new System.Drawing.Size(40, 40);
            this.btnModificar.TabIndex = 27;
            this.btnModificar.UseVisualStyleBackColor = false;
            this.btnModificar.Click += new System.EventHandler(this.btnModificar_Click);
            // 
            // JSonConfigurador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 405);
            this.Controls.Add(this.btnModificar);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lboxJSON);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.tbServerMax);
            this.Controls.Add(this.tbServerMin);
            this.Controls.Add(this.tbSimulatorMax);
            this.Controls.Add(this.tbSimulatorMin);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbServer);
            this.Controls.Add(this.cbWrite);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbRange);
            this.Controls.Add(this.tbMax);
            this.Controls.Add(this.tbMin);
            this.Controls.Add(this.tbParameter);
            this.Controls.Add(this.lboxParametros);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "JSonConfigurador";
            this.Text = "JSonConfigurador";
            this.Load += new System.EventHandler(this.JSonConfigurador_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lboxParametros;
        private System.Windows.Forms.TextBox tbParameter;
        private System.Windows.Forms.TextBox tbMin;
        private System.Windows.Forms.TextBox tbMax;
        private System.Windows.Forms.TextBox tbRange;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbWrite;
        private System.Windows.Forms.TextBox tbServer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.TextBox tbSimulatorMin;
        private System.Windows.Forms.TextBox tbSimulatorMax;
        private System.Windows.Forms.TextBox tbServerMax;
        private System.Windows.Forms.TextBox tbServerMin;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListBox lboxJSON;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnModificar;
    }
}