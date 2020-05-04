using System.Collections.Generic;
using System.Threading.Tasks;
using ICSERP.Communication;
using ICSERP.Entities.UserManagament;
using ICSERP.Models.UserManagement;

namespace ICSERP.Services
{
    public interface IBranchService
    {
         Task<BranchResponse> Create(Branch model, int companyId);

         Task<BranchResponse> Update(int id,Branch model);

         Task<GeneralModel> Delete(int id);

         Task<IEnumerable<Branch>> Read(int companyId);

         Task<BranchResponse> GetById(int id);
    }
}