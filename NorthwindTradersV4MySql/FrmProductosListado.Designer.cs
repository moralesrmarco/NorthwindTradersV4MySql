namespace NorthwindTradersV4MySql
{
    partial class FrmProductosListado
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
            this.tabpListarTodos = new System.Windows.Forms.TabPage();
            this.btnListarTodos = new System.Windows.Forms.Button();
            this.tabpBuscarProducto = new System.Windows.Forms.TabPage();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnBuscar = new System.Windows.Forms.Button();
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
            this.Grb = new System.Windows.Forms.GroupBox();
            this.Dgv = new System.Windows.Forms.DataGridView();
            this.tabcOperacion.SuspendLayout();
            this.tabpListarTodos.SuspendLayout();
            this.tabpBuscarProducto.SuspendLayout();
            this.Grb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // tabcOperacion
            // 
            this.tabcOperacion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabcOperacion.Controls.Add(this.tabpListarTodos);
            this.tabcOperacion.Controls.Add(this.tabpBuscarProducto);
            this.tabcOperacion.Location = new System.Drawing.Point(21, 20);
            this.tabcOperacion.Margin = new System.Windows.Forms.Padding(4);
            this.tabcOperacion.Name = "tabcOperacion";
            this.tabcOperacion.SelectedIndex = 0;
            this.tabcOperacion.Size = new System.Drawing.Size(1373, 73);
            this.tabcOperacion.TabIndex = 0;
            this.tabcOperacion.SelectedIndexChanged += new System.EventHandler(this.tabcOperacion_SelectedIndexChanged);
            this.tabcOperacion.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabcOperacion_Selected);
            // 
            // tabpListarTodos
            // 
            this.tabpListarTodos.Controls.Add(this.btnListarTodos);
            this.tabpListarTodos.Location = new System.Drawing.Point(4, 25);
            this.tabpListarTodos.Margin = new System.Windows.Forms.Padding(4);
            this.tabpListarTodos.Name = "tabpListarTodos";
            this.tabpListarTodos.Padding = new System.Windows.Forms.Padding(4);
            this.tabpListarTodos.Size = new System.Drawing.Size(1365, 44);
            this.tabpListarTodos.TabIndex = 0;
            this.tabpListarTodos.Text = "   Listar todos los productos   ";
            this.tabpListarTodos.UseVisualStyleBackColor = true;
            // 
            // btnListarTodos
            // 
            this.btnListarTodos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnListarTodos.Location = new System.Drawing.Point(21, 6);
            this.btnListarTodos.Margin = new System.Windows.Forms.Padding(4);
            this.btnListarTodos.Name = "btnListarTodos";
            this.btnListarTodos.Size = new System.Drawing.Size(267, 28);
            this.btnListarTodos.TabIndex = 0;
            this.btnListarTodos.Tag = "Listar";
            this.btnListarTodos.Text = "   Listar todos los productos   ";
            this.btnListarTodos.UseVisualStyleBackColor = true;
            this.btnListarTodos.Click += new System.EventHandler(this.btnListarTodos_Click);
            // 
            // tabpBuscarProducto
            // 
            this.tabpBuscarProducto.Controls.Add(this.btnLimpiar);
            this.tabpBuscarProducto.Controls.Add(this.btnBuscar);
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
            this.tabpBuscarProducto.Margin = new System.Windows.Forms.Padding(4);
            this.tabpBuscarProducto.Name = "tabpBuscarProducto";
            this.tabpBuscarProducto.Padding = new System.Windows.Forms.Padding(4);
            this.tabpBuscarProducto.Size = new System.Drawing.Size(1365, 44);
            this.tabpBuscarProducto.TabIndex = 1;
            this.tabpBuscarProducto.Text = "   Buscar un producto   ";
            this.tabpBuscarProducto.UseVisualStyleBackColor = true;
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(1255, 6);
            this.btnLimpiar.Margin = new System.Windows.Forms.Padding(4);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(100, 28);
            this.btnLimpiar.TabIndex = 12;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // btnBuscar
            // 
            this.btnBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuscar.Location = new System.Drawing.Point(1147, 6);
            this.btnBuscar.Margin = new System.Windows.Forms.Padding(4);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(100, 28);
            this.btnBuscar.TabIndex = 11;
            this.btnBuscar.Tag = "Buscar";
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // cboProveedor
            // 
            this.cboProveedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProveedor.FormattingEnabled = true;
            this.cboProveedor.Location = new System.Drawing.Point(979, 7);
            this.cboProveedor.Margin = new System.Windows.Forms.Padding(4);
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
            this.cboCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCategoria.FormattingEnabled = true;
            this.cboCategoria.Location = new System.Drawing.Point(735, 7);
            this.cboCategoria.Margin = new System.Windows.Forms.Padding(4);
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
            this.txtProducto.Margin = new System.Windows.Forms.Padding(4);
            this.txtProducto.MaxLength = 40;
            this.txtProducto.Name = "txtProducto";
            this.txtProducto.Size = new System.Drawing.Size(132, 22);
            this.txtProducto.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(465, 12);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 20);
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
            this.txtIdFinal.Margin = new System.Windows.Forms.Padding(4);
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
            this.txtIdInicial.Margin = new System.Windows.Forms.Padding(4);
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
            // Grb
            // 
            this.Grb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Grb.Controls.Add(this.Dgv);
            this.Grb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Grb.Location = new System.Drawing.Point(21, 98);
            this.Grb.Margin = new System.Windows.Forms.Padding(4);
            this.Grb.Name = "Grb";
            this.Grb.Padding = new System.Windows.Forms.Padding(30);
            this.Grb.Size = new System.Drawing.Size(1373, 657);
            this.Grb.TabIndex = 1;
            this.Grb.TabStop = false;
            this.Grb.Text = "»   Listado de productos   «";
            this.Grb.Paint += new System.Windows.Forms.PaintEventHandler(this.GrbPaint);
            // 
            // Dgv
            // 
            this.Dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Dgv.Location = new System.Drawing.Point(30, 46);
            this.Dgv.Margin = new System.Windows.Forms.Padding(4);
            this.Dgv.Name = "Dgv";
            this.Dgv.RowHeadersWidth = 51;
            this.Dgv.Size = new System.Drawing.Size(1313, 581);
            this.Dgv.TabIndex = 0;
            // 
            // FrmProductosListado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1405, 764);
            this.ControlBox = false;
            this.Controls.Add(this.Grb);
            this.Controls.Add(this.tabcOperacion);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmProductosListado";
            this.Text = "» Listado de productos «";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmProductosListado_FormClosed);
            this.Load += new System.EventHandler(this.FrmProductosListado_Load);
            this.tabcOperacion.ResumeLayout(false);
            this.tabpListarTodos.ResumeLayout(false);
            this.tabpBuscarProducto.ResumeLayout(false);
            this.tabpBuscarProducto.PerformLayout();
            this.Grb.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabcOperacion;
        private System.Windows.Forms.TabPage tabpListarTodos;
        private System.Windows.Forms.TabPage tabpBuscarProducto;
        private System.Windows.Forms.Button btnListarTodos;
        private System.Windows.Forms.GroupBox Grb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView Dgv;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtIdFinal;
        private System.Windows.Forms.TextBox txtIdInicial;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtProducto;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.ComboBox cboProveedor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboCategoria;
        private System.Windows.Forms.Button btnLimpiar;
    }
}