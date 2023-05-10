using ITSG.Excersise.Application.Dtos;
using ITSG.Excersise.Application.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ITSG.Excersise.Api.Controllers
{
    [Route("api/security")]
    [ApiController]
    public class SecurityController: ControllerBase
    {
        private readonly IMediator _please;
        private readonly IConfiguration _configuration;

        public SecurityController(IMediator mediator, IConfiguration configuration)
        {
            _please = mediator;
            _configuration = configuration;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IResult> Token([FromBody] UserDto request)
        {
            var user = await _please.Send(new GetUserQuery(request.UserName, request.Password));

            if (user == null)
                return Results.Unauthorized();

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            
            return Results.Ok(CreateToken(issuer, audience, key, user));

        }

        private string CreateToken(string issuer, string audience, byte[] key, UserDetailsDto user)
        {
            var claims = user.Roles.Select(r => new Claim(ClaimTypes.Role, r))
                .ToList();

            claims.Add(new Claim("Id", user.UserId.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Login));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Login));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, user.UserId.ToString()));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(55),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
