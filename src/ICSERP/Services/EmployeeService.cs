using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ICSERP.Communication;
using ICSERP.DataContext;
using ICSERP.Entities.UserManagament;
using ICSERP.Models.UserManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ICSERP.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly Func<AppDataContext> _dbcontext;

        public ClaimsPrincipal User { get; }

        public EmployeeService(Func<AppDataContext> context)
        {
            _dbcontext = context;
        }
        public async Task<AuthResponse> Create(User user, PersonalInformation profile)
        {
            using (var _context = _dbcontext())
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var exist_user = await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
                        if(exist_user != null)
                        {
                            return new AuthResponse("User Already Exist");
                        }
                        
                        byte[] passwordHash, passwordSalt;
                        CreatePasswordHash(user.Email, out passwordHash, out passwordSalt);
                        user.PasswordHash = passwordHash;
                        user.PasswordSalt = passwordSalt;
                        _context.Users.Add(user);
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

        public async Task<AuthResponse> DeactivateEmployee(int id)
        {
            using (var _context = _dbcontext())
            {
                var user = await _context.Users.FindAsync(id);
                if(user == null) return new AuthResponse("User does not Exist");
                return new AuthResponse(user);
            }
        }


        public async Task<ProfileResponse> EmployeePersonalInformation(int id, PersonalInformation profile)
        {
            using(var _context = _dbcontext())
            {
                var userprofile = await _context.PersonalInformation.FirstOrDefaultAsync(x => x.UserId == id);
                if(userprofile == null)
                {
                    return new ProfileResponse("User does not Exist");
                } 
                userprofile.Title = profile.Title.Trim();
                userprofile.FirstName = profile.FirstName.Trim();
                userprofile.LastName = profile.LastName.Trim();
                userprofile.MiddleName = profile.MiddleName.Trim();
                userprofile.Email1 = profile.Email1.Trim();
                userprofile.Email2 = profile.Email2.Trim();
                userprofile.Gender = profile.Gender.Trim();
                userprofile.MaritalStatus = profile.MaritalStatus.Trim();
                userprofile.StateOfOrgin = profile.StateOfOrgin.Trim();
                userprofile.Nationality = profile.Nationality.Trim();
                userprofile.DateOfBirth = profile.DateOfBirth;
                userprofile.PhoneNumber1 = profile.PhoneNumber1;
                userprofile.PhoneNumber2 = profile.PhoneNumber2;
                userprofile.CurrentAddress = profile.CurrentAddress;
                userprofile.CurrentCity = profile.CurrentCity.Trim();
                userprofile.CurrentState = profile.CurrentState;
                userprofile.CurrentTown = profile.CurrentTown;
                userprofile.PermanantAddress = profile.PermanantAddress;
                userprofile.PermamentCity = profile.PermamentCity.Trim();
                userprofile.PermanentState = profile.PermanentState;
                userprofile.PermamentTown = profile.PermamentTown;
                _context.PersonalInformation.Update(userprofile);  
                await _context.SaveChangesAsync(); 

                return new ProfileResponse(userprofile); 
            }
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
    }
}