using System.Threading.Tasks;
using RestApiDating.Models;

namespace RestApiDating.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExits(string username);
    }
}