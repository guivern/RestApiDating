using System;

namespace RestApiDating.Dtos
{
    public class MensajeDetailDto
    {
        public int Id { get; set; }
        public int EmisorId { get; set; }
        public int ReceptorId { get; set; }
        // Siguiendo la convencion NombreClaseNombreAtributo
        // Automaper hace el siguiente mapeo NombreClase.NombreAtributo
        public string EmisorNombre { get; set; } // Emisor.Nombre
        public string ReceptorNombre { get; set; } // Receptor.Nombre
        public string FotoEmisorUrl { get; set; }
        public string FotoReceptorUrl { get; set; }
        public string Contenido { get; set; }
        public DateTime FechaEnvio { get; set; }
        public DateTime? FechaLectura { get; set; }
        public bool HaSidoLeido { get; set; }
    }
}