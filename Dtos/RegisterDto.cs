using System;
using RestApiDating.Annotations;

namespace RestApiDating.Dtos
{
    public class RegisterDto
    {
        [Requerido]
        public string Username {get; set;}
        
        [Requerido]
        [LongMax(8, MinimumLength = 3, ErrorMessage = "La contraseña debe tener entre 4 y 8 caracteres")]
        public string Password {get; set;}

        [Requerido]
        public string Genero { get; set; }

        [Requerido]
        public string Nombre { get; set; }

        [Requerido]
        public string Apellido { get; set; }

        [Requerido]
        public DateTime FechaNacimiento { get; set; }

        [Requerido]
        public string Ciudad { get; set; }

        [Requerido]
        public string Pais { get; set; }
    }
}