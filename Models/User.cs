using System;
using System.Collections.Generic;

namespace RestApiDating.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Genero { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? UltimaConexion { get; set; } = DateTime.Now;
        public string Introduccion { get; set; }
        public string Buscando { get; set; }
        public string Intereses { get; set; }
        public string Ciudad { get; set; }
        public string Pais { get; set; }
        public virtual ICollection<Foto> Fotos { get; set; }
        public virtual ICollection<Like> Likes { get; set; } // likes que dio el usuario
        public virtual ICollection<Like> Likers { get; set; } // likes que recibio el usuario
        public virtual ICollection<Mensaje> MensajesEnviados { get; set; }
        public virtual ICollection<Mensaje> MensajesRecibidos { get; set; }
    }
}