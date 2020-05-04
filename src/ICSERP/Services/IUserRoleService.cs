using System.Collections.Generic;
using System.Threading.Tasks;
using ICSERP.Communication;
using ICSERP.Entities.UserManagament;
using ICSERP.Models.UserManagement;

namespace ICSERP.Services
{
    public interface IUserRoleService
    {
         Task<RoleResponse> Create(Role role,int companyId);

         Task<IEnumerable<Role>> GetAll(int companyId, int accesslevel);

         Task<RoleResponse> GetById (int id);
         Task<object> Update (int id, RoleModel role);

         Task<object> Delete (int id);

         Task<GeneralModel> AssignRoleToUser(RoleUserModel model,int companyId);

         Task<GeneralModel> RemoveRoleFromUser(int roleid, int userid);
    }
}