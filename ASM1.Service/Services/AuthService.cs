using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using ASM1.Service.Services.Interfaces;
using ASM1.Service.Dtos;
using ASM1.Service.Mappers;

namespace ASM1.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;
        private readonly IMapper _mapper;

        public AuthService(IAuthRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public bool CheckRole(UserDto userDto, string requiredRole)
        {
            var user = _mapper.Map<UserDto, User>(userDto);
            return _repository.CheckRole(user, requiredRole);
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            var user = await _repository.GetUserByEmail(email);
            return _mapper.Map<User, UserDto>(user);
        }

        public async Task<UserDto> Login(string email, string password)
        {
            var user = await _repository.Login(email, password);
            return _mapper.Map<User, UserDto>(user);
        }

        public async Task<bool> Register(UserDto userDto, int? dealerId = null)
        {
            var user = _mapper.Map<UserDto, User>(userDto);
            return await _repository.Register(user, dealerId);
        }

        public async Task<UserDto> GetUserById(int userId)
        {
            var user = await _repository.GetUserById(userId);
            return _mapper.Map<User, UserDto>(user);
        }
    }
}
