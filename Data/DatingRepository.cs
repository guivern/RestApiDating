using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestApiDating.Helpers;
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
            var foto = await _context.Fotos.SingleOrDefaultAsync(f => f.UserId == userId 
                && f.EsPrincipal);
            return foto;
        }

        public async Task<Like> GetLike(int likerId, int likedId)
        {
            var like = await _context.Likes.FirstOrDefaultAsync(l => l.LikedId == likedId 
                && l.LikedId == likedId);
            return like;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.Fotos)
                .FirstOrDefaultAsync(u => u.Id == id);
            
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var query = _context.Users.Include(u => u.Fotos).AsQueryable();

            // para no incluir al usuario logueado
            query = query.Where(u => u.Id != userParams.UserId); 
            // para listar usuarios del sexo opuesto
            query = query.Where(u => u.Genero.ToLower().Equals(userParams.Genero)); 

            // el siguiente calculo es necesario para filtrar por rango de edad
            // porque en la bd almacenamos la fecha de nacimiento y no la edad
            var fechaNacMin = DateTime.Today.AddYears(-userParams.EdadMax - 1);
            var fechaNacMax = DateTime.Today.AddYears(-userParams.EdadMin);

            query = query.Where(u => u.FechaNacimiento >= fechaNacMin 
                && u.FechaNacimiento <= fechaNacMax);

            if(!string.IsNullOrEmpty(userParams.OrderBy) && userParams.OrderBy.Equals("fechaCreacion"))
            {
                query = query.OrderByDescending(u => u.FechaCreacion);
            }
            else
            {
                query = query.OrderByDescending(u => u.UltimaConexion);
            }

            PagedList<User> users = await PagedList<User>
                .CreateAsync(query, userParams.PageNumber, userParams.PageSize);
            
            return users;
        }

        public async Task<bool> SaveAll()
        {
            // retorna 0 si no pudo guardar ningun cambio 
            return await _context.SaveChangesAsync() > 0;
        }
    }
}