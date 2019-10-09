using System;

namespace RestApiDating.Dtos
{
    public class MensajeCreateDto
    {
        public int ReceptorId { get; set; }
        public string Contenido { get; set; }
        public DateTime FechaEnvio { get; set; } = DateTime.Now;
    }
}