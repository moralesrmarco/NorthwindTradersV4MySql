namespace NorthwindTradersV4MySql
{
    partial class FrmRptProductos
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
            this.tabcOperacion = new System.Windows.Forms.TabControl();
            this.tabpImprimirTodos = new System.Windows.Forms.TabPage();
            this.btnImprimirTodos = new System.Windows.Forms.Button();
            this.tabpBuscarProducto = new System.Windows.Forms.TabPage();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.cboProveedor = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cboCategoria = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtProducto = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtIdFinal = new System.Windows.Forms.TextBox();
            this.txtIdInicial = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.label7 = new System.Windows.Forms.Label();
            this.Cbo2OrdenadoPor = new System.Windows.Forms.ComboBox();
            this.Cbo2AscDesc = new System.Windows.Forms.ComboBox();
            this.Cbo1AscDesc = new System.Windows.Forms.ComboBox();
            this.Cbo1OrdenadoPor = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabcOperacion.SuspendLayout();
            this.tabpImprimirTodos.SuspendLayout();
            this.tabpBuscarProducto.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabcOperacion
            // 
            this.tabcOperacion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabcOperacion.Controls.Add(this.tabpImprimirTodos);
            this.tabcOperacion.Controls.Add(this.tabpBuscarProducto);
            this.tabcOperacion.Location = new System.Drawing.Point(21, 20);
            this.tabcOperacion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabcOperacion.Name = "tabcOperacion";
            this.tabcOperacion.SelectedIndex = 0;
            this.tabcOperacion.Size = new System.Drawing.Size(1750, 73);
            this.tabcOperacion.TabIndex = 1;
            this.tabcOperacion.SelectedIndexChanged += new System.EventHandler(this.tabcOperacion_SelectedIndexChanged);
            this.tabcOperacion.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabcOperacion_Selected);
            // 
            // tabpImprimirTodos
            // 
            this.tabpImprimirTodos.Controls.Add(this.Cbo1AscDesc);
            this.tabpImprimirTodos.Controls.Add(this.Cbo1OrdenadoPor);
            this.tabpImprimirTodos.Controls.Add(this.label8);
            this.tabpImprimirTodos.Controls.Add(this.btnImprimirTodos);
            this.tabpImprimirTodos.Location = new System.Drawing.Point(4, 25);
            this.tabpImprimirTodos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabpImprimirTodos.Name = "tabpImprimirTodos";
            this.tabpImprimirTodos.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabpImprimirTodos.Size = new System.Drawing.Size(1742, 44);
            this.tabpImprimirTodos.TabIndex = 0;
            this.tabpImprimirTodos.Text = "   Imprimir todos los productos   ";
            this.tabpImprimirTodos.UseVisualStyleBackColor = true;
            // 
            // btnImprimirTodos
            // 
            this.btnImprimirTodos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImprimirTodos.Location = new System.Drawing.Point(438, 8);
            this.btnImprimirTodos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnImprimirTodos.Name = "btnImprimirTodos";
            this.btnImprimirTodos.Size = new System.Drawing.Size(267, 28);
            this.btnImprimirTodos.TabIndex = 0;
            this.btnImprimirTodos.Tag = "ImprimirTodos";
            this.btnImprimirTodos.Text = "   Imprimir todos los productos   ";
            this.btnImprimirTodos.UseVisualStyleBackColor = true;
            this.btnImprimirTodos.Click += new System.EventHandler(this.btnImprimirTodos_Click);
            // 
            // tabpBuscarProducto
            // 
            this.tabpBuscarProducto.Controls.Add(this.Cbo2AscDesc);
            this.tabpBuscarProducto.Controls.Add(this.Cbo2OrdenadoPor);
            this.tabpBuscarProducto.Controls.Add(this.label7);
            this.tabpBuscarProducto.Controls.Add(this.btnLimpiar);
            this.tabpBuscarProducto.Controls.Add(this.btnImprimir);
            this.tabpBuscarProducto.Controls.Add(this.cboProveedor);
            this.tabpBuscarProducto.Controls.Add(this.label6);
            this.tabpBuscarProducto.Controls.Add(this.cboCategoria);
            this.tabpBuscarProducto.Controls.Add(this.label5);
            this.tabpBuscarProducto.Controls.Add(this.txtProducto);
            this.tabpBuscarProducto.Controls.Add(this.label4);
            this.tabpBuscarProducto.Controls.Add(this.label3);
            this.tabpBuscarProducto.Controls.Add(this.txtIdFinal);
            this.tabpBuscarProducto.Controls.Add(this.txtIdInicial);
            this.tabpBuscarProducto.Controls.Add(this.label2);
            this.tabpBuscarProducto.Controls.Add(this.label1);
            this.tabpBuscarProducto.Location = new System.Drawing.Point(4, 25);
            this.tabpBuscarProducto.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabpBuscarProducto.Name = "tabpBuscarProducto";
            this.tabpBuscarProducto.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabpBuscarProducto.Size = new System.Drawing.Size(1742, 44);
            this.tabpBuscarProducto.TabIndex = 1;
            this.tabpBuscarProducto.Text = "   Buscar un producto   ";
            this.tabpBuscarProducto.UseVisualStyleBackColor = true;
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(1634, 6);
            this.btnLimpiar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(100, 28);
            this.btnLimpiar.TabIndex = 12;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // btnImprimir
            // 
            this.btnImprimir.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImprimir.Location = new System.Drawing.Point(1526, 6);
            this.btnImprimir.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(100, 28);
            this.btnImprimir.TabIndex = 11;
            this.btnImprimir.Tag = "Imprimir";
            this.btnImprimir.Text = "Imprimir";
            this.btnImprimir.UseVisualStyleBackColor = true;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // cboProveedor
            // 
            this.cboProveedor.FormattingEnabled = true;
            this.cboProveedor.Location = new System.Drawing.Point(979, 7);
            this.cboProveedor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboProveedor.Name = "cboProveedor";
            this.cboProveedor.Size = new System.Drawing.Size(160, 24);
            this.cboProveedor.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(901, 12);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 16);
            this.label6.TabIndex = 9;
            this.label6.Text = "Proveedor:";
            // 
            // cboCategoria
            // 
            this.cboCategoria.FormattingEnabled = true;
            this.cboCategoria.Location = new System.Drawing.Point(735, 7);
            this.cboCategoria.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboCategoria.Name = "cboCategoria";
            this.cboCategoria.Size = new System.Drawing.Size(160, 24);
            this.cboCategoria.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(660, 12);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 16);
            this.label5.TabIndex = 7;
            this.label5.Text = "Categoría:";
            // 
            // txtProducto
            // 
            this.txtProducto.Location = new System.Drawing.Point(527, 7);
            this.txtProducto.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtProducto.MaxLength = 40;
            this.txtProducto.Name = "txtProducto";
            this.txtProducto.Size = new System.Drawing.Size(132, 22);
            this.txtProducto.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(460, 12);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "Producto:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(317, 12);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Id final:";
            // 
            // txtIdFinal
            // 
            this.txtIdFinal.Location = new System.Drawing.Point(373, 7);
            this.txtIdFinal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtIdFinal.MaxLength = 10;
            this.txtIdFinal.Name = "txtIdFinal";
            this.txtIdFinal.Size = new System.Drawing.Size(87, 22);
            this.txtIdFinal.TabIndex = 3;
            this.txtIdFinal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtIdFinal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtIdFinal_KeyPress);
            this.txtIdFinal.Leave += new System.EventHandler(this.txtIdFinal_Leave);
            // 
            // txtIdInicial
            // 
            this.txtIdInicial.Location = new System.Drawing.Point(224, 7);
            this.txtIdInicial.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtIdInicial.MaxLength = 10;
            this.txtIdInicial.Name = "txtIdInicial";
            this.txtIdInicial.Size = new System.Drawing.Size(87, 22);
            this.txtIdInicial.TabIndex = 2;
            this.txtIdInicial.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtIdInicial.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtIdInicial_KeyPress);
            this.txtIdInicial.Leave += new System.EventHandler(this.txtIdInicial_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(160, 12);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Id inicial:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Buscar un producto:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.reportViewer1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(21, 111);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(20, 20, 20, 20);
            this.groupBox1.Size = new System.Drawing.Size(1736, 630);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "» Reporte de productos «";
            this.groupBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.GrbPaint);
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "NorthwindTradersV4MySql.RptProductos.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(20, 39);
            this.reportViewer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(1696, 571);
            this.reportViewer1.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1149, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 16);
            this.label7.TabIndex = 13;
            this.label7.Text = "Ordenado por:";
            // 
            // Cbo2OrdenadoPor
            // 
            this.Cbo2OrdenadoPor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cbo2OrdenadoPor.FormattingEnabled = true;
            this.Cbo2OrdenadoPor.Location = new System.Drawing.Point(1245, 7);
            this.Cbo2OrdenadoPor.Name = "Cbo2OrdenadoPor";
            this.Cbo2OrdenadoPor.Size = new System.Drawing.Size(150, 24);
            this.Cbo2OrdenadoPor.TabIndex = 14;
            // 
            // Cbo2AscDesc
            // 
            this.Cbo2AscDesc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cbo2AscDesc.FormattingEnabled = true;
            this.Cbo2AscDesc.Location = new System.Drawing.Point(1401, 7);
            this.Cbo2AscDesc.Name = "Cbo2AscDesc";
            this.Cbo2AscDesc.Size = new System.Drawing.Size(122, 24);
            this.Cbo2AscDesc.TabIndex = 15;
            // 
            // Cbo1AscDesc
            // 
            this.Cbo1AscDesc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cbo1AscDesc.FormattingEnabled = true;
            this.Cbo1AscDesc.Location = new System.Drawing.Point(287, 12);
            this.Cbo1AscDesc.Name = "Cbo1AscDesc";
            this.Cbo1AscDesc.Size = new System.Drawing.Size(122, 24);
            this.Cbo1AscDesc.TabIndex = 18;
            // 
            // Cbo1OrdenadoPor
            // 
            this.Cbo1OrdenadoPor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cbo1OrdenadoPor.FormattingEnabled = true;
            this.Cbo1OrdenadoPor.Location = new System.Drawing.Point(131, 12);
            this.Cbo1OrdenadoPor.Name = "Cbo1OrdenadoPor";
            this.Cbo1OrdenadoPor.Size = new System.Drawing.Size(150, 24);
            this.Cbo1OrdenadoPor.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(35, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 16);
            this.label8.TabIndex = 16;
            this.label8.Text = "Ordenado por:";
            // 
            // FrmRptProductos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1782, 764);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabcOperacion);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmRptProductos";
            this.Padding = new System.Windows.Forms.Padding(20, 20, 20, 20);
            this.Text = "» Reporte de productos «";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmRptProductos_FormClosed);
            this.Load += new System.EventHandler(this.FrmRptProductos_Load);
            this.tabcOperacion.ResumeLayout(false);
            this.tabpImprimirTodos.ResumeLayout(false);
            this.tabpImprimirTodos.PerformLayout();
            this.tabpBuscarProducto.ResumeLayout(false);
            this.tabpBuscarProducto.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabcOperacion;
        private System.Windows.Forms.TabPage tabpImprimirTodos;
        private System.Windows.Forms.Button btnImprimirTodos;
        private System.Windows.Forms.TabPage tabpBuscarProducto;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.ComboBox cboProveedor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboCategoria;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtProducto;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtIdFinal;
        private System.Windows.Forms.TextBox txtIdInicial;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.GroupBox groupBox1;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.ComboBox Cbo2AscDesc;
        private System.Windows.Forms.ComboBox Cbo2OrdenadoPor;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox Cbo1AscDesc;
        private System.Windows.Forms.ComboBox Cbo1OrdenadoPor;
        private System.Windows.Forms.Label label8;
    }
}