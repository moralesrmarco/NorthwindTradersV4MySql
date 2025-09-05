using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindTradersV4MySql
{
    internal class Utils
    {
        #region VariablesGlobales
        public static string clbdd = "Consultando la base de datos... ";
        public static string oueclbdd = "Ocurrio un error con la base de datos:\n";
        public static string oue = "Ocurrio un error:\n";
        public static string nwtr = "» Northwind Traders Ver 4.0 MySql «";
        public static string preguntaCerrar = "¿Esta seguro de querer cerrar el formulario?, si responde SI, se perderan los datos no guardados";
        public static string insertandoRegistro = "Insertando registro en la base de datos...";
        public static string modificandoRegistro = "Modificando registro en la base de datos...";
        public static string eliminandoRegistro = "Eliminando registro en la base de datos...";
        public static string errorCriterioSelec = "Error: Proporcione los criterios de selección";
        public static string noDatos = "No se encontraron datos para mostrar en el reporte";
        #endregion
    }
}
