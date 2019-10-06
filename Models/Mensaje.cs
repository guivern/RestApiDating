using System;

namespace RestApiDating.Models
{
    public class Mensaje
    {
        public int Id { get; set; }
        public int EmisorId { get; set; }
        public int ReceptorId { get; set; }
        public User Emisor { get; set; }
        public User Receptor { get; set; }
        public string Contenido { get; set; }
        public DateTime FechaEnvio { get; set; }
        public DateTime? FechaLectura { get; set; }
        public bool HaSidoLeido { get; set; }
        public bool HaSidoEliminadoPorEmisor { get; set; }
        public bool HaSidoEliminadoPorReceptor { get; set; }
    }
}