using System.Collections.Generic;
using System.Data;

namespace NorthwindTradersV4MySql
{
    internal static class ReportDataTableAdapter
    {
        public static DataTable ConvertirVentasMensuales(List<DtoVentaMensual> ventas)
        {
            var dt = new DataTable();
            dt.Columns.Add("Mes", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("NombreMes", typeof(string));
            foreach (var v in ventas)
                dt.Rows.Add(v.Mes, v.Total, v.NombreMes);
            return dt;
        }

        public static DataTable ConvertirVentaAnualComparativa(List<DtoVentaAnualComparativa> ventas)
        {
            var dt = new DataTable();
            dt.Columns.Add("Mes", typeof(int));
            dt.Columns.Add("NombreMes", typeof(string));
            dt.Columns.Add("Año", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            foreach (var v in ventas)
                dt.Rows.Add(v.Mes, v.NombreMes, v.Año, v.Total);
            return dt;
        }

        public static DataTable ConvertirProductoMasVendido(List<DtoProductoMasVendido> productos)
        {
            var dt = new DataTable();
            dt.Columns.Add("Posicion", typeof(int));
            dt.Columns.Add("NombreProducto", typeof(string));
            dt.Columns.Add("CantidadVendida", typeof(int));
            foreach (var p in productos)
                dt.Rows.Add(p.Posicion, p.NombreProducto, p.CantidadVendida);
            return dt;
        }
    }
}
