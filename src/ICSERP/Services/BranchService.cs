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
    public class BranchService : IBranchService
    {

        private readonly Func<AppDataContext> _dbcontext;

        public BranchService(Func<AppDataContext> context)
        {
            _dbcontext = context;
        }
        public async Task<BranchResponse> Create(Branch model, int companyId)
        {
            using (var _context = _dbcontext())
            {
                var branch = await _context.Branches.FirstOrDefaultAsync(x => x.CompanyId == companyId && x.BranchName == model.BranchName);

                if(branch != null)
                {
                    return new BranchResponse("Branch Already Exist");
                }
                model.CompanyId = companyId;
                _context.Branches.Add(model);
                await _context.SaveChangesAsync();
                return new BranchResponse(model);
            }
        }

        public async Task<GeneralModel> Delete(int id)
        {
            using (var _context = _dbcontext())
            {
                var branch = await _context.Branches.FindAsync(id);
                if(branch == null)
                {
                    return  new GeneralModel {
                        Success = true,
                        Message = "User Deleted from Role"
                    }; 
                }
                branch.Is_deleted = true;
                _context.Branches.Update(branch);
                await _context.SaveChangesAsync();
                return  new GeneralModel {
                        Success = true,
                        Message = "Branch Deleted"
                    };
            }
        }

        public async Task<BranchResponse> GetById(int id)
        {
            using (var _context = _dbcontext())
            {
                var branch = await _context.Branches.FirstOrDefaultAsync(x => x.Is_deleted == false && x.BranchId == id);
                if(branch == null)
                {
                    return new BranchResponse("Branch not Found");
                }
                return new BranchResponse(branch);
            }
        }

        public async Task<IEnumerable<Branch>> Read(int companyId)
        {
            using (var _context = _dbcontext())
            {
                var branches = await _context.Branches.Where(x => x.Is_deleted == false && x.CompanyId == companyId).ToListAsync();
                return branches;
            }
        }

        public async Task<BranchResponse> Update(int id, Branch model)
        {
            using (var _context = _dbcontext())
            {
                var branch = await _context.Branches.FindAsync(id);
                if(branch == null)
                {
                    return new BranchResponse("Branch Not Found");
                }
                branch.BranchName = model.BranchName;
                _context.Branches.Update(branch);
                await _context.SaveChangesAsync();
                return new BranchResponse(branch);
            }
        }
    }
}