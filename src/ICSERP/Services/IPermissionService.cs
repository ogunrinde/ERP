using System.Threading.Tasks;
using ICSERP.Communication;
using ICSERP.Models.UserManagement;

namespace ICSERP.Services
{
    public interface IPermissionService
    {
         Task<PermissionResponse> Create(RolePermissionModel model, int companyId);
         Task<object> Delete(int PermissionId);

         Task<object> Update(RolePermissionModel model, int companyId);

         Task<UserPermissionResponse> CreateUserPermission(DirectUserPermissionModel model, int companyId);

         Task<UserPermissionResponse> UpdateUserPermission(DirectUserPermissionModel model, int companyId);

         Task<object> DeleteUserBasedPermission(int UserId, string DocumentType);
    }
}