using System.Collections.Generic;
using System.Threading.Tasks;
using ICSERP.Communication;
using ICSERP.Entities.UserManagament;
using ICSERP.Models.UserManagement;

namespace ICSERP.Services
{
    public interface IDepartmentService
    {
         Task<DepartmentResponse> Create(Department model, int companyId);

         Task<DepartmentResponse> Update(int id,Department model);

         Task<GeneralModel> Delete(int id);

         Task<IEnumerable<Department>> Read(int companyId);

         Task<DepartmentResponse> GetById(int id);
    }
}