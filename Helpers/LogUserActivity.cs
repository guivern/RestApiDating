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
            // next() llama al action method
            // todo codigo antes de next(), se ejecuta antes de ejecutarse el action
            // todo codigo despues de next(), se ejecuta despues de ejecutarse action
            var resultContext = await next();
            
            // esto se ejecuta luego de ejectuarse el action
            var userId = int.Parse(resultContext.HttpContext
                .User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var user = await _context.Users.FindAsync(userId);
            user.UltimaConexion = DateTime.Now;

            await _context.SaveChangesAsync();
        }
    }
}