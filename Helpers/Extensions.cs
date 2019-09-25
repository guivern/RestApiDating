using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace RestApiDating.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static void AddPagination(this HttpResponse response, int currentPage, int pageSize, int totalCount, int totalPages)
        {
            var paginationHeader = new { currentPage, pageSize, totalCount, totalPages };

            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination"); // expone la cabecera
        }

        public static int CalcularEdad(this DateTime? fechaNacimiento)
        {
            if (fechaNacimiento != null)
            {
                var edad = DateTime.Today.Year - fechaNacimiento.Value.Year;
                if (fechaNacimiento.Value.AddYears(edad) > DateTime.Today)
                {
                    edad = edad - 1;
                }
                return edad;
            }

            return 0;
        }
    }
}