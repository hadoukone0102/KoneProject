using KoneProject.DTOs;

namespace KoneProject.Interfaces

{
    public interface IJwtUtils
    {
        // public string GenerateJwtToken(User user);
        public Boolean ValidateJwtToken(string token);
        public UserDto? ValidAndParseJwtToken(string jwtToken);
    }
}
