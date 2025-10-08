namespace NorthwindTradersV4MySql
{
    internal class PedidoDetalle
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public decimal Discount { get; set; }
        public int RowVersion { get; set; }
    }
}
