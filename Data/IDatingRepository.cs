using System.Collections.Generic;
using System.Threading.Tasks;
using RestApiDating.Helpers;
using RestApiDating.Models;

namespace RestApiDating.Data
{
    public interface IDatingRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<User> GetUser(int id);
        Task<Foto> GetFoto(int id);
        Task<Foto> GetFotoPrincipal(int userId);
        Task<Like> GetLike(int likerId, int likedId);
    }
}