using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ICSERP.Communication;
using ICSERP.DataContext;
using ICSERP.Entities.UserManagament;
using ICSERP.Models.UserManagement;
using Microsoft.EntityFrameworkCore;

namespace ICSERP.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly Func<AppDataContext> _dbcontext;
        private PermissionModel _permissionModel { get; } 
        public ClaimsPrincipal User { get; }

        public UserRoleService(Func<AppDataContext> context)
        {
            _dbcontext = context;
        }


        public async Task<RoleResponse> Create(Role role, int companyId)
        {
             role.CompanyId = companyId;
             role.DateCreated = DateTime.Now;
             using (var _context = _dbcontext())
             {
                 _context.Roles.Add(role);
                 await _context.SaveChangesAsync();
                 return new RoleResponse(role);
             }
             
            
        }

        public async Task<IEnumerable<Role>> GetAll(int companyId, int accesslevel)
        {
           using (var _context = _dbcontext())
           { 
            var roles = await _context.Roles.Where(x => x.CompanyId == companyId).ToListAsync();
            var filteredroles = Filter(roles,accesslevel);
            return filteredroles;
           }
        }

        public async Task<RoleResponse> GetById(int id)
        {
           using (var _context = _dbcontext())
           { 
            var role = await _context.Roles.FindAsync(id);
            if(role == null)
                return new RoleResponse("Role not Found");
            return new RoleResponse(role);
           }
        }

        

        public async Task<object> Delete(int id)
        {
           using (var _context = _dbcontext())
           { 
                var role = await _context.Roles.FindAsync(id);
                if(role == null)
                {
                        var model = new ReturnModel{
                            Status = false,
                            Message = "Role not Found"
                        };
                        return model;
                }
                

                role.Is_deleted = true;
                _context.Roles.Update(role);
                await _context.SaveChangesAsync();
                var successmodel = new ReturnModel{
                        Status = true,
                        Message = "Role Deleted Successfully"
                };
                return successmodel;
           }    
        }
        public async Task<object> Update(int id,RoleModel model)
        {
           using (var _context = _dbcontext())
           { 
                var role = await _context.Roles.FindAsync(id);
                if(role == null)
                {
                        var updatemodel = new ReturnModel{
                            Status = false,
                            Message = "Role not Found"
                        };
                        return updatemodel;
                }
               

                role.Is_deleted = false;
                role.RoleName = model.RoleName;
                role.Description = model.Description;
                _context.Roles.Update(role);
                await _context.SaveChangesAsync();
                var successmodel = new ReturnModel{
                        Status = true,
                        Message = "Role Updated Successfully",
                        Role = new ReturnRoleModel {
                            RoleId = role.RoleId,
                            RoleName = role.RoleName,
                            Description = role.Description
                        }
                };
                return successmodel;
           }    
        }
        private static IList<Role> Filter(IList<Role> roles, int accesslevel)
        {
            switch(accesslevel) 
            {
            case 0:
                return roles.Where(x => x.CompanyId == 1).ToList();
            case 1:
                return roles.Where(x => x.CompanyId == 1).ToList();
            default:
                return roles.Where(x => x.CompanyId == 1).ToList();
            }
            
        }

        public async Task<GeneralModel> AssignRoleToUser(RoleUserModel model, int companyId)
        {
            using (var _context = _dbcontext())
            {
                var user = await _context.Users.FindAsync(model.UserId);
                var role = await _context.Roles.FindAsync(model.RoleId);
                if(user == null)
                {
                    return  new GeneralModel {
                        Success = false,
                        Message = "User not Found"
                    };
                }
                else if(role == null)
                {
                    return  new GeneralModel {
                        Success = false,
                        Message = "Role not Found"
                    };
                }

                var UserRole = new UserRole {
                            Users = user,
                            Roles = role,
                            CompanyId = companyId,
                            DateCreated = DateTime.Now
                };
                _context.UserRoles.Add(UserRole);
                await _context.SaveChangesAsync();
                
                var result = new GeneralModel {
                    Success = true,
                    Message = "Role Assigned to User"
                };
                return result;
            }
        }
    
        public async Task<GeneralModel> RemoveRoleFromUser(int roleid, int userid)
        {
            using (var _context = _dbcontext())
            {
                var user = await _context.Users.FindAsync(userid);
                var role = await _context.Roles.FindAsync(roleid);
                
                if(user == null)
                {
                    return  new GeneralModel {
                        Success = false,
                        Message = "User not Found"
                    };
                }
                else if(role == null)
                {
                    return  new GeneralModel {
                        Success = false,
                        Message = "User not Found"
                    };
                }
                var userRole = await _context.UserRoles
                              .FirstOrDefaultAsync(x => x.RoleId == userid && x.UserId == roleid);
                
                if(userRole == null)
                {
                   return  new GeneralModel {
                        Success = false,
                        Message = "Role was never assigned to User"
                    }; 
                }
                userRole.Is_deleted = true;
                _context.UserRoles.Update(userRole);
                await _context.SaveChangesAsync();
                return  new GeneralModel {
                        Success = true,
                        Message = "User Deleted from Role"
                    };
            }
        }
    }
}