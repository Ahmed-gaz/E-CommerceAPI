using E_CommerceAPI.DTOs;
using E_CommerceAPI.Models;

namespace E_CommerceAPI.Repos
{
    public interface IAuthenticationRepo
    {
        public Task<List<User>> GetUser();
        public Task<User?> Register(RegisterDto registerDto);
        public Task<User?> Login(LoginDto loginDto);

    }
}
