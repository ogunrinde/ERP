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
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly IMapper _mapper;

        public PermissionController(IMapper mapper, IPermissionService permissionService)
        {
            _mapper = mapper;
            _permissionService = permissionService;
        }

        [A1AuthorizePermission(Permissions = "Role,Create")]
        [HttpPost("AssignPermissionToRole")]
        public async Task<IActionResult> AssignPermissionToRole([FromBody] RolePermissionModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            int companyId = int.Parse(HttpContext.User.Claims.Where( c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var create = await _permissionService.Create(model, companyId);
            var update = await _permissionService.Update(create._rolepermission,companyId);
            //if(!resul) return BadRequest(new {Success = false, Message = result.Message});
            return Ok(new {Success = true, result = update});
        }

        [A1AuthorizePermission(Permissions = "Role,Update")]
        [HttpPut("UpdateAssignPermissionToRole")]
        public async Task<IActionResult> UpdateAssignPermissionToRole([FromBody] RolePermissionModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            int companyId = int.Parse(HttpContext.User.Claims.Where( c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var update = await _permissionService.Update(model,companyId);
            return Ok(new {Success = true, result = update});
        }

        [A1AuthorizePermission(Permissions = "Role,Delete")]
        [HttpDelete("RemovePermissionFromRole/{permissionId}")]
        public async Task<IActionResult> RemovePermissionFromRole(int permissionId)
        {
            var result = await _permissionService.Delete(permissionId);
            return Ok(new {Success = result});
        }

        [A1AuthorizePermission(Permissions = "Role,Create")]
        [HttpPost("AssignUserBasedPermissionToUser")]
        public async Task<IActionResult> AssignUserBasedPermissionToUser([FromBody] DirectUserPermissionModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            int companyId = int.Parse(HttpContext.User.Claims.Where( c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            int userId = int.Parse(HttpContext.User.Claims.Where(c =>c.Type == ClaimTypes.Name).FirstOrDefault().Value);
            var result = await _permissionService.CreateUserPermission(model, companyId);
            result = await _permissionService.UpdateUserPermission(result._userpermission,companyId);
            return Ok(new {Success = true, result = result});
        }

        [A1AuthorizePermission(Permissions = "Role,Update")]
        [HttpPost("UpdateUserBasedPermissionToUser")]
        public async Task<IActionResult> UpdateUserBasedPermissionToUser([FromBody] DirectUserPermissionModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            int companyId = int.Parse(HttpContext.User.Claims.Where( c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            int userId = int.Parse(HttpContext.User.Claims.Where(c =>c.Type == ClaimTypes.Name).FirstOrDefault().Value);
            var update = await _permissionService.UpdateUserPermission(model,companyId);
            return Ok(new {Success = true, result = update});
        }

        [A1AuthorizePermission(Permissions = "Role,Delete")]
        [HttpDelete("RemoveUserBasedPermissionFromUser/{userId}/{documentType}")]
        public async Task<IActionResult> RemoveUserBasedPermissionFromUser(int userId,string documentType)
        {
            var result = await _permissionService.DeleteUserBasedPermission(userId, documentType);
            if(result == null)
            {
                return BadRequest(new { Success =  false, Message = "User Permission Not Found"});
            }
            return Ok(new {Success = result, Message = "User Permission Deleted"});
        }
    }
}