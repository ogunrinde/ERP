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
    public class DepartmentService : IDepartmentService
    {

        private readonly Func<AppDataContext> _dbcontext;

        public DepartmentService(Func<AppDataContext> context)
        {
            _dbcontext = context;
        }
        public async Task<DepartmentResponse> Create(Department model, int companyId)
        {
            using (var _context = _dbcontext())
            {
                var dept = await _context.Departments.FirstOrDefaultAsync(x => x.CompanyId == companyId && x.DepartmentName == model.DepartmentName);
                if(dept != null)
                {
                    return new DepartmentResponse("Department Already Exist");
                }
                model.CompanyId = companyId;
                _context.Departments.Add(model);
                await _context.SaveChangesAsync();
                return new DepartmentResponse(model);

            }
        }

        public async Task<GeneralModel> Delete(int id)
        {
            using (var _context = _dbcontext())
            {
                var dept = await _context.Departments.FindAsync(id);
                if(dept == null)
                {
                    return  new GeneralModel {
                        Success = false,
                        Message = "Department Not found"
                    }; 
                }
                dept.Is_deleted = true;
                _context.Departments.Update(dept);
                await _context.SaveChangesAsync();
                return  new GeneralModel {
                        Success = true,
                        Message = "Department Deleted"
                    };
            }
        }

        public async Task<IEnumerable<Department>> Read(int companyId)
        {
            using (var _context = _dbcontext())
            {
                var dept = await _context.Departments.Where(x => x.Is_deleted == false &&
                 x.CompanyId == companyId).ToListAsync();
                return dept;
            }
        }

        public async Task<DepartmentResponse> Update(int id, Department model)
        {
            using (var _context = _dbcontext())
            {
                var dept = await _context.Departments.FindAsync(id);
                if(dept == null)
                {
                    return new DepartmentResponse("Department Not Found");
                }
                dept.DepartmentName = model.DepartmentName;
                _context.Departments.Update(dept);
                await _context.SaveChangesAsync();
                return new DepartmentResponse(dept);
            }
        }

        public async Task<DepartmentResponse> GetById(int id)
        {
            using (var _context = _dbcontext())
            {
                var dept = await _context.Departments.FirstOrDefaultAsync(x => x.Is_deleted == false && x.DepartmentId == id);
                if(dept == null)
                {
                    return new DepartmentResponse("Department not Found");
                }
                return new DepartmentResponse(dept);
            }
        }
    }
}