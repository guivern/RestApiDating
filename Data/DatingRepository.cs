using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestApiDating.Models;

namespace RestApiDating.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;

        public DatingRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Foto> GetFoto(int id)
        {
            var foto = await _context.Fotos.SingleOrDefaultAsync(f => f.Id == id);
            return foto;
        }

        public async Task<Foto> GetFotoPrincipal(int userId)
        {
            var foto = await _context.Fotos.SingleOrDefaultAsync(f => f.UserId == userId && f.EsPrincipal);
            return foto;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users
            .Include(u => u.Fotos)
            .FirstOrDefaultAsync(u => u.Id == id);
            
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users
            .Include(u => u.Fotos)
            .ToListAsync();
            
            return users;
        }

        public async Task<bool> SaveAll()
        {
            // retorna 0 si no pudo guardar ningun cambio 
            return await _context.SaveChangesAsync() > 0;
        }
    }
}