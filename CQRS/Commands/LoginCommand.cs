using E_CommerceAPI.DTOs;
using E_CommerceAPI.Models;
using MediatR;

namespace E_CommerceAPI.CQRS.Commands
{
    public record LoginCommand(LoginDto loginDto) : IRequest<string>;
    
}
