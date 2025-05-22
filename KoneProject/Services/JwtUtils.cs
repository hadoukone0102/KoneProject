using KoneProject.DTOs;
using KoneProject.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

    namespace KoneProject.Services;
public class JwtUtils : IJwtUtils
    {
        private readonly IConfiguration _configuration;
        public JwtUtils(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Boolean ValidateJwtToken(string token)
        {
            if (token == null)
                return false;

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:ValidAudience"],
                    ValidIssuer = _configuration["Jwt:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
                };

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:ValidAudience"],
                    ValidIssuer = _configuration["Jwt:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"])),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public UserDto? ValidAndParseJwtToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);
            var jwtValue = jwtToken.Split("Bearer ")[1];
            // Configure token validation parameters
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            // Parse and validate the JWT token
            ClaimsPrincipal claimsPrincipal;
            try
            {
                claimsPrincipal = tokenHandler.ValidateToken(jwtValue, tokenValidationParameters, out _);
                var emailClaim = claimsPrincipal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
                var roleClaim = claimsPrincipal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role")?.Value;
            // Create a new UserDto instance with the extracted information
            UserDto userDto = new UserDto
                {
                    Email = emailClaim,
                    Role = roleClaim,
                };

                return userDto;

            }
            catch (Exception ex)
            {

                return null;
            }

        }



    }
