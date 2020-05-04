using System.Threading.Tasks;
using ICSERP.Communication;
using ICSERP.Entities.UserManagament;
using ICSERP.Models.UserManagement;

namespace ICSERP.Services
{
    public interface IEmployeeService
    {
         Task<AuthResponse> Create(User user, PersonalInformation profile);
         Task<ProfileResponse> EmployeePersonalInformation(int id, PersonalInformation model);

         //Task<object> CompanyEmployeeInformation(CompanyEmployeeInformation model);

    
         //Task<UserModel> UpdateCompanyEmployeeInformation(int id, CompanyEmployeeInformation model); 

         Task<AuthResponse> DeactivateEmployee(int id); 
    }
}