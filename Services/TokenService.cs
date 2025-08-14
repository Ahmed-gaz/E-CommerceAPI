namespace E_CommerceAPI.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
          _config = config;
        }
    }
}
