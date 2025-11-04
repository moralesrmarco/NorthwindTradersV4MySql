namespace NorthwindTradersV4MySql
{
    internal class DtoProductoMasVendido
    {
        public int Posicion { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int CantidadVendida { get; set; }
    }
}
