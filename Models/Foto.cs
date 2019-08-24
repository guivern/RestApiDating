using System;

namespace RestApiDating.Models
{
    public class Foto
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCarga { get; set; } = DateTime.Now;
        public bool EsPrincipal { get; set; }
        public string IdPublico { get; set; }
        // relcionaes EF por convencion
        // por default son onDeleteCascade
        public User User { get; set; }
        public int UserId { get; set; }
    }
}