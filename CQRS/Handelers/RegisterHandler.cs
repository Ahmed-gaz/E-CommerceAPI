using E_CommerceAPI.CQRS.Commands;
using E_CommerceAPI.DTOs;
using E_CommerceAPI.Models;
using E_CommerceAPI.Repos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Security.Cryptography;

namespace E_CommerceAPI.CQRS.Handelers
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, User?>
    {
        private readonly IAuthenticationRepo _repo;
        public RegisterHandler(IAuthenticationRepo repo)
        {
         _repo = repo;
        }
        public async Task<User?> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {

            var user = await _repo.Register(request.registerDto);

            if (user == null) 
                return null;

            return user;
        }
    }
}
