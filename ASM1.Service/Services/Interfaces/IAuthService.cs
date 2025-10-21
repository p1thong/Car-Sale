using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASM1.Service.Dtos;

namespace ASM1.Service.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> Login(string email, string password);
        bool CheckRole(UserDto user, string requiredRole);
        Task<UserDto> GetUserByEmail(string email);
        Task<bool> Register(UserDto user, int? dealerId = null);
        Task<UserDto> GetUserById(int userId);
    }
}