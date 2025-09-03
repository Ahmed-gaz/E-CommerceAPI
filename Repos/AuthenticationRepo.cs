using E_CommerceAPI.CQRS.Commands;
using E_CommerceAPI.DTOs;
using E_CommerceAPI.Models;
using E_CommerceAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Threading.Tasks;


namespace E_CommerceAPI.Repos
{
    public class AuthenticationRepo : IAuthenticationRepo
    {
        private ApplicationDbContext _context;


        public AuthenticationRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<User>> GetUser()
        {
            return await _context.Users.ToListAsync();       
        }

        public async Task<User?> Login(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            return user;
        }

        public async Task<User?> Register(RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                return null;

            using var hmac = new HMACSHA256();
            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Password = registerDto.Password,
                Role = "user"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

      
    }
}
