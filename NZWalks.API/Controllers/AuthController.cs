using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Dto;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };

            var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (!identityResult.Succeeded) return BadRequest(identityResult.Errors.Select(e => e.Description));

            if (registerRequestDto.Roles == null || !registerRequestDto.Roles.Any())
                return BadRequest("Roles of User is Empty");

            identityResult = await _userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

            if (!identityResult.Succeeded)
                return BadRequest(identityResult.Errors.Select(e => e.Description));

            return Ok(new { Message = "User has been registered", UserId = identityUser.Id });


        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.Username);

            if(user == null) return BadRequest("Username or password is incorrect");

            var passwordValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (!passwordValid) return BadRequest("Username or password is incorrect");

            var roles = await _userManager.GetRolesAsync(user);

            var jwtToken = _tokenRepository.CreateJwtToken(user, roles.ToList());

            var response = new LoginResponseDto
            {
                JwtToken = jwtToken
            };

            return Ok(response);


        }
    }
}
