using System.Threading.Tasks;
using ICSERP.Communication;
using ICSERP.Entities.UserManagament;
using ICSERP.Models.UserManagement;

namespace ICSERP.Services
{
    public interface IUserAuthService
    {
         Task<AuthResponse> Authenticate(string username, string password);
         Task<AuthResponse> Register(User model, PersonalInformation profile, Company company, string password);

         PermissionModel GetById(int id, string type);

         Task<User> GetUserId(int id);
    }
}