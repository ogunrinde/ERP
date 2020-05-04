using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ERP.CustomAuthorization;
using ICSERP.Entities.UserManagament;
using ICSERP.Extensions;
using ICSERP.Helpers;
using ICSERP.Models.UserManagement;
using ICSERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ICSERP.Controllers.UserManagement
{

    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {

        private readonly IUserAuthService _userAuthService;
        private IMapper _mapper;

        private readonly AppSettings _appSettings;

        public AuthController(IUserAuthService userAuthService, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _mapper = mapper;
            _userAuthService = userAuthService;
            _appSettings = appSettings.Value;

        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            var user = await _userAuthService.Authenticate(model.Email, model.password);
            if(!user.Success)
              return BadRequest(new {success = false, message = user.Message});


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user._user.UserId.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user._user.CompanyId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token); 
            var userdata = _mapper.Map<UserModel>(user._user);
            HttpContext.Session.SetString("JWToken", tokenString);
            return Ok(new 
                {
                    success = true,
                    user = userdata,
                    token = tokenString
                }
            );
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            var usermodel = new User 
            {
                Email = model.Email.Trim(),
                DateCreated = DateTime.Now,
                UserType = model.UserType.Trim().ToLower() == "system user"   ? "System User" : "Website User"
            };
            var profile = new PersonalInformation 
            {
                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim(),
                Email1 = model.Email.Trim(),
                Nationality = model.Country.Trim(),
                PhoneNumber1 = model.PhoneNumber,
                Gender = model.Gender

            };

            var company = new Company
            {
                CompanyName = model.CompanyName.Trim(),
                CompanyTask = model.CompanyService.Trim(),
                DateCreated = DateTime.Now
            };
            //var user = _mapper.Map<User>(model);
            var result = await _userAuthService.Register(usermodel, profile, company, model.Password);
            if(!result.Success)
                return BadRequest(new {
                    success = false,
                    message = result.Message
                });
            
             
            var newusermodel = _mapper.Map<UserModel>(result._user);
            return Ok(new {
                success = true,
                user = newusermodel
             });    
        }
        [A1AuthorizePermission(Permissions = "Role,Create")]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(new {okay = "4"});
        }
    }
}