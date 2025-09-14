using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindTradersV4MySql
{
    internal class EmpleadoConReportsTo : Empleado
    {
        public string ReportsToName { get; set; }
    }
}
