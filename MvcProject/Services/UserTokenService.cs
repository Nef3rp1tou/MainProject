using MvcProject.Interfaces.IRepositories;
using MvcProject.Interfaces.IServices;
using System.Text;
using System.Text.Json;
using MvcProject.DTOs;
using MvcProject.Enums;
using MvcProject.Utilities;

namespace MvcProject.Services
{
    public class UserTokenService : IUserTokenService
    {
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IConfiguration _configuration;
        private readonly string _baseCasinoApiUrl;


        public UserTokenService(IUserTokenRepository userTokenRepository,  IConfiguration configuration)
        {
            _userTokenRepository = userTokenRepository;
            _configuration = configuration;
            _baseCasinoApiUrl = _configuration["CasinoApi:BaseUrl"] ?? String.Empty;

        }

        public async Task<Guid> GeneratePublicToken(string userId)
        {
                return await _userTokenRepository.GeneratePublicToken(userId);
        }

      
    }
}
