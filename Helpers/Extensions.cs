using System;
using Microsoft.AspNetCore.Http;

namespace RestApiDating.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin","*");
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