using System.Collections.Generic;
using System.Threading.Tasks;
using ICSERP.Communication;
using ICSERP.Entities.UserManagament;
using ICSERP.Models.UserManagement;

namespace ICSERP.Services
{
    public interface IUnitService
    {
        Task<UnitResponse> Create(Unit model, int companyId);

        Task<UnitResponse> Update(int id,Unit model);

        Task<GeneralModel> Delete(int id);

        Task<IEnumerable<Unit>> Read(int companyId);

        Task<UnitResponse> GetById(int id);
    }
}