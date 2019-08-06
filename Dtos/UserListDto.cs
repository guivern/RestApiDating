using System;

namespace RestApiDating.Dtos
{
    public class UserListDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Genero { get; set; }
        public int Edad { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? UltimaConexion { get; set; }
        public string Ciudad { get; set; }
        public string Pais { get; set; }
        public string FotoUrl { get; set; }
    }
}