using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ICSERP.Communication;
using ICSERP.DataContext;
using ICSERP.Entities.UserManagament;
using ICSERP.Models.UserManagement;
using Microsoft.EntityFrameworkCore;

namespace ICSERP.Services
{
    public class UnitService : IUnitService
    {
        private readonly Func<AppDataContext> _dbcontext;
        public UnitService(Func<AppDataContext> context)
        {
            _dbcontext = context;
        }
        public async Task<UnitResponse> Create(Unit model, int companyId)
        {
            using (var _context = _dbcontext())
            {
                var department = await _context.Departments.FindAsync(model.DepartmentId);
                if(department == null)
                {
                    return new UnitResponse("Department Not Found");
                }
                var unit = await _context.Units.FirstOrDefaultAsync(x => x.CompanyId == companyId && x.UnitName == model.UnitName);

                if(unit != null)
                {
                    return new UnitResponse("Unit Already Exist");
                }
                model.CompanyId = companyId;
                _context.Units.Add(model);
                await _context.SaveChangesAsync();
                return new UnitResponse(model);
            }
        }

        public async Task<GeneralModel> Delete(int id)
        {
            using (var _context = _dbcontext())
            {
                var unit = await _context.Units.FindAsync(id);
                if(unit == null)
                {
                    return  new GeneralModel {
                        Success = false,
                        Message = "Unit Not found"
                    }; 
                }
                unit.Is_deleted = true;
                _context.Units.Update(unit);
                await _context.SaveChangesAsync();
                return  new GeneralModel {
                        Success = true,
                        Message = "Unit Deleted"
                    };
            }
        }

        public async Task<IEnumerable<Unit>> Read(int companyId)
        {
            using (var _context = _dbcontext())
            {
                var units = await _context.Units.Where(x => x.CompanyId == companyId).ToListAsync();
                return units;
            }
        }

        public async Task<UnitResponse> Update(int id, Unit model)
        {
            using (var _context = _dbcontext())
            {
                var unit = await _context.Units.FindAsync(id);
                if(unit == null)
                {
                    return new UnitResponse("Unit Not Found");
                }
                unit.UnitName = model.UnitName;
                _context.Units.Update(unit);
                await _context.SaveChangesAsync();
                return new UnitResponse(unit);
            }
        }
        public async Task<UnitResponse> GetById(int id)
        {
            using (var _context = _dbcontext())
            {
                var unit = await _context.Units.FirstOrDefaultAsync(x => x.Is_deleted == false && x.UnitId == id);
                if(unit == null)
                {
                    return new UnitResponse("Unit not Found");
                }
                return new UnitResponse(unit);
            }
        }
    }
}