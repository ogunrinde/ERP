using System;
using System.Collections.Generic;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICSERP.Controllers.UserManagement
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private IUserRoleService _userRoleService;
        private IMapper _mapper;

        public RoleController(IUserRoleService userRoleService, IMapper mapper)
        {
            _userRoleService = userRoleService;
            _mapper = mapper;
        }

        [A1AuthorizePermission(Permissions = "Role,Create")]
        [HttpPost("createrole")]
        public async Task<IActionResult> Createrole([FromBody] RoleModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            var role = _mapper.Map<Role>(model);
            int companyId = int.Parse(HttpContext.User.Claims.Where( c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var result = await _userRoleService.Create(role,companyId);
            return Ok(role);
        }

        [A1AuthorizePermission(Permissions = "Role,Read")]
        [HttpGet("roles")]
        public async Task<IActionResult> GetAllRole()
        {
            int accessLevel = (int)HttpContext.Session.GetInt32("documentAccessLevel");
            int companyId = int.Parse(HttpContext.User.Claims.Where( c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var roles =  await _userRoleService.GetAll(companyId,accessLevel);
            var allroles = _mapper.Map<IEnumerable<ReturnRoleModel>>(roles);
            return Ok(allroles);
        }

        [A1AuthorizePermission(Permissions = "Role,Read")]
        [HttpGet("GetbyId/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var status =  await _userRoleService.GetById(id);
                if(!status.Success)
                    return BadRequest(new {Success = false, Message = status.Message});
                
                var role = _mapper.Map<ReturnRoleModel>(status._role);
                return Ok(role);
            }
            catch(Exception ex)
            {
                return BadRequest(new {ex.Message});
            }
            
        }

        [A1AuthorizePermission(Permissions = "Role,Update")]
        [HttpPut("updaterole/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]RoleModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            try
            {
                var status =  await _userRoleService.Update(id,model);
                return Ok(status);
            }
            catch(Exception ex)
            {
                return BadRequest(new {ex.Message});
            }
            
        }

        [A1AuthorizePermission(Permissions = "Role,Delete")]
        [HttpDelete("deleterole/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            
            var status =  await _userRoleService.Delete(id);
            return Ok(status);
        }

        [A1AuthorizePermission(Permissions = "Role,Create")]
        [HttpPost("AssignRoleToUser")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] RoleUserModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            int companyId = int.Parse(HttpContext.User.Claims.Where( c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var result =  await _userRoleService.AssignRoleToUser(model,companyId);
            if(!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [A1AuthorizePermission(Permissions = "Role,Delete")]
        [HttpDelete("RemoveRoleFromUser/{userid}/{roleid}")]
        public async Task<IActionResult> RemoveRoleFromUser(int roleid, int userid)
        {
            
            var result =  await _userRoleService.RemoveRoleFromUser(roleid,userid);
            //if(!result.Success) return BadRequest(result);
            return Ok(result);
        }


    }
}