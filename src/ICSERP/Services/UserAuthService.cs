using System;
using System.Collections;
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
    public class UserAuthService : IUserAuthService
    {
        private readonly Func<AppDataContext> _dbcontext;
        public UserAuthService(Func<AppDataContext> context)
        {
            _dbcontext = context;
        }
        public async Task<AuthResponse> Authenticate(string Email, string password)
        {
            using (var _context = _dbcontext())
            { 
                try
                {
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == Email);
                    if(user == null) return new AuthResponse("User not Found");
                    string value = VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);
                    if (value != "success") return new AuthResponse(value);
                    return new AuthResponse(user);
                }
                catch(Exception ex)
                {
                    return new AuthResponse($"An error occurred when attempting to login: {ex.Message}");
                }
            }    
        }

        public async  Task<AuthResponse> Register(User user, PersonalInformation profile, Company company, string password)
        {
           using (var _context = _dbcontext())
           { 
                var is_user = await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
                if(is_user != null) return new AuthResponse("User Already Exist");
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try 
                    {
                         _context.Companies.Add(company);
                        await _context.SaveChangesAsync();


                        List<Permission> permission = new List<Permission>();
                        string[] doc_type = {"User","Role", "Unit", "Department", "Branch"};
                        for(int i = 0; i < doc_type.Length; i++)
                        {
                            permission.Add(
                                new Permission {
                                    DocumentType = doc_type[i],
                                    DocumentAccessLevel = 0,
                                    Create = true,
                                    Read = true,
                                    Update = true,
                                    Delete = true,
                                    Upload = true,
                                    Download = true,
                                    Amend = true,
                                    Cancel = true,
                                    Approval = true,
                                    SetPermission = true,
                                    CompanyId = company.CompanyId,
                                    DateCreated = DateTime.Now
                                }
                            );
                        }
                        //_context.Permissions.AddRange(permission);
                        //int records = await _context.SaveChangesAsync();

                        var role = new Role {
                            RoleName = "Owner",
                            Description = "Full Access",
                            CompanyId = company.CompanyId,
                            DateCreated = DateTime.Now
                        };

                        //List<RolePermission> rolepermission = new List<RolePermission>();
                        foreach(Permission perm in permission)
                        {
                            var roleperm = new RolePermission { 
                                Roles = role,
                                Permissions = perm,
                                CompanyId = company.CompanyId,
                                DateCreated = DateTime.Now
                            };
                            _context.RolePermissions.Add(roleperm);
                        };
                        
                        await _context.SaveChangesAsync();

                        user.CompanyId =  company.CompanyId;
                        _context.Users.Add(user);
                        await _context.SaveChangesAsync();

                        //_context.Roles.Add(role);
                        //await _context.SaveChangesAsync();                       

                        var UserRole = new UserRole {
                            Users = user,
                            Roles = role,
                            CompanyId = company.CompanyId,
                            DateCreated = DateTime.Now
                        };

                        _context.UserRoles.Add(UserRole);
                        await _context.SaveChangesAsync();

                        profile.UserId = user.UserId;
                        _context.PersonalInformation.Add(profile);
                        await _context.SaveChangesAsync();
                        

                        transaction.Commit();

                        
                        
                         

                        return new AuthResponse(user);
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        return new AuthResponse(ex.Message);
                    }
                    
                }
           }   
        }

        private static string VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) return "password is null";
            if (string.IsNullOrWhiteSpace(password)) return "Value cannot be empty or whitespace only string.";
            if (storedHash.Length != 64) return "Invalid length of password hash (64 bytes expected).";
            if (storedSalt.Length != 128) return "Invalid length of password salt (128 bytes expected).";

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return "Username and Password not match";
                }
            }

            return "success";
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password is null");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public PermissionModel GetById(int id, string Permissionparam)
        {
           using (var _context = _dbcontext())
           { 
            var perm = Permissionparam.Split(",");
            var doc_type = perm[0];
            var perm_type = perm[1];
            var user = _context.Users.FirstOrDefault(x => x.UserId == id);
            var roles = _context.Permissions
                                .Where(x => x.CompanyId == user.CompanyId)
                                .Where(z => z.DocumentType == doc_type)
                                .Include(b => b.RolePermissions)
                                .ThenInclude(r => r.Roles)
                                .ToList();

            var userroles = _context.Users
                            .Where(x => x.UserId == id)
                            .Include(c => c.UserRoles)
                            .ThenInclude(row => row.Roles)
                            .Select( a => new{
                                a.UserRoles
                            }).ToList();    

            var userSpecialPermission = _context.SpecialPermissions
                                        .Where(x => x.UserId == id
                                         && x.DocumentType == doc_type)
                                        .ToList();                          
            var rolesperm = roles;
            if(perm_type == "Create")
            {
                rolesperm = roles.Where(x => x.Create == true).ToList();
                userSpecialPermission = userSpecialPermission.Where(x => x.Create == true).ToList();
            }
            else if(perm_type == "Read")
            {
                rolesperm = roles.Where(x => x.Read == true).ToList();
                userSpecialPermission = userSpecialPermission.Where(x => x.Read == true).ToList();
            } 
            else if(perm_type == "Update")
            {
                rolesperm = roles.Where(x => x.Update == true).ToList();
                userSpecialPermission = userSpecialPermission.Where(x => x.Update == true).ToList();
            } 
            else if(perm_type == "Delete")
            {
                rolesperm = roles.Where(x => x.Delete == true).ToList();
                userSpecialPermission = userSpecialPermission.Where(x => x.Delete == true).ToList();
            } 
            else if(perm_type == "Download")
            {
                rolesperm = roles.Where(x => x.Download == true).ToList();
                userSpecialPermission = userSpecialPermission.Where(x => x.Download == true).ToList();
            } 
            else if(perm_type == "Upload")
            {
                rolesperm = roles.Where(x => x.Upload == true).ToList();
                userSpecialPermission = userSpecialPermission.Where(x => x.Upload == true).ToList();
            }
            else if(perm_type == "Amend")
            {
                rolesperm = roles.Where(x => x.Amend == true).ToList();
                userSpecialPermission = userSpecialPermission.Where(x => x.Amend == true).ToList();
            }
            else if(perm_type == "Cancel")
            {
                rolesperm = roles.Where(x => x.Cancel == true).ToList();
                userSpecialPermission = userSpecialPermission.Where(x => x.Cancel == true).ToList();
            } 
            else if(perm_type == "Approval")
            {
                rolesperm = roles.Where(x => x.Approval == true).ToList();
                userSpecialPermission = userSpecialPermission.Where(x => x.Approval == true).ToList();
            }
            

            var roleswithaccess = new ArrayList(); 
            var DocumentAccessLevel = new ArrayList();
            var userroleaccess = new ArrayList();
            var accessrole = new ArrayList();
            int accessLevel = 0;

            //Get the roles and document access level for document
            foreach (var role in rolesperm)
            {
                foreach(var  r in role.RolePermissions)
                {
                    roleswithaccess.Add(r.Roles.RoleName);
                    DocumentAccessLevel.Add(role.DocumentAccessLevel);
                }
            }

            //Get all the roles a User belongs
            foreach (var userrole in userroles)
            {
                foreach(var  y in userrole.UserRoles)
                {
                   userroleaccess.Add(y.Roles.RoleName);
                }
            }
            //Check if a user role belongs to the list role that have access
            foreach (string t in userroleaccess)
            {
                if (roleswithaccess.Contains(t))
                {
                   int index = roleswithaccess.IndexOf(t); 
                   accessrole.Add(t);
                   accessLevel = index > accessLevel ? index : accessLevel;
                }
            }
            //If user does not have access, Check if User has special Permission
            if(accessrole.Count == 0)
            {
                accessLevel = userSpecialPermission.Count > 0 ?
                                 userSpecialPermission[0].DocumentAccessLevel :
                                 accessLevel;
            }

            var access = new PermissionModel{
                Status = (accessrole.Count > 0 || userSpecialPermission.Count > 0) ? true : false,
                DocType = doc_type,
                AccessType = perm_type,
                Roles = accessrole,
                CompanyId = (int)user.CompanyId,
                UserId = user.UserId,
                DocumentAccessLevel = accessLevel
            };
            
        
            return access;
           }
        }
        public async Task<User> GetUserId(int id)
        {
            using (var _context = _dbcontext())
            {  
              return await _context.Users.FindAsync(id);
            }
        }
    }
}