using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NorthwindTradersV4MySql
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 3.1 SELECT: obtener todos los productos con precio > 50
            var dbHelper = new DatabaseHelper();
            var sqlSelect = "SELECT EmployeeID, LastName, FirstName FROM Employees WHERE EmployeeID >= @EmployeeID";
            var parametrosSelect = new Dictionary<string, object>
            {
                {"@EmployeeID", 1}
            };
            var employees = dbHelper.EjecutarSelect(sqlSelect, parametrosSelect);

            // Recorres los resultados
            foreach (var fila in employees)
            {
                var respuesta = MessageBox.Show($"{fila["EmployeeID"]}: {fila["LastName"]}, {fila["FirstName"]}", Utils.nwtr, MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                if (respuesta == DialogResult.Cancel)
                    break;
            }

            //// 3.2 INSERT: agregar un nuevo producto
            //var sqlInsert = "INSERT INTO Employees (LastName, FirstName) VALUES (@LastName, @FirstName)";
            //var parametrosInsert = new Dictionary<string, object>
            //{
            //    {"@LastName", "Nuevo LastName"},
            //    {"@FirstName", "FirstName"}
            //};
            //int filas = dbHelper.EjecutarNoQuery(sqlInsert, parametrosInsert);
            //MessageBox.Show($"{filas} fila(s) insertada(s).");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dbHelper = new DatabaseHelper();
            dbHelper.ProbarConexion();
        }

    }
}
