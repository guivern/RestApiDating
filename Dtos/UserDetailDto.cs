using System;
using System.Collections.Generic;
using RestApiDating.Models;

namespace RestApiDating.Dtos
{
    public class UserDetailDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Genero { get; set; }
        public int Edad { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? UltimaConexion { get; set; }
        public string Introduccion { get; set; }
        public string Buscando { get; set; }
        public string Intereses { get; set; }
        public string Ciudad { get; set; }
        public string Pais { get; set; }
        public string FotoUrl { get; set; }
        public ICollection<FotoDto> Fotos { get; set; }
    }
}