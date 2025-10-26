namespace NorthwindTradersV4MySql
{
    partial class FrmPedidosDetalleCrud
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.GrbDetalle = new System.Windows.Forms.GroupBox();
            this.DgvDetalle = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Producto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Precio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Descuento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Importe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Modificar = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Eliminar = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ProductoId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RowVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GrbAgregarProducto = new System.Windows.Forms.GroupBox();
            this.txtUInventario = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.txtDescuento = new System.Windows.Forms.TextBox();
            this.txtCantidad = new System.Windows.Forms.TextBox();
            this.txtPrecio = new System.Windows.Forms.TextBox();
            this.label38 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.cboProducto = new System.Windows.Forms.ComboBox();
            this.cboCategoria = new System.Windows.Forms.ComboBox();
            this.label37 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.GrbPedido = new System.Windows.Forms.GroupBox();
            this.BtnNota = new System.Windows.Forms.Button();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.txtCliente = new System.Windows.Forms.TextBox();
            this.txtId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.GrbBuscar = new System.Windows.Forms.GroupBox();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.txtBDirigidoa = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtBCompañiaT = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtBEmpleado = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.dtpBFEnvioFin = new System.Windows.Forms.DateTimePicker();
            this.dtpBFEnvioIni = new System.Windows.Forms.DateTimePicker();
            this.chkBFEnvioNull = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.dtpBFRequeridoFin = new System.Windows.Forms.DateTimePicker();
            this.dtpBFRequeridoIni = new System.Windows.Forms.DateTimePicker();
            this.chkBFRequeridoNull = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.chkBFPedidoNull = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.dtpBFPedidoFin = new System.Windows.Forms.DateTimePicker();
            this.dtpBFPedidoIni = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.txtBCliente = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtBIdFinal = new System.Windows.Forms.TextBox();
            this.txtBIdInicial = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.GrbPedidos = new System.Windows.Forms.GroupBox();
            this.DgvPedidos = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel1.SuspendLayout();
            this.GrbDetalle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvDetalle)).BeginInit();
            this.GrbAgregarProducto.SuspendLayout();
            this.GrbPedido.SuspendLayout();
            this.GrbBuscar.SuspendLayout();
            this.GrbPedidos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvPedidos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.GrbDetalle);
            this.panel1.Controls.Add(this.GrbAgregarProducto);
            this.panel1.Controls.Add(this.GrbPedido);
            this.panel1.Controls.Add(this.GrbBuscar);
            this.panel1.Controls.Add(this.GrbPedidos);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1579, 937);
            this.panel1.TabIndex = 0;
            // 
            // GrbDetalle
            // 
            this.GrbDetalle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GrbDetalle.Controls.Add(this.DgvDetalle);
            this.GrbDetalle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrbDetalle.Location = new System.Drawing.Point(459, 620);
            this.GrbDetalle.Margin = new System.Windows.Forms.Padding(4);
            this.GrbDetalle.Name = "GrbDetalle";
            this.GrbDetalle.Padding = new System.Windows.Forms.Padding(4);
            this.GrbDetalle.Size = new System.Drawing.Size(1099, 305);
            this.GrbDetalle.TabIndex = 6;
            this.GrbDetalle.TabStop = false;
            this.GrbDetalle.Text = "»   Detalle del pedido:   «";
            this.GrbDetalle.Paint += new System.Windows.Forms.PaintEventHandler(this.GrbPaint);
            // 
            // DgvDetalle
            // 
            this.DgvDetalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvDetalle.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.Producto,
            this.Precio,
            this.Cantidad,
            this.Descuento,
            this.Importe,
            this.Modificar,
            this.Eliminar,
            this.ProductoId,
            this.RowVersion});
            this.DgvDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvDetalle.Location = new System.Drawing.Point(4, 20);
            this.DgvDetalle.Margin = new System.Windows.Forms.Padding(4);
            this.DgvDetalle.Name = "DgvDetalle";
            this.DgvDetalle.RowHeadersWidth = 51;
            this.DgvDetalle.Size = new System.Drawing.Size(1091, 281);
            this.DgvDetalle.TabIndex = 0;
            this.DgvDetalle.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvDetalle_CellClick);
            // 
            // Id
            // 
            this.Id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Id.HeaderText = "Id";
            this.Id.MinimumWidth = 6;
            this.Id.Name = "Id";
            this.Id.Width = 50;
            // 
            // Producto
            // 
            this.Producto.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Producto.HeaderText = "Producto";
            this.Producto.MinimumWidth = 6;
            this.Producto.Name = "Producto";
            // 
            // Precio
            // 
            this.Precio.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Precio.HeaderText = "Precio";
            this.Precio.MinimumWidth = 6;
            this.Precio.Name = "Precio";
            this.Precio.Width = 83;
            // 
            // Cantidad
            // 
            this.Cantidad.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Cantidad.HeaderText = "Cantidad";
            this.Cantidad.MinimumWidth = 6;
            this.Cantidad.Name = "Cantidad";
            this.Cantidad.Width = 101;
            // 
            // Descuento
            // 
            this.Descuento.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Descuento.HeaderText = "Descuento";
            this.Descuento.MinimumWidth = 6;
            this.Descuento.Name = "Descuento";
            this.Descuento.Width = 114;
            // 
            // Importe
            // 
            this.Importe.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Importe.HeaderText = "Importe";
            this.Importe.MinimumWidth = 6;
            this.Importe.Name = "Importe";
            this.Importe.Width = 91;
            // 
            // Modificar
            // 
            this.Modificar.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Modificar.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Modificar.HeaderText = "Modificar producto";
            this.Modificar.MinimumWidth = 6;
            this.Modificar.Name = "Modificar";
            this.Modificar.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Modificar.Width = 134;
            // 
            // Eliminar
            // 
            this.Eliminar.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Eliminar.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Eliminar.HeaderText = "Eliminar producto";
            this.Eliminar.MinimumWidth = 6;
            this.Eliminar.Name = "Eliminar";
            this.Eliminar.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Eliminar.Width = 127;
            // 
            // ProductoId
            // 
            this.ProductoId.HeaderText = "ProductoId";
            this.ProductoId.MinimumWidth = 6;
            this.ProductoId.Name = "ProductoId";
            this.ProductoId.Visible = false;
            this.ProductoId.Width = 125;
            // 
            // RowVersion
            // 
            this.RowVersion.HeaderText = "RowVersion";
            this.RowVersion.MinimumWidth = 6;
            this.RowVersion.Name = "RowVersion";
            this.RowVersion.Visible = false;
            this.RowVersion.Width = 125;
            // 
            // GrbAgregarProducto
            // 
            this.GrbAgregarProducto.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GrbAgregarProducto.Controls.Add(this.txtUInventario);
            this.GrbAgregarProducto.Controls.Add(this.label4);
            this.GrbAgregarProducto.Controls.Add(this.btnAgregar);
            this.GrbAgregarProducto.Controls.Add(this.txtDescuento);
            this.GrbAgregarProducto.Controls.Add(this.txtCantidad);
            this.GrbAgregarProducto.Controls.Add(this.txtPrecio);
            this.GrbAgregarProducto.Controls.Add(this.label38);
            this.GrbAgregarProducto.Controls.Add(this.label39);
            this.GrbAgregarProducto.Controls.Add(this.label40);
            this.GrbAgregarProducto.Controls.Add(this.cboProducto);
            this.GrbAgregarProducto.Controls.Add(this.cboCategoria);
            this.GrbAgregarProducto.Controls.Add(this.label37);
            this.GrbAgregarProducto.Controls.Add(this.label36);
            this.GrbAgregarProducto.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrbAgregarProducto.Location = new System.Drawing.Point(459, 512);
            this.GrbAgregarProducto.Margin = new System.Windows.Forms.Padding(4);
            this.GrbAgregarProducto.Name = "GrbAgregarProducto";
            this.GrbAgregarProducto.Padding = new System.Windows.Forms.Padding(4);
            this.GrbAgregarProducto.Size = new System.Drawing.Size(1099, 98);
            this.GrbAgregarProducto.TabIndex = 5;
            this.GrbAgregarProducto.TabStop = false;
            this.GrbAgregarProducto.Text = "»   Agregar producto:   «";
            this.GrbAgregarProducto.Paint += new System.Windows.Forms.PaintEventHandler(this.GrbPaint);
            // 
            // txtUInventario
            // 
            this.txtUInventario.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUInventario.Location = new System.Drawing.Point(459, 59);
            this.txtUInventario.Margin = new System.Windows.Forms.Padding(4);
            this.txtUInventario.Name = "txtUInventario";
            this.txtUInventario.ReadOnly = true;
            this.txtUInventario.Size = new System.Drawing.Size(65, 23);
            this.txtUInventario.TabIndex = 6;
            this.txtUInventario.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(267, 64);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(181, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Unidades en inventario:";
            // 
            // btnAgregar
            // 
            this.btnAgregar.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregar.Location = new System.Drawing.Point(1003, 53);
            this.btnAgregar.Margin = new System.Windows.Forms.Padding(4);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(40, 37);
            this.btnAgregar.TabIndex = 4;
            this.btnAgregar.Text = "+";
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // txtDescuento
            // 
            this.txtDescuento.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescuento.Location = new System.Drawing.Point(875, 59);
            this.txtDescuento.Margin = new System.Windows.Forms.Padding(4);
            this.txtDescuento.MaxLength = 4;
            this.txtDescuento.Name = "txtDescuento";
            this.txtDescuento.Size = new System.Drawing.Size(65, 23);
            this.txtDescuento.TabIndex = 3;
            this.txtDescuento.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtCantidad
            // 
            this.txtCantidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCantidad.Location = new System.Drawing.Point(651, 59);
            this.txtCantidad.Margin = new System.Windows.Forms.Padding(4);
            this.txtCantidad.MaxLength = 15;
            this.txtCantidad.Name = "txtCantidad";
            this.txtCantidad.Size = new System.Drawing.Size(105, 23);
            this.txtCantidad.TabIndex = 2;
            this.txtCantidad.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtPrecio
            // 
            this.txtPrecio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrecio.Location = new System.Drawing.Point(128, 59);
            this.txtPrecio.Margin = new System.Windows.Forms.Padding(4);
            this.txtPrecio.MaxLength = 15;
            this.txtPrecio.Name = "txtPrecio";
            this.txtPrecio.ReadOnly = true;
            this.txtPrecio.Size = new System.Drawing.Size(105, 23);
            this.txtPrecio.TabIndex = 5;
            this.txtPrecio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(779, 64);
            this.label38.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(90, 17);
            this.label38.TabIndex = 7;
            this.label38.Text = "Descuento:";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(565, 64);
            this.label39.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(77, 17);
            this.label39.TabIndex = 6;
            this.label39.Text = "Cantidad:";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(60, 64);
            this.label40.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(59, 17);
            this.label40.TabIndex = 5;
            this.label40.Text = "Precio:";
            // 
            // cboProducto
            // 
            this.cboProducto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProducto.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboProducto.FormattingEnabled = true;
            this.cboProducto.Location = new System.Drawing.Point(672, 20);
            this.cboProducto.Margin = new System.Windows.Forms.Padding(4);
            this.cboProducto.Name = "cboProducto";
            this.cboProducto.Size = new System.Drawing.Size(393, 25);
            this.cboProducto.TabIndex = 1;
            this.cboProducto.SelectedIndexChanged += new System.EventHandler(this.cboProducto_SelectedIndexChanged);
            // 
            // cboCategoria
            // 
            this.cboCategoria.BackColor = System.Drawing.SystemColors.Window;
            this.cboCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCategoria.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCategoria.FormattingEnabled = true;
            this.cboCategoria.Location = new System.Drawing.Point(128, 20);
            this.cboCategoria.Margin = new System.Windows.Forms.Padding(4);
            this.cboCategoria.Name = "cboCategoria";
            this.cboCategoria.Size = new System.Drawing.Size(393, 25);
            this.cboCategoria.TabIndex = 0;
            this.cboCategoria.SelectedIndexChanged += new System.EventHandler(this.cboCategoria_SelectedIndexChanged);
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(589, 25);
            this.label37.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(78, 17);
            this.label37.TabIndex = 0;
            this.label37.Text = "Producto:";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(33, 25);
            this.label36.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(83, 17);
            this.label36.TabIndex = 0;
            this.label36.Text = "Categoría:";
            // 
            // GrbPedido
            // 
            this.GrbPedido.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GrbPedido.Controls.Add(this.BtnNota);
            this.GrbPedido.Controls.Add(this.txtTotal);
            this.GrbPedido.Controls.Add(this.txtCliente);
            this.GrbPedido.Controls.Add(this.txtId);
            this.GrbPedido.Controls.Add(this.label3);
            this.GrbPedido.Controls.Add(this.label20);
            this.GrbPedido.Controls.Add(this.label2);
            this.GrbPedido.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrbPedido.Location = new System.Drawing.Point(459, 384);
            this.GrbPedido.Margin = new System.Windows.Forms.Padding(4);
            this.GrbPedido.Name = "GrbPedido";
            this.GrbPedido.Padding = new System.Windows.Forms.Padding(4);
            this.GrbPedido.Size = new System.Drawing.Size(1099, 118);
            this.GrbPedido.TabIndex = 4;
            this.GrbPedido.TabStop = false;
            this.GrbPedido.Text = "»   Pedido:   «";
            this.GrbPedido.Paint += new System.Windows.Forms.PaintEventHandler(this.GrbPaint);
            // 
            // BtnNota
            // 
            this.BtnNota.Enabled = false;
            this.BtnNota.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnNota.Location = new System.Drawing.Point(503, 80);
            this.BtnNota.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnNota.Name = "BtnNota";
            this.BtnNota.Size = new System.Drawing.Size(220, 33);
            this.BtnNota.TabIndex = 20;
            this.BtnNota.Text = "Nota de remisión";
            this.BtnNota.UseVisualStyleBackColor = true;
            this.BtnNota.Click += new System.EventHandler(this.BtnNota_Click);
            // 
            // txtTotal
            // 
            this.txtTotal.BackColor = System.Drawing.Color.White;
            this.txtTotal.Location = new System.Drawing.Point(120, 81);
            this.txtTotal.Margin = new System.Windows.Forms.Padding(4);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.ReadOnly = true;
            this.txtTotal.Size = new System.Drawing.Size(340, 23);
            this.txtTotal.TabIndex = 2;
            this.txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtCliente
            // 
            this.txtCliente.BackColor = System.Drawing.Color.White;
            this.txtCliente.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCliente.Location = new System.Drawing.Point(120, 52);
            this.txtCliente.Margin = new System.Windows.Forms.Padding(4);
            this.txtCliente.Name = "txtCliente";
            this.txtCliente.ReadOnly = true;
            this.txtCliente.Size = new System.Drawing.Size(927, 23);
            this.txtCliente.TabIndex = 1;
            // 
            // txtId
            // 
            this.txtId.BackColor = System.Drawing.Color.White;
            this.txtId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtId.Location = new System.Drawing.Point(120, 22);
            this.txtId.Margin = new System.Windows.Forms.Padding(4);
            this.txtId.MaxLength = 10;
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(132, 23);
            this.txtId.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(55, 84);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 20);
            this.label3.TabIndex = 17;
            this.label3.Text = "Total:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(51, 57);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(63, 17);
            this.label20.TabIndex = 18;
            this.label20.Text = "Cliente:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(88, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 17);
            this.label2.TabIndex = 19;
            this.label2.Text = "Id:";
            // 
            // GrbBuscar
            // 
            this.GrbBuscar.Controls.Add(this.btnLimpiar);
            this.GrbBuscar.Controls.Add(this.btnBuscar);
            this.GrbBuscar.Controls.Add(this.label19);
            this.GrbBuscar.Controls.Add(this.txtBDirigidoa);
            this.GrbBuscar.Controls.Add(this.label18);
            this.GrbBuscar.Controls.Add(this.txtBCompañiaT);
            this.GrbBuscar.Controls.Add(this.label17);
            this.GrbBuscar.Controls.Add(this.txtBEmpleado);
            this.GrbBuscar.Controls.Add(this.label15);
            this.GrbBuscar.Controls.Add(this.label16);
            this.GrbBuscar.Controls.Add(this.dtpBFEnvioFin);
            this.GrbBuscar.Controls.Add(this.dtpBFEnvioIni);
            this.GrbBuscar.Controls.Add(this.chkBFEnvioNull);
            this.GrbBuscar.Controls.Add(this.label14);
            this.GrbBuscar.Controls.Add(this.label12);
            this.GrbBuscar.Controls.Add(this.label13);
            this.GrbBuscar.Controls.Add(this.dtpBFRequeridoFin);
            this.GrbBuscar.Controls.Add(this.dtpBFRequeridoIni);
            this.GrbBuscar.Controls.Add(this.chkBFRequeridoNull);
            this.GrbBuscar.Controls.Add(this.label11);
            this.GrbBuscar.Controls.Add(this.chkBFPedidoNull);
            this.GrbBuscar.Controls.Add(this.label10);
            this.GrbBuscar.Controls.Add(this.label9);
            this.GrbBuscar.Controls.Add(this.dtpBFPedidoFin);
            this.GrbBuscar.Controls.Add(this.dtpBFPedidoIni);
            this.GrbBuscar.Controls.Add(this.label8);
            this.GrbBuscar.Controls.Add(this.txtBCliente);
            this.GrbBuscar.Controls.Add(this.label7);
            this.GrbBuscar.Controls.Add(this.txtBIdFinal);
            this.GrbBuscar.Controls.Add(this.txtBIdInicial);
            this.GrbBuscar.Controls.Add(this.label6);
            this.GrbBuscar.Controls.Add(this.label5);
            this.GrbBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrbBuscar.Location = new System.Drawing.Point(21, 374);
            this.GrbBuscar.Margin = new System.Windows.Forms.Padding(4);
            this.GrbBuscar.Name = "GrbBuscar";
            this.GrbBuscar.Padding = new System.Windows.Forms.Padding(4);
            this.GrbBuscar.Size = new System.Drawing.Size(405, 551);
            this.GrbBuscar.TabIndex = 3;
            this.GrbBuscar.TabStop = false;
            this.GrbBuscar.Text = "»   Buscar un pedido:   «";
            this.GrbBuscar.Paint += new System.Windows.Forms.PaintEventHandler(this.GrbPaint);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpiar.Location = new System.Drawing.Point(103, 457);
            this.btnLimpiar.Margin = new System.Windows.Forms.Padding(4);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(133, 28);
            this.btnLimpiar.TabIndex = 15;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(247, 457);
            this.btnBuscar.Margin = new System.Windows.Forms.Padding(4);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(133, 28);
            this.btnBuscar.TabIndex = 16;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(41, 412);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(73, 17);
            this.label19.TabIndex = 62;
            this.label19.Text = "Enviar a:";
            // 
            // txtBDirigidoa
            // 
            this.txtBDirigidoa.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBDirigidoa.Location = new System.Drawing.Point(124, 407);
            this.txtBDirigidoa.Margin = new System.Windows.Forms.Padding(4);
            this.txtBDirigidoa.MaxLength = 40;
            this.txtBDirigidoa.Name = "txtBDirigidoa";
            this.txtBDirigidoa.Size = new System.Drawing.Size(255, 23);
            this.txtBDirigidoa.TabIndex = 14;
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(11, 366);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(107, 31);
            this.label18.TabIndex = 61;
            this.label18.Text = "Compañía transportista:";
            this.label18.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtBCompañiaT
            // 
            this.txtBCompañiaT.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBCompañiaT.Location = new System.Drawing.Point(124, 368);
            this.txtBCompañiaT.Margin = new System.Windows.Forms.Padding(4);
            this.txtBCompañiaT.MaxLength = 40;
            this.txtBCompañiaT.Name = "txtBCompañiaT";
            this.txtBCompañiaT.Size = new System.Drawing.Size(255, 23);
            this.txtBCompañiaT.TabIndex = 13;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(31, 329);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(83, 17);
            this.label17.TabIndex = 60;
            this.label17.Text = "Vendedor:";
            // 
            // txtBEmpleado
            // 
            this.txtBEmpleado.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBEmpleado.Location = new System.Drawing.Point(124, 324);
            this.txtBEmpleado.Margin = new System.Windows.Forms.Padding(4);
            this.txtBEmpleado.MaxLength = 40;
            this.txtBEmpleado.Name = "txtBEmpleado";
            this.txtBEmpleado.Size = new System.Drawing.Size(255, 23);
            this.txtBEmpleado.TabIndex = 12;
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(197, 274);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(52, 31);
            this.label15.TabIndex = 59;
            this.label15.Text = "Fecha final:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label16
            // 
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(17, 274);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(52, 31);
            this.label16.TabIndex = 58;
            this.label16.Text = "Fecha inicial:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dtpBFEnvioFin
            // 
            this.dtpBFEnvioFin.Checked = false;
            this.dtpBFEnvioFin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpBFEnvioFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBFEnvioFin.Location = new System.Drawing.Point(253, 277);
            this.dtpBFEnvioFin.Margin = new System.Windows.Forms.Padding(4);
            this.dtpBFEnvioFin.Name = "dtpBFEnvioFin";
            this.dtpBFEnvioFin.ShowCheckBox = true;
            this.dtpBFEnvioFin.Size = new System.Drawing.Size(125, 23);
            this.dtpBFEnvioFin.TabIndex = 11;
            this.dtpBFEnvioFin.ValueChanged += new System.EventHandler(this.dtpBFEnvioFin_ValueChanged);
            this.dtpBFEnvioFin.Leave += new System.EventHandler(this.dtpBFEnvioFin_Leave);
            // 
            // dtpBFEnvioIni
            // 
            this.dtpBFEnvioIni.Checked = false;
            this.dtpBFEnvioIni.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpBFEnvioIni.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBFEnvioIni.Location = new System.Drawing.Point(71, 277);
            this.dtpBFEnvioIni.Margin = new System.Windows.Forms.Padding(4);
            this.dtpBFEnvioIni.Name = "dtpBFEnvioIni";
            this.dtpBFEnvioIni.ShowCheckBox = true;
            this.dtpBFEnvioIni.Size = new System.Drawing.Size(125, 23);
            this.dtpBFEnvioIni.TabIndex = 10;
            this.dtpBFEnvioIni.ValueChanged += new System.EventHandler(this.dtpBFEnvioIni_ValueChanged);
            this.dtpBFEnvioIni.Leave += new System.EventHandler(this.dtpBFEnvioIni_Leave);
            // 
            // chkBFEnvioNull
            // 
            this.chkBFEnvioNull.AutoSize = true;
            this.chkBFEnvioNull.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBFEnvioNull.Location = new System.Drawing.Point(267, 246);
            this.chkBFEnvioNull.Margin = new System.Windows.Forms.Padding(4);
            this.chkBFEnvioNull.Name = "chkBFEnvioNull";
            this.chkBFEnvioNull.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkBFEnvioNull.Size = new System.Drawing.Size(99, 17);
            this.chkBFEnvioNull.TabIndex = 9;
            this.chkBFEnvioNull.Text = "Fecha = null";
            this.chkBFEnvioNull.UseVisualStyleBackColor = true;
            this.chkBFEnvioNull.CheckedChanged += new System.EventHandler(this.chkBFEnvioNull_CheckedChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(17, 245);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(124, 17);
            this.label14.TabIndex = 57;
            this.label14.Text = "Fecha de envío:";
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(197, 201);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(52, 31);
            this.label12.TabIndex = 56;
            this.label12.Text = "Fecha final:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(17, 201);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(52, 31);
            this.label13.TabIndex = 54;
            this.label13.Text = "Fecha inicial:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dtpBFRequeridoFin
            // 
            this.dtpBFRequeridoFin.Checked = false;
            this.dtpBFRequeridoFin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpBFRequeridoFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBFRequeridoFin.Location = new System.Drawing.Point(253, 203);
            this.dtpBFRequeridoFin.Margin = new System.Windows.Forms.Padding(4);
            this.dtpBFRequeridoFin.Name = "dtpBFRequeridoFin";
            this.dtpBFRequeridoFin.ShowCheckBox = true;
            this.dtpBFRequeridoFin.Size = new System.Drawing.Size(125, 23);
            this.dtpBFRequeridoFin.TabIndex = 8;
            this.dtpBFRequeridoFin.ValueChanged += new System.EventHandler(this.dtpBFRequeridoFin_ValueChanged);
            this.dtpBFRequeridoFin.Leave += new System.EventHandler(this.dtpBFRequeridoFin_Leave);
            // 
            // dtpBFRequeridoIni
            // 
            this.dtpBFRequeridoIni.Checked = false;
            this.dtpBFRequeridoIni.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpBFRequeridoIni.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBFRequeridoIni.Location = new System.Drawing.Point(71, 203);
            this.dtpBFRequeridoIni.Margin = new System.Windows.Forms.Padding(4);
            this.dtpBFRequeridoIni.Name = "dtpBFRequeridoIni";
            this.dtpBFRequeridoIni.ShowCheckBox = true;
            this.dtpBFRequeridoIni.Size = new System.Drawing.Size(125, 23);
            this.dtpBFRequeridoIni.TabIndex = 7;
            this.dtpBFRequeridoIni.ValueChanged += new System.EventHandler(this.dtpBFRequeridoIni_ValueChanged);
            this.dtpBFRequeridoIni.Leave += new System.EventHandler(this.dtpBFRequeridoIni_Leave);
            // 
            // chkBFRequeridoNull
            // 
            this.chkBFRequeridoNull.AutoSize = true;
            this.chkBFRequeridoNull.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBFRequeridoNull.Location = new System.Drawing.Point(267, 171);
            this.chkBFRequeridoNull.Margin = new System.Windows.Forms.Padding(4);
            this.chkBFRequeridoNull.Name = "chkBFRequeridoNull";
            this.chkBFRequeridoNull.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkBFRequeridoNull.Size = new System.Drawing.Size(99, 17);
            this.chkBFRequeridoNull.TabIndex = 6;
            this.chkBFRequeridoNull.Text = "Fecha = null";
            this.chkBFRequeridoNull.UseVisualStyleBackColor = true;
            this.chkBFRequeridoNull.CheckedChanged += new System.EventHandler(this.chkBFRequeridoNull_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 171);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(141, 17);
            this.label11.TabIndex = 50;
            this.label11.Text = "Fecha de entrega:";
            // 
            // chkBFPedidoNull
            // 
            this.chkBFPedidoNull.AutoSize = true;
            this.chkBFPedidoNull.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBFPedidoNull.Location = new System.Drawing.Point(267, 103);
            this.chkBFPedidoNull.Margin = new System.Windows.Forms.Padding(4);
            this.chkBFPedidoNull.Name = "chkBFPedidoNull";
            this.chkBFPedidoNull.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkBFPedidoNull.Size = new System.Drawing.Size(99, 17);
            this.chkBFPedidoNull.TabIndex = 3;
            this.chkBFPedidoNull.Text = "Fecha = null";
            this.chkBFPedidoNull.UseVisualStyleBackColor = true;
            this.chkBFPedidoNull.CheckedChanged += new System.EventHandler(this.chkBFPedidoNull_CheckedChanged);
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(197, 129);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 31);
            this.label10.TabIndex = 46;
            this.label10.Text = "Fecha final:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(17, 129);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 31);
            this.label9.TabIndex = 44;
            this.label9.Text = "Fecha inicial:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dtpBFPedidoFin
            // 
            this.dtpBFPedidoFin.Checked = false;
            this.dtpBFPedidoFin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpBFPedidoFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBFPedidoFin.Location = new System.Drawing.Point(253, 132);
            this.dtpBFPedidoFin.Margin = new System.Windows.Forms.Padding(4);
            this.dtpBFPedidoFin.Name = "dtpBFPedidoFin";
            this.dtpBFPedidoFin.ShowCheckBox = true;
            this.dtpBFPedidoFin.Size = new System.Drawing.Size(125, 23);
            this.dtpBFPedidoFin.TabIndex = 5;
            this.dtpBFPedidoFin.ValueChanged += new System.EventHandler(this.dtpBFPedidoFin_ValueChanged);
            this.dtpBFPedidoFin.Leave += new System.EventHandler(this.dtpBFPedidoFin_Leave);
            // 
            // dtpBFPedidoIni
            // 
            this.dtpBFPedidoIni.Checked = false;
            this.dtpBFPedidoIni.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpBFPedidoIni.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBFPedidoIni.Location = new System.Drawing.Point(71, 132);
            this.dtpBFPedidoIni.Margin = new System.Windows.Forms.Padding(4);
            this.dtpBFPedidoIni.Name = "dtpBFPedidoIni";
            this.dtpBFPedidoIni.ShowCheckBox = true;
            this.dtpBFPedidoIni.Size = new System.Drawing.Size(125, 23);
            this.dtpBFPedidoIni.TabIndex = 4;
            this.dtpBFPedidoIni.ValueChanged += new System.EventHandler(this.dtpBFPedidoIni_ValueChanged);
            this.dtpBFPedidoIni.Leave += new System.EventHandler(this.dtpBFPedidoIni_Leave);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 102);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(134, 17);
            this.label8.TabIndex = 41;
            this.label8.Text = "Fecha de pedido:";
            // 
            // txtBCliente
            // 
            this.txtBCliente.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBCliente.Location = new System.Drawing.Point(124, 63);
            this.txtBCliente.Margin = new System.Windows.Forms.Padding(4);
            this.txtBCliente.MaxLength = 40;
            this.txtBCliente.Name = "txtBCliente";
            this.txtBCliente.Size = new System.Drawing.Size(255, 23);
            this.txtBCliente.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(51, 68);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 17);
            this.label7.TabIndex = 37;
            this.label7.Text = "Cliente:";
            // 
            // txtBIdFinal
            // 
            this.txtBIdFinal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBIdFinal.Location = new System.Drawing.Point(295, 28);
            this.txtBIdFinal.Margin = new System.Windows.Forms.Padding(4);
            this.txtBIdFinal.MaxLength = 10;
            this.txtBIdFinal.Name = "txtBIdFinal";
            this.txtBIdFinal.Size = new System.Drawing.Size(87, 23);
            this.txtBIdFinal.TabIndex = 1;
            this.txtBIdFinal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBIdFinal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBIdFinal_KeyPress);
            this.txtBIdFinal.Leave += new System.EventHandler(this.txtBIdFinal_Leave);
            // 
            // txtBIdInicial
            // 
            this.txtBIdInicial.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBIdInicial.Location = new System.Drawing.Point(124, 28);
            this.txtBIdInicial.Margin = new System.Windows.Forms.Padding(4);
            this.txtBIdInicial.MaxLength = 10;
            this.txtBIdInicial.Name = "txtBIdInicial";
            this.txtBIdInicial.Size = new System.Drawing.Size(87, 23);
            this.txtBIdInicial.TabIndex = 0;
            this.txtBIdInicial.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBIdInicial.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBIdInicial_KeyPress);
            this.txtBIdInicial.Leave += new System.EventHandler(this.txtBIdInicial_Leave);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(220, 33);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 17);
            this.label6.TabIndex = 34;
            this.label6.Text = "Id final:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 33);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 17);
            this.label5.TabIndex = 31;
            this.label5.Text = "Id inicial:";
            // 
            // GrbPedidos
            // 
            this.GrbPedidos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GrbPedidos.Controls.Add(this.DgvPedidos);
            this.GrbPedidos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrbPedidos.Location = new System.Drawing.Point(21, 59);
            this.GrbPedidos.Margin = new System.Windows.Forms.Padding(4);
            this.GrbPedidos.Name = "GrbPedidos";
            this.GrbPedidos.Padding = new System.Windows.Forms.Padding(4);
            this.GrbPedidos.Size = new System.Drawing.Size(1536, 295);
            this.GrbPedidos.TabIndex = 2;
            this.GrbPedidos.TabStop = false;
            this.GrbPedidos.Text = "»   Pedidos:   «";
            this.GrbPedidos.Paint += new System.Windows.Forms.PaintEventHandler(this.GrbPaint);
            // 
            // DgvPedidos
            // 
            this.DgvPedidos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvPedidos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvPedidos.Location = new System.Drawing.Point(4, 20);
            this.DgvPedidos.Margin = new System.Windows.Forms.Padding(4);
            this.DgvPedidos.Name = "DgvPedidos";
            this.DgvPedidos.RowHeadersWidth = 51;
            this.DgvPedidos.Size = new System.Drawing.Size(1528, 271);
            this.DgvPedidos.TabIndex = 0;
            this.DgvPedidos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvPedidos_CellClick);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(21, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1535, 39);
            this.label1.TabIndex = 1;
            this.label1.Text = "     Busque el pedido y seleccionelo en la lista que se muestra";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // FrmPedidosDetalleCrud
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1579, 937);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmPedidosDetalleCrud";
            this.Text = "» Mantenimiento de detalle de pedidos «";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmPedidosDetalleCrud_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmPedidosDetalleCrud_FormClosed);
            this.Load += new System.EventHandler(this.FrmPedidosDetalleCrud_Load);
            this.panel1.ResumeLayout(false);
            this.GrbDetalle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvDetalle)).EndInit();
            this.GrbAgregarProducto.ResumeLayout(false);
            this.GrbAgregarProducto.PerformLayout();
            this.GrbPedido.ResumeLayout(false);
            this.GrbPedido.PerformLayout();
            this.GrbBuscar.ResumeLayout(false);
            this.GrbBuscar.PerformLayout();
            this.GrbPedidos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvPedidos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox GrbPedidos;
        private System.Windows.Forms.DataGridView DgvPedidos;
        private System.Windows.Forms.GroupBox GrbBuscar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtBDirigidoa;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtBCompañiaT;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtBEmpleado;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.DateTimePicker dtpBFEnvioFin;
        private System.Windows.Forms.DateTimePicker dtpBFEnvioIni;
        private System.Windows.Forms.CheckBox chkBFEnvioNull;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.DateTimePicker dtpBFRequeridoFin;
        private System.Windows.Forms.DateTimePicker dtpBFRequeridoIni;
        private System.Windows.Forms.CheckBox chkBFRequeridoNull;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chkBFPedidoNull;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dtpBFPedidoFin;
        private System.Windows.Forms.DateTimePicker dtpBFPedidoIni;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtBCliente;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtBIdFinal;
        private System.Windows.Forms.TextBox txtBIdInicial;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox GrbPedido;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.TextBox txtCliente;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox GrbAgregarProducto;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.TextBox txtDescuento;
        private System.Windows.Forms.TextBox txtCantidad;
        private System.Windows.Forms.TextBox txtPrecio;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.ComboBox cboProducto;
        private System.Windows.Forms.ComboBox cboCategoria;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUInventario;
        private System.Windows.Forms.GroupBox GrbDetalle;
        private System.Windows.Forms.DataGridView DgvDetalle;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button BtnNota;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Producto;
        private System.Windows.Forms.DataGridViewTextBoxColumn Precio;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cantidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn Descuento;
        private System.Windows.Forms.DataGridViewTextBoxColumn Importe;
        private System.Windows.Forms.DataGridViewButtonColumn Modificar;
        private System.Windows.Forms.DataGridViewButtonColumn Eliminar;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductoId;
        private System.Windows.Forms.DataGridViewTextBoxColumn RowVersion;
    }
}