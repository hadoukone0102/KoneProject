using KoneProject.DTOs;
using KoneProject.Helpers;
using KoneProject.Models;

namespace KoneProject.Interfaces
{
    public interface UserInterface
    {
        Task<ApiRessponse<string>> Register(UserDto userDto);
        Task<ApiRessponse<string>> Login(loginDto userDto);

        // Les methode pour la gestion des utilisateurs
        Task<ApiRessponse<List<UserModel>>> GetAllUsers();
        Task<ApiRessponse<string>> GetUserById(int id);
    }
}
