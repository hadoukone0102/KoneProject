using KoneProject.DTOs;
using KoneProject.Helpers;

namespace KoneProject.Interfaces
{
    public interface UserInterface
    {
        Task<ApiRessponse<string>> Register(UserDto userDto);
        Task<ApiRessponse<string>> Login(loginDto userDto);
    }
}
