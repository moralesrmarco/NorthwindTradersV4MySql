﻿namespace NorthwindTradersV4MySql
{
    partial class FrmProductosCrud
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
            this.components = new System.ComponentModel.Container();
            this.tabcOperacion = new System.Windows.Forms.TabControl();
            this.tbpConsultar = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.tbpRegistrar = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.tbpModificar = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.tbpEliminar = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.grbProductos = new System.Windows.Forms.GroupBox();
            this.Dgv = new System.Windows.Forms.DataGridView();
            this.grbBuscar = new System.Windows.Forms.GroupBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.cboBProveedor = new System.Windows.Forms.ComboBox();
            this.cboBCategoria = new System.Windows.Forms.ComboBox();
            this.txtBProducto = new System.Windows.Forms.TextBox();
            this.txtBIdFin = new System.Windows.Forms.TextBox();
            this.txtBIdIni = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.grbProducto = new System.Windows.Forms.GroupBox();
            this.btnOperacion = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.chkbDescontinuado = new System.Windows.Forms.CheckBox();
            this.cboProveedor = new System.Windows.Forms.ComboBox();
            this.cboCategoria = new System.Windows.Forms.ComboBox();
            this.txtUPedido = new System.Windows.Forms.TextBox();
            this.txtPPedido = new System.Windows.Forms.TextBox();
            this.txtUInventario = new System.Windows.Forms.TextBox();
            this.txtPrecio = new System.Windows.Forms.TextBox();
            this.txtCantidadxU = new System.Windows.Forms.TextBox();
            this.txtProducto = new System.Windows.Forms.TextBox();
            this.txtId = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.tabcOperacion.SuspendLayout();
            this.tbpConsultar.SuspendLayout();
            this.tbpRegistrar.SuspendLayout();
            this.tbpModificar.SuspendLayout();
            this.tbpEliminar.SuspendLayout();
            this.grbProductos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Dgv)).BeginInit();
            this.grbBuscar.SuspendLayout();
            this.grbProducto.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabcOperacion
            // 
            this.tabcOperacion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabcOperacion.Controls.Add(this.tbpConsultar);
            this.tabcOperacion.Controls.Add(this.tbpRegistrar);
            this.tabcOperacion.Controls.Add(this.tbpModificar);
            this.tabcOperacion.Controls.Add(this.tbpEliminar);
            this.tabcOperacion.Location = new System.Drawing.Point(21, 10);
            this.tabcOperacion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabcOperacion.Name = "tabcOperacion";
            this.tabcOperacion.SelectedIndex = 0;
            this.tabcOperacion.Size = new System.Drawing.Size(1280, 69);
            this.tabcOperacion.TabIndex = 0;
            this.tabcOperacion.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabcOperacion_Selected);
            // 
            // tbpConsultar
            // 
            this.tbpConsultar.Controls.Add(this.label1);
            this.tbpConsultar.Location = new System.Drawing.Point(4, 25);
            this.tbpConsultar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbpConsultar.Name = "tbpConsultar";
            this.tbpConsultar.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbpConsultar.Size = new System.Drawing.Size(1272, 40);
            this.tbpConsultar.TabIndex = 0;
            this.tbpConsultar.Text = "   Consultar producto   ";
            this.tbpConsultar.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(481, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Busque el producto y seleccionelo en la lista que se muestra para ver su detalle";
            // 
            // tbpRegistrar
            // 
            this.tbpRegistrar.Controls.Add(this.label2);
            this.tbpRegistrar.Location = new System.Drawing.Point(4, 25);
            this.tbpRegistrar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbpRegistrar.Name = "tbpRegistrar";
            this.tbpRegistrar.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbpRegistrar.Size = new System.Drawing.Size(1272, 40);
            this.tbpRegistrar.TabIndex = 1;
            this.tbpRegistrar.Text = "   Registrar un producto   ";
            this.tbpRegistrar.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(283, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Proporcione los datos del producto a registrar.";
            // 
            // tbpModificar
            // 
            this.tbpModificar.Controls.Add(this.label3);
            this.tbpModificar.Location = new System.Drawing.Point(4, 25);
            this.tbpModificar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbpModificar.Name = "tbpModificar";
            this.tbpModificar.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbpModificar.Size = new System.Drawing.Size(1272, 40);
            this.tbpModificar.TabIndex = 2;
            this.tbpModificar.Text = "   Modificar un producto   ";
            this.tbpModificar.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 10);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(588, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Busque el producto y seleccionelo en la lista que se muestra, para que pueda modi" +
    "ficar sus datos";
            // 
            // tbpEliminar
            // 
            this.tbpEliminar.Controls.Add(this.label4);
            this.tbpEliminar.Location = new System.Drawing.Point(4, 25);
            this.tbpEliminar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbpEliminar.Name = "tbpEliminar";
            this.tbpEliminar.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbpEliminar.Size = new System.Drawing.Size(1272, 40);
            this.tbpEliminar.TabIndex = 3;
            this.tbpEliminar.Text = "   Eliminar un producto   ";
            this.tbpEliminar.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 10);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(865, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "Busque el producto a eliminar y seleccionelo en la lista que se muestra, no se pu" +
    "eden eliminar productos que ya estan relacionados a un pedido";
            // 
            // grbProductos
            // 
            this.grbProductos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbProductos.Controls.Add(this.Dgv);
            this.grbProductos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbProductos.Location = new System.Drawing.Point(21, 79);
            this.grbProductos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbProductos.Name = "grbProductos";
            this.grbProductos.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbProductos.Size = new System.Drawing.Size(1269, 295);
            this.grbProductos.TabIndex = 1;
            this.grbProductos.TabStop = false;
            this.grbProductos.Text = "»   Productos:   «";
            this.grbProductos.Paint += new System.Windows.Forms.PaintEventHandler(this.GrbPaint);
            // 
            // Dgv
            // 
            this.Dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Dgv.Location = new System.Drawing.Point(4, 20);
            this.Dgv.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Dgv.Name = "Dgv";
            this.Dgv.RowHeadersWidth = 51;
            this.Dgv.Size = new System.Drawing.Size(1261, 271);
            this.Dgv.TabIndex = 0;
            this.Dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Dgv_CellClick);
            // 
            // grbBuscar
            // 
            this.grbBuscar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grbBuscar.Controls.Add(this.btnBuscar);
            this.grbBuscar.Controls.Add(this.btnLimpiar);
            this.grbBuscar.Controls.Add(this.cboBProveedor);
            this.grbBuscar.Controls.Add(this.cboBCategoria);
            this.grbBuscar.Controls.Add(this.txtBProducto);
            this.grbBuscar.Controls.Add(this.txtBIdFin);
            this.grbBuscar.Controls.Add(this.txtBIdIni);
            this.grbBuscar.Controls.Add(this.label9);
            this.grbBuscar.Controls.Add(this.label8);
            this.grbBuscar.Controls.Add(this.label7);
            this.grbBuscar.Controls.Add(this.label6);
            this.grbBuscar.Controls.Add(this.label5);
            this.grbBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbBuscar.Location = new System.Drawing.Point(21, 384);
            this.grbBuscar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbBuscar.Name = "grbBuscar";
            this.grbBuscar.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbBuscar.Size = new System.Drawing.Size(277, 402);
            this.grbBuscar.TabIndex = 2;
            this.grbBuscar.TabStop = false;
            this.grbBuscar.Text = "»   Buscar un producto:   «";
            this.grbBuscar.Paint += new System.Windows.Forms.PaintEventHandler(this.GrbPaint);
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(139, 326);
            this.btnBuscar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(120, 28);
            this.btnBuscar.TabIndex = 6;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpiar.Location = new System.Drawing.Point(11, 325);
            this.btnLimpiar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(117, 28);
            this.btnLimpiar.TabIndex = 5;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // cboBProveedor
            // 
            this.cboBProveedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBProveedor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboBProveedor.FormattingEnabled = true;
            this.cboBProveedor.Location = new System.Drawing.Point(19, 266);
            this.cboBProveedor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboBProveedor.Name = "cboBProveedor";
            this.cboBProveedor.Size = new System.Drawing.Size(239, 25);
            this.cboBProveedor.TabIndex = 4;
            // 
            // cboBCategoria
            // 
            this.cboBCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBCategoria.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboBCategoria.FormattingEnabled = true;
            this.cboBCategoria.Location = new System.Drawing.Point(19, 204);
            this.cboBCategoria.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboBCategoria.Name = "cboBCategoria";
            this.cboBCategoria.Size = new System.Drawing.Size(239, 25);
            this.cboBCategoria.TabIndex = 3;
            // 
            // txtBProducto
            // 
            this.txtBProducto.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBProducto.Location = new System.Drawing.Point(19, 144);
            this.txtBProducto.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBProducto.MaxLength = 40;
            this.txtBProducto.Name = "txtBProducto";
            this.txtBProducto.Size = new System.Drawing.Size(239, 23);
            this.txtBProducto.TabIndex = 2;
            // 
            // txtBIdFin
            // 
            this.txtBIdFin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBIdFin.Location = new System.Drawing.Point(96, 79);
            this.txtBIdFin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBIdFin.MaxLength = 10;
            this.txtBIdFin.Name = "txtBIdFin";
            this.txtBIdFin.Size = new System.Drawing.Size(132, 23);
            this.txtBIdFin.TabIndex = 1;
            this.txtBIdFin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBIdFin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBIdFin_KeyPress);
            this.txtBIdFin.Leave += new System.EventHandler(this.txtBIdFin_Leave);
            // 
            // txtBIdIni
            // 
            this.txtBIdIni.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBIdIni.Location = new System.Drawing.Point(96, 39);
            this.txtBIdIni.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBIdIni.MaxLength = 10;
            this.txtBIdIni.Name = "txtBIdIni";
            this.txtBIdIni.Size = new System.Drawing.Size(132, 23);
            this.txtBIdIni.TabIndex = 0;
            this.txtBIdIni.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBIdIni.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBIdIni_KeyPress);
            this.txtBIdIni.Leave += new System.EventHandler(this.txtBIdIni_Leave);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 240);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 17);
            this.label9.TabIndex = 0;
            this.label9.Text = "Proveedor:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 178);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 17);
            this.label8.TabIndex = 0;
            this.label8.Text = "Categoría:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 118);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 17);
            this.label7.TabIndex = 0;
            this.label7.Text = "Producto:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 84);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 17);
            this.label6.TabIndex = 0;
            this.label6.Text = "Id final:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 44);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "Id inicial:";
            // 
            // grbProducto
            // 
            this.grbProducto.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbProducto.Controls.Add(this.btnOperacion);
            this.grbProducto.Controls.Add(this.label21);
            this.grbProducto.Controls.Add(this.label20);
            this.grbProducto.Controls.Add(this.chkbDescontinuado);
            this.grbProducto.Controls.Add(this.cboProveedor);
            this.grbProducto.Controls.Add(this.cboCategoria);
            this.grbProducto.Controls.Add(this.txtUPedido);
            this.grbProducto.Controls.Add(this.txtPPedido);
            this.grbProducto.Controls.Add(this.txtUInventario);
            this.grbProducto.Controls.Add(this.txtPrecio);
            this.grbProducto.Controls.Add(this.txtCantidadxU);
            this.grbProducto.Controls.Add(this.txtProducto);
            this.grbProducto.Controls.Add(this.txtId);
            this.grbProducto.Controls.Add(this.label19);
            this.grbProducto.Controls.Add(this.label18);
            this.grbProducto.Controls.Add(this.label17);
            this.grbProducto.Controls.Add(this.label16);
            this.grbProducto.Controls.Add(this.label15);
            this.grbProducto.Controls.Add(this.label14);
            this.grbProducto.Controls.Add(this.label13);
            this.grbProducto.Controls.Add(this.label12);
            this.grbProducto.Controls.Add(this.label11);
            this.grbProducto.Controls.Add(this.label10);
            this.grbProducto.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbProducto.Location = new System.Drawing.Point(331, 384);
            this.grbProducto.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbProducto.Name = "grbProducto";
            this.grbProducto.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbProducto.Size = new System.Drawing.Size(965, 402);
            this.grbProducto.TabIndex = 3;
            this.grbProducto.TabStop = false;
            this.grbProducto.Text = "»   Producto:   «";
            this.grbProducto.Paint += new System.Windows.Forms.PaintEventHandler(this.GrbPaint);
            // 
            // btnOperacion
            // 
            this.btnOperacion.AutoSize = true;
            this.btnOperacion.Location = new System.Drawing.Point(556, 335);
            this.btnOperacion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOperacion.Name = "btnOperacion";
            this.btnOperacion.Size = new System.Drawing.Size(247, 33);
            this.btnOperacion.TabIndex = 10;
            this.btnOperacion.Text = "Registrar producto";
            this.btnOperacion.UseVisualStyleBackColor = true;
            this.btnOperacion.Visible = false;
            this.btnOperacion.Click += new System.EventHandler(this.btnOperacion_Click);
            // 
            // label21
            // 
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(565, 276);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(228, 38);
            this.label21.TabIndex = 5;
            this.label21.Text = "Cantidad de unidades en el inventario en la cual se debe realizar un nuevo pedido" +
    "";
            // 
            // label20
            // 
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(565, 242);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(228, 37);
            this.label20.TabIndex = 4;
            this.label20.Text = "Cantidad mínima que se debe solicitar en un nuevo pedido";
            // 
            // chkbDescontinuado
            // 
            this.chkbDescontinuado.AutoSize = true;
            this.chkbDescontinuado.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkbDescontinuado.Location = new System.Drawing.Point(408, 321);
            this.chkbDescontinuado.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkbDescontinuado.Name = "chkbDescontinuado";
            this.chkbDescontinuado.Size = new System.Drawing.Size(18, 17);
            this.chkbDescontinuado.TabIndex = 9;
            this.chkbDescontinuado.UseVisualStyleBackColor = true;
            // 
            // cboProveedor
            // 
            this.cboProveedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProveedor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboProveedor.FormattingEnabled = true;
            this.cboProveedor.Location = new System.Drawing.Point(408, 82);
            this.cboProveedor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboProveedor.Name = "cboProveedor";
            this.cboProveedor.Size = new System.Drawing.Size(357, 25);
            this.cboProveedor.TabIndex = 2;
            // 
            // cboCategoria
            // 
            this.cboCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCategoria.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCategoria.FormattingEnabled = true;
            this.cboCategoria.Location = new System.Drawing.Point(408, 49);
            this.cboCategoria.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboCategoria.Name = "cboCategoria";
            this.cboCategoria.Size = new System.Drawing.Size(357, 25);
            this.cboCategoria.TabIndex = 1;
            // 
            // txtUPedido
            // 
            this.txtUPedido.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUPedido.Location = new System.Drawing.Point(408, 249);
            this.txtUPedido.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtUPedido.MaxLength = 5;
            this.txtUPedido.Name = "txtUPedido";
            this.txtUPedido.Size = new System.Drawing.Size(132, 23);
            this.txtUPedido.TabIndex = 7;
            this.txtUPedido.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUPedido_KeyPress);
            this.txtUPedido.Validating += new System.ComponentModel.CancelEventHandler(this.txtUPedido_Validating);
            // 
            // txtPPedido
            // 
            this.txtPPedido.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPPedido.Location = new System.Drawing.Point(408, 282);
            this.txtPPedido.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPPedido.MaxLength = 5;
            this.txtPPedido.Name = "txtPPedido";
            this.txtPPedido.Size = new System.Drawing.Size(132, 23);
            this.txtPPedido.TabIndex = 8;
            this.txtPPedido.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPPedido_KeyPress);
            this.txtPPedido.Validating += new System.ComponentModel.CancelEventHandler(this.txtPPedido_Validating);
            // 
            // txtUInventario
            // 
            this.txtUInventario.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUInventario.Location = new System.Drawing.Point(408, 215);
            this.txtUInventario.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtUInventario.MaxLength = 5;
            this.txtUInventario.Name = "txtUInventario";
            this.txtUInventario.Size = new System.Drawing.Size(132, 23);
            this.txtUInventario.TabIndex = 6;
            this.txtUInventario.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUInventario_KeyPress);
            this.txtUInventario.Validating += new System.ComponentModel.CancelEventHandler(this.txtUInventario_Validating);
            // 
            // txtPrecio
            // 
            this.txtPrecio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrecio.Location = new System.Drawing.Point(408, 182);
            this.txtPrecio.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPrecio.MaxLength = 21;
            this.txtPrecio.Name = "txtPrecio";
            this.txtPrecio.Size = new System.Drawing.Size(196, 23);
            this.txtPrecio.TabIndex = 5;
            this.txtPrecio.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPrecio_KeyPress);
            // 
            // txtCantidadxU
            // 
            this.txtCantidadxU.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCantidadxU.Location = new System.Drawing.Point(408, 149);
            this.txtCantidadxU.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCantidadxU.MaxLength = 20;
            this.txtCantidadxU.Name = "txtCantidadxU";
            this.txtCantidadxU.Size = new System.Drawing.Size(276, 23);
            this.txtCantidadxU.TabIndex = 4;
            // 
            // txtProducto
            // 
            this.txtProducto.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProducto.Location = new System.Drawing.Point(408, 116);
            this.txtProducto.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtProducto.MaxLength = 40;
            this.txtProducto.Name = "txtProducto";
            this.txtProducto.Size = new System.Drawing.Size(357, 23);
            this.txtProducto.TabIndex = 3;
            // 
            // txtId
            // 
            this.txtId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtId.Location = new System.Drawing.Point(408, 16);
            this.txtId.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtId.MaxLength = 10;
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(132, 23);
            this.txtId.TabIndex = 0;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(268, 320);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(121, 17);
            this.label19.TabIndex = 0;
            this.label19.Text = "Descontinuado:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(257, 287);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(168, 21);
            this.label18.TabIndex = 0;
            this.label18.Text = "Nivel de reorden:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(231, 254);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(158, 17);
            this.label17.TabIndex = 0;
            this.label17.Text = "Unidades en pedido:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(207, 220);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(181, 17);
            this.label16.TabIndex = 0;
            this.label16.Text = "Unidades en inventario:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(333, 187);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(59, 17);
            this.label15.TabIndex = 0;
            this.label15.Text = "Precio:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(229, 154);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(160, 17);
            this.label14.TabIndex = 0;
            this.label14.Text = "Cantidad por unidad:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(313, 121);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(78, 17);
            this.label13.TabIndex = 0;
            this.label13.Text = "Producto:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(304, 87);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 17);
            this.label12.TabIndex = 0;
            this.label12.Text = "Proveedor:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(307, 54);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(83, 17);
            this.label11.TabIndex = 0;
            this.label11.Text = "Categoría:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(367, 21);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(26, 17);
            this.label10.TabIndex = 0;
            this.label10.Text = "Id:";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // FrmProductosCrud
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1312, 793);
            this.ControlBox = false;
            this.Controls.Add(this.grbProducto);
            this.Controls.Add(this.grbBuscar);
            this.Controls.Add(this.grbProductos);
            this.Controls.Add(this.tabcOperacion);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmProductosCrud";
            this.Text = "» Mantenimiento de productos «";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmProductosCrud_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmProductosCrud_FormClosed);
            this.Load += new System.EventHandler(this.FrmProductosCrud_Load);
            this.tabcOperacion.ResumeLayout(false);
            this.tbpConsultar.ResumeLayout(false);
            this.tbpConsultar.PerformLayout();
            this.tbpRegistrar.ResumeLayout(false);
            this.tbpRegistrar.PerformLayout();
            this.tbpModificar.ResumeLayout(false);
            this.tbpModificar.PerformLayout();
            this.tbpEliminar.ResumeLayout(false);
            this.tbpEliminar.PerformLayout();
            this.grbProductos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Dgv)).EndInit();
            this.grbBuscar.ResumeLayout(false);
            this.grbBuscar.PerformLayout();
            this.grbProducto.ResumeLayout(false);
            this.grbProducto.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabcOperacion;
        private System.Windows.Forms.TabPage tbpConsultar;
        private System.Windows.Forms.TabPage tbpRegistrar;
        private System.Windows.Forms.TabPage tbpModificar;
        private System.Windows.Forms.TabPage tbpEliminar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox grbProductos;
        private System.Windows.Forms.DataGridView Dgv;
        private System.Windows.Forms.GroupBox grbBuscar;
        private System.Windows.Forms.GroupBox grbProducto;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBIdFin;
        private System.Windows.Forms.TextBox txtBIdIni;
        private System.Windows.Forms.ComboBox cboBProveedor;
        private System.Windows.Forms.ComboBox cboBCategoria;
        private System.Windows.Forms.TextBox txtBProducto;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkbDescontinuado;
        private System.Windows.Forms.ComboBox cboProveedor;
        private System.Windows.Forms.ComboBox cboCategoria;
        private System.Windows.Forms.TextBox txtPPedido;
        private System.Windows.Forms.TextBox txtUInventario;
        private System.Windows.Forms.TextBox txtPrecio;
        private System.Windows.Forms.TextBox txtCantidadxU;
        private System.Windows.Forms.TextBox txtProducto;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.TextBox txtUPedido;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button btnOperacion;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}