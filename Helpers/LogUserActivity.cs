using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using RestApiDating.Data;

namespace RestApiDating.Helpers
{
    /// <summary>
    /// Action filter que permitirá registrar la última conexión del usuario,
    /// cada vez que el usuario realice alguna interacción con la API, se 
    /// actualizará su última conexión.
    /// </summary>
    public class LogUserActivity : IAsyncActionFilter
    {
        private DataContext _context;

        public LogUserActivity(DataContext context)
        {
            _context = context;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // context ejecuta el codigo antes/durante se esta ejecutando la accion
            // next ejecuta el codigo luego de ejecutarse la accion
            var resultContext = await next();
            
            var userId = int.Parse(resultContext.HttpContext
                .User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var user = await _context.Users.FindAsync(userId);
            user.UltimaConexion = DateTime.Now;

            await _context.SaveChangesAsync();
        }
    }
}