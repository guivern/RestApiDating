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
            return await _context.Fotos.SingleOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Foto> GetFotoPrincipal(int userId)
        {
            return await _context.Fotos
                .SingleOrDefaultAsync(f => f.UserId == userId && f.EsPrincipal);
        }

        public async Task<Like> GetLike(int likerId, int likedId)
        {
            return await _context.Likes
                .FirstOrDefaultAsync(l => l.LikedId == likedId && l.LikedId == likedId);
        }

        public async Task<User> GetUser(int id)
        {
            return await _context.Users
                .Include(u => u.Fotos)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var query = _context.Users.Include(u => u.Fotos).AsQueryable();

            // para no incluir al usuario logueado
            query = query.Where(u => u.Id != userParams.UserId);
            // para listar usuarios del sexo opuesto
            query = query.Where(u => u.Genero.ToLower().Equals(userParams.Genero));

            if (userParams.Likes)
            {
                var userLikes = await GetUserLikesIdList(userParams.UserId);
                query = query.Where(u => userLikes.Contains(u.Id));
            }

            if (userParams.Likers)
            {
                var userLikers = await GetUserLikersIdList(userParams.UserId);
                query = query.Where(u => userLikers.Contains(u.Id));
            }

            // el siguiente calculo es necesario para filtrar por rango de edad
            // porque en la bd almacenamos la fecha de nacimiento y no la edad
            var fechaNacMin = DateTime.Today.AddYears(-userParams.EdadMax - 1);
            var fechaNacMax = DateTime.Today.AddYears(-userParams.EdadMin);

            query = query.Where(u => u.FechaNacimiento >= fechaNacMin
                && u.FechaNacimiento <= fechaNacMax);

            if (!string.IsNullOrEmpty(userParams.OrderBy) && userParams.OrderBy.Equals("fechaCreacion"))
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

        /// <summary>
        /// Obtiene una lista de ids de usuarios que le gustan a userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<IEnumerable<int>> GetUserLikesIdList(int userId)
        {
            return await _context.Likes
                .Where(l => l.LikerId == userId)
                .Select(l => l.LikedId)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene una lista de ids de usuarios que dieron like a userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<IEnumerable<int>> GetUserLikersIdList(int userId)
        {
            return await _context.Likes
                .Where(l => l.LikedId == userId)
                .Select(l => l.LikerId)
                .ToListAsync();
        }

        public async Task<Mensaje> GetMensaje(int id)
        {
            return await _context.Mensajes
                .Include(m => m.Emisor)
                .Include(m => m.Receptor)
                .SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedList<Mensaje>> GetMensajesForUser(MensajesParams mensajesParams)
        {
            var query = _context.Mensajes
                .Include(m => m.Emisor).ThenInclude(u => u.Fotos)
                .Include(m => m.Receptor).ThenInclude(u => u.Fotos)
                .AsQueryable();

            switch (mensajesParams.Buzon?.ToLower())
            {
                case "entrada":
                    query = query.Where(m => m.ReceptorId == mensajesParams.UserId);
                    break;
                case "salida":
                    query = query.Where(m => m.EmisorId == mensajesParams.UserId);
                    break;
                default: // retornamos los mensajes no leidos
                    query = query.Where(m => m.ReceptorId == mensajesParams.UserId 
                        && !m.HaSidoLeido);
                    break;
            }

            query = query.OrderByDescending(m => m.FechaEnvio);

            return await PagedList<Mensaje>.CreateAsync(query, mensajesParams.PageNumber, mensajesParams.PageSize);

        }

        public async Task<IEnumerable<Mensaje>> GetConversacion(int emisorId, int receptorId)
        {
             var conversacion = await _context.Mensajes
                .Include(m => m.Emisor).ThenInclude(u => u.Fotos)
                .Include(m => m.Receptor).ThenInclude(u => u.Fotos)
                // en una conversacion tanto emisor como receptor pueden intercambiar roles
                .Where(m => m.EmisorId == emisorId && m.ReceptorId == receptorId
                    || m.EmisorId == receptorId && m.ReceptorId == emisorId)
                .OrderByDescending(m => m.Id)
                .ToListAsync();

            return conversacion;
        }
    }
}