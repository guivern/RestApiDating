using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestApiDating.Models;

namespace RestApiDating.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username.Equals(username));

            if(user == null) return null;

            if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExits(string username)
        {
            return await _context.Users
            .AnyAsync(u => u.Username.Equals(username));
        }

        // Genera un salt y hash a partir del password
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key; // clave para generar el hash
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        // Chequea si el password proporcionado es correcto
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                // genera un hash a partir del password y el passwordSalt
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                // compara el hash generado con el hash original 
                for(int i = 0; i < computedHash.Length; i++)
                {
                    if(computedHash[i] != passwordHash[i]) return false; 
                }
                return true;
            }
        }
    }
}