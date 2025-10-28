using System;

namespace NorthwindTradersV4MySql
{
    internal class Usuario
    {
        public int Id { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string Nombres { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public DateTime FechaCaptura { get; set; }
        public DateTime FechaModificacion { get; set; }
        public bool Estatus { get; set; }
    }
}
