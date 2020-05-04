using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ERP.CustomAuthorization;
using ICSERP.Entities.UserManagament;
using ICSERP.Extensions;
using ICSERP.Models.UserManagement;
using ICSERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ICSERP.Controllers.UserManagement
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class EmployeeController: ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        [A1AuthorizePermission(Permissions = "User,Create")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] EmployeeRegisterModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            int companyId = int.Parse(HttpContext.User.Claims.Where( c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            int userId = int.Parse(HttpContext.User.Claims.Where( c => c.Type == ClaimTypes.Name).FirstOrDefault().Value);
            var profile =  new PersonalInformation {
                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim(),
                Email1 = model.Email.Trim(),
                Gender = model.Gender.Trim()
            };
            var usermodel = new User 
            {
                Email = model.Email.Trim(),
                DateCreated = DateTime.Now,
                UserType = "Website User",
                CompanyId = companyId,
                Created_By = userId
            };
            var result = await _employeeService.Create(usermodel, profile);
            if(!result.Success) return BadRequest(new {Success = false, Message = result.Message});
            var user = _mapper.Map<UserModel>(result._user);
            return Ok(new {Success = true, User = user});

        }

        [A1AuthorizePermission(Permissions = "User,Update")]
        [HttpPut("EmployeePersonalInformation/{id}")]
        public async Task<IActionResult> EmployeePersonalInformation(int id,[FromBody] EmployeePersonalInformation profile)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            var info = _mapper.Map<PersonalInformation>(profile);
            var result = await _employeeService.EmployeePersonalInformation(id, info);
            if(!result.Success) return BadRequest(new {Success = false, Message = result.Message});
            return Ok(new {Success = true, Profile = result._profile});
        }


        

        [A1AuthorizePermission(Permissions = "User,Create")]
        [HttpDelete("DeactivateEmployee/{id}")]
        public async Task<IActionResult> DeactivateEmployee(int id)
        {
            var result = await _employeeService.DeactivateEmployee(id);
            if(!result.Success) return BadRequest(new {Success = false, Message = result.Message});

            return Ok(new {Success = true});
        }
    }
}