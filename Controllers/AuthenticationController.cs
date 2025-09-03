using E_CommerceAPI.DTOs;
using E_CommerceAPI.Models;
using E_CommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using E_CommerceAPI.CQRS.Queries;
using E_CommerceAPI.CQRS.Commands;


namespace E_CommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediatR;
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;

        public AuthenticationController(ApplicationDbContext context, TokenService tokenService, IMediator mediator)
        {
            _tokenService = tokenService;
            _context = context;
            _mediatR = mediator;
        }
        
        [HttpGet("get")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _mediatR.Send(new GetUserQuery());

            return Ok(users);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var user = await _mediatR.Send(new RegisterCommand(registerDto));
            
            if (user == null)
                return BadRequest("Email alredy exist");

            return Ok("User registered successfully");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var userToken = await _mediatR.Send(new LoginCommand(loginDto));
            
            if(userToken == null)
                return Unauthorized("Invalid email or password");
  
            return Ok(new { Token = userToken });
        }

    }
}
