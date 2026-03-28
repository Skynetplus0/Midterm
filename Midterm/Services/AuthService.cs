using Midterm.DTOs.Auth;
using Midterm.Helpers;
using Midterm.Repositories.Interfaces;
using Midterm.Services.Interfaces;



namespace Midterm.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthService(IUserRepository userRepository, JwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var isPasswordValid = PasswordHasher.Verify(request.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var tokenResult = _jwtTokenGenerator.GenerateToken(user);

            return new LoginResponseDto
            {
                Token = tokenResult.Token,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                ExpiresAtUtc = tokenResult.ExpiresAtUtc
            };
        }
    }
}