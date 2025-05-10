using KoneProject.Datas;
using KoneProject.DTOs;
using KoneProject.Helpers;
using KoneProject.Interfaces;
using KoneProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace KoneProject.Services
{
    public class UserService : UserInterface
    {
        // constructor
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public UserService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ApiRessponse<string>> Register(UserDto userDto)
        {
            // verifions si le mail de l'utilisateur existe déjà  
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (existingUser != null)
            {
                var message = "Email already exists";
                return new ApiRessponse<string>(false, message);
            }
            // Hash the password  
            using var hmcl = new HMACSHA512();
            var newUser = new UserModel
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = Convert.ToBase64String(hmcl.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password))),
                PasswordSalt = hmcl.Key,
                Role = userDto.Role,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            // ajout de l'utilisateur dans la base de données  
            _context.Users.Add(newUser);
            var result = await _context.SaveChangesAsync();
            return new ApiRessponse<string>(
               true,
               "User registered successfully",
               newUser
              );
        }
        public async Task<ApiRessponse<string>> Login(loginDto logDto)
        {
            // verifions si le mail de l'utilisateur existe déjà
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == logDto.Email);
            if (existingUser == null)
            {
                return new ApiRessponse<string>(false, "E-mail introuvable", null);
            }
            // Verifions si le mot de passe est correct
            using var hmac = new HMACSHA512(existingUser.PasswordSalt);
            var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(logDto.Password)));
            if (computedHash != existingUser.Password)
            {
                return new ApiRessponse<string>(false, "E-mail ou Mot de passe Incorrecte");
            }

            // Generate JWT token
            var token = GenerateJwtToken(existingUser);
            return new ApiRessponse<string>(true, "Connexion réussie", token);

        }

        private string GenerateJwtToken(UserModel user)
        {
            var keyString = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(keyString))
            {
                throw new ArgumentNullException("Jwt:Key", "JWT key cannot be null or empty.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Gestion des utilisateurs

        public async Task<ApiRessponse<List<UserModel>>> GetAllUsers()
        {
            var users = _context.Users.ToList();
            if (users == null)
            {
                return await Task.FromResult(new ApiRessponse<List<UserModel>>(false, "Aucun utilisateur trouvé"));
            }
            return await Task.FromResult(new ApiRessponse<List<UserModel>>(true, "Liste des utilisateurs", users));
        }

        public async Task<ApiRessponse<string>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return new ApiRessponse<string>(false, "Utilisateur introuvable");
            }
            return new ApiRessponse<string>(true, "Utilisateur trouvé", user);
        }

    }
}
