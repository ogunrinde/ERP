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
    public class PermissionService : IPermissionService
    {
        private readonly Func<AppDataContext> _dbcontext;
        public PermissionService(Func<AppDataContext> context)
        {
            _dbcontext = context;
        }

        public async Task<PermissionResponse> Create(RolePermissionModel model, int companyId)
        {
            using (var _context = _dbcontext())
            {

                using (var transaction = _context.Database.BeginTransaction())
                {
                    
                    try
                    {
                        var checkroleperm = _dbcontext().Permissions
                                .Where(x => x.CompanyId == companyId)
                                .Include(b => b.RolePermissions)
                                .ThenInclude(r => r.Roles)
                                .ToList();
                        bool RolePermissionExist = false;              
                        bool updatePermission = false;
                        for(int i = 0; i < model.RolePerms.Length; i++)
                        {
                                var Role = await _context.Roles.FindAsync(model.RolePerms[i].RoleId);
                                var filterroleperm =  checkroleperm.Where(x => x.DocumentType == model.RolePerms[i].Permission.DocumentType);
                                foreach (var t in filterroleperm)
                                {
                                    foreach(var r in t.RolePermissions)
                                    {
                                            var result = t.RolePermissions.Any(x => x.PermissionId == r.Roles.RoleId);
                                            RolePermissionExist = result;
                                            updatePermission = result ? true : updatePermission;
                                    
                                        
                                    }   
                                    
                                }
                                if(Role != null && RolePermissionExist == false)
                                {
                                    var roleperm = new RolePermission { 
                                        Roles = Role,
                                        Permissions = new Permission {
                                            DocumentType = model.RolePerms[i].Permission.DocumentType,
                                            DocumentAccessLevel = model.RolePerms[i].Permission.DocumentAccessLevel,
                                            Create = model.RolePerms[i].Permission.Create,
                                            Read = model.RolePerms[i].Permission.Read,
                                            Update = model.RolePerms[i].Permission.Update,
                                            Delete = model.RolePerms[i].Permission.Delete,
                                            Upload = model.RolePerms[i].Permission.Upload,
                                            Download = model.RolePerms[i].Permission.Download,
                                            Amend = model.RolePerms[i].Permission.Amend,
                                            Cancel = model.RolePerms[i].Permission.Cancel,
                                            Approval = model.RolePerms[i].Permission.Approval,
                                            SetPermission = model.RolePerms[i].Permission.SetPermission,
                                            CompanyId = companyId,
                                            DateCreated = DateTime.Now
                                        },
                                        CompanyId = companyId,
                                        DateCreated = DateTime.Now
                                    };
                                    model.RolePerms[i].Success = true;
                                    model.RolePerms[i].Message = "Created";
                                    _context.RolePermissions.Add(roleperm);
                                }else if(Role == null)
                                {
                                    model.RolePerms[i].Success = false;
                                    model.RolePerms[i].Message = "Role Not Found";
                                }else if(RolePermissionExist == true)
                                {
                                    model.RolePerms[i].Success = false;
                                    model.RolePerms[i].Message = "Permission Exist for Role";
                                }
                                
                        }
                        await _context.SaveChangesAsync();

                        transaction.Commit(); 
                        return new PermissionResponse(model);
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        return new PermissionResponse(ex.Message);
                    }
                       
                }
            }

        }

        public async Task<object> Delete(int permissionId)
        {
            using( var _context = _dbcontext())
            {
                var perm = await _context.Permissions.FirstOrDefaultAsync(x =>x.PermissionId == permissionId);
                _context.Permissions.Remove(perm);
                var i = await _context.SaveChangesAsync();
                return i == 1 ? true : false;
            }
            
        }

        public async Task<object> Update(RolePermissionModel model,int companyId)
        {
            
           using (var _context = _dbcontext())
            {
            var checkroleperm = _context.Permissions
                        .Where(x => x.CompanyId == companyId)
                        .Include(b => b.RolePermissions)
                        .ThenInclude(r => r.Roles)
                        .ToList();
            try
            {        

                    for(int i = 0; i < model.RolePerms.Length; i++)
                    {

                            int permissionId = 0;
                            var Role = await _context.Roles.FindAsync(model.RolePerms[i].RoleId);
                            
                            var filterroleperm =  checkroleperm.Where(x => x.DocumentType == model.RolePerms[i].Permission.DocumentType).ToList();   
                            
                            foreach(var t in filterroleperm)
                            {
                                foreach(var b in t.RolePermissions)
                                {
                                    if(t.PermissionId == b.RoleId)
                                    {
                                        permissionId =  t.PermissionId;
                                    }
                                }
                                
                                
                            }
                              
                            

                            if(Role != null && permissionId > 0)
                            {
                                var Permission =  await _context.Permissions.FirstOrDefaultAsync(x => x.PermissionId == permissionId);
                                Permission.PermissionId = permissionId;
                                Permission.DocumentType = model.RolePerms[i].Permission.DocumentType;
                                Permission.DocumentAccessLevel = model.RolePerms[i].Permission.DocumentAccessLevel;
                                Permission.Create = model.RolePerms[i].Permission.Create;
                                Permission.Read = model.RolePerms[i].Permission.Read;
                                Permission.Update = model.RolePerms[i].Permission.Update;
                                Permission.Delete = model.RolePerms[i].Permission.Delete;
                                Permission.Upload = model.RolePerms[i].Permission.Upload;
                                Permission.Download = model.RolePerms[i].Permission.Download;
                                Permission.Amend = model.RolePerms[i].Permission.Amend;
                                Permission.Cancel = model.RolePerms[i].Permission.Cancel;
                                Permission.Approval = model.RolePerms[i].Permission.Approval;
                                Permission.SetPermission = model.RolePerms[i].Permission.SetPermission;
                                _context.Permissions.Update(Permission);
                                model.RolePerms[i].Success = true;
                                model.RolePerms[i].Message = "Updated";
                                //_context.Permissions.Update(Permission);
                            }else if(Role == null)
                            {
                                model.RolePerms[i].Success = false;
                                model.RolePerms[i].Message = "Role Not Found";
                            }else if(permissionId == 0)
                            {
                                model.RolePerms[i].Success = false;
                                model.RolePerms[i].Message = "Permission Not Found";
                            }
                                
                    }
                        await _context.SaveChangesAsync();

                        
                        return new PermissionResponse(model);
                    }
                    catch(Exception ex)
                    {
                        return new PermissionResponse(ex.Message);
                    }
                       
            }
        }

        public async Task<object> DeleteUserBasedPermission(int userId, string DocumentType)
        {
            using (var _context = _dbcontext())
            {
                var userpermission = await _context.SpecialPermissions
                                    .FirstOrDefaultAsync(x=> x.UserId == userId && x.DocumentType == DocumentType);
                    if(userpermission  == null)
                    {
                        return null;
                    }               
                    userpermission.Is_deleted = true;
                    _context.SpecialPermissions.Update(userpermission);
                    await _context.SaveChangesAsync();

                    return true;

            }
        }

        public async Task<UserPermissionResponse> CreateUserPermission(DirectUserPermissionModel model, int companyId)
        {
            using (var _context = _dbcontext())
            {
                try
                {   
                    var userPerm = await _context.SpecialPermissions.Where(x => x.Is_deleted == false && x.CompanyId == companyId).ToListAsync();     

                    for(int i = 0; i < model.UserPerms.Length; i++)
                    {
                        var is_user_perm_exist = userPerm.Any(x => x.UserId == model.UserPerms[i].UserId && x.DocumentType == model.UserPerms[i].Permission.DocumentType);
                        if(is_user_perm_exist == true)
                        {
                            model.UserPerms[i].Success = false;
                            model.UserPerms[i].Message = "User Permission Exist";
                        }
                        else
                        {
                            var perm = new SpecialPermission {
                                UserId = model.UserPerms[i].UserId,
                                DocumentType = model.UserPerms[i].Permission.DocumentType,
                                DocumentAccessLevel= model.UserPerms[i].Permission.DocumentAccessLevel,
                                Create = model.UserPerms[i].Permission.Create,
                                Read = model.UserPerms[i].Permission.Read,
                                Update = model.UserPerms[i].Permission.Update,
                                Delete = model.UserPerms[i].Permission.Delete,
                                Upload = model.UserPerms[i].Permission.Upload,
                                Download = model.UserPerms[i].Permission.Download,
                                Amend = model.UserPerms[i].Permission.Amend,
                                Cancel = model.UserPerms[i].Permission.Cancel,
                                Approval = model.UserPerms[i].Permission.Approval,
                                SetPermission = model.UserPerms[i].Permission.SetPermission,
                                CompanyId = companyId,
                                DateCreated = DateTime.Now
                            };
                            model.UserPerms[i].Success = true;
                            model.UserPerms[i].Message = "Created";
                            _context.SpecialPermissions.Add(perm);
                        }
                    }
                    await _context.SaveChangesAsync();

                    return new UserPermissionResponse(model);
                }
                catch(Exception ex)
                {
                    return new UserPermissionResponse(ex.Message);
                }
            }    
        }


        public async Task<UserPermissionResponse> UpdateUserPermission(DirectUserPermissionModel model, int companyId)
        {
            using (var _context = _dbcontext())
            {
                try
                {   
                    var userPerm = await _context.SpecialPermissions.Where(x => x.Is_deleted == false && x.CompanyId == companyId).ToListAsync();     

                    for(int i = 0; i < model.UserPerms.Length; i++)
                    {
                        var userPermission = userPerm.FirstOrDefault(x => x.UserId == model.UserPerms[i].UserId && x.DocumentType == model.UserPerms[i].Permission.DocumentType);
                        if(userPermission == null)
                        {
                            model.UserPerms[i].Success = false;
                            model.UserPerms[i].Message = "User Permission Not Found";
                        }
                        else
                        {
                                userPermission.UserId = model.UserPerms[i].UserId;
                                userPermission.DocumentType = model.UserPerms[i].Permission.DocumentType;
                                userPermission.DocumentAccessLevel= model.UserPerms[i].Permission.DocumentAccessLevel;
                                userPermission.Create = model.UserPerms[i].Permission.Create;
                                userPermission.Read = model.UserPerms[i].Permission.Read;
                                userPermission.Update = model.UserPerms[i].Permission.Update;
                                userPermission.Delete = model.UserPerms[i].Permission.Delete;
                                userPermission.Upload = model.UserPerms[i].Permission.Upload;
                                userPermission.Download = model.UserPerms[i].Permission.Download;
                                userPermission.Amend = model.UserPerms[i].Permission.Amend;
                                userPermission.Cancel = model.UserPerms[i].Permission.Cancel;
                                userPermission.Approval = model.UserPerms[i].Permission.Approval;
                                userPermission.SetPermission = model.UserPerms[i].Permission.SetPermission;
                                userPermission.CompanyId = companyId;
                                userPermission.DateCreated = DateTime.Now;
                                model.UserPerms[i].Success = true;
                                model.UserPerms[i].Message = "Updated";
                                _context.SpecialPermissions.Update(userPermission);
                        }
                    }
                    await _context.SaveChangesAsync();

                    return new UserPermissionResponse(model);
                }
                catch(Exception ex)
                {
                    return new UserPermissionResponse(ex.Message);
                }
            }    
        }
    }
}