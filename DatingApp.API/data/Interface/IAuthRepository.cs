using System.Threading.Tasks;
using DatingApp.API.model;

namespace DatingApp.API.data.Interface
{
    public interface IAuthRepository
    {
         Task<User> Register(User user,string Password);

         Task<User> Login(string Username, string Password);

         Task<bool> UserExits(string Username);
    }
}