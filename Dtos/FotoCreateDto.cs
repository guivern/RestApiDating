using Microsoft.AspNetCore.Http;

namespace RestApiDating.Dtos
{
    public class FotoCreateDto
    {
        public IFormFile File { get; set; }
        public string Descripcion { get; set; }
    }
}