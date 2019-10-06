using System;

namespace RestApiDating.Dtos
{
    public class MensajeCreateDto
    {
        public int ReceptorId { get; set; }
        public string Contenido { get; set; }
    }
}