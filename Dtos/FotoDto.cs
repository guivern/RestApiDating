using System;

namespace RestApiDating.Dtos
{
    public class FotoDto
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCarga { get; set; }
        public bool EsPrincipal { get; set; }
        public string IdPublico { get; set; }
    }
}