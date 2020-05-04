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
using Microsoft.AspNetCore.Mvc;

namespace ICSERP.Controllers.UserManagement
{
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _deptService;

        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentService deptService, IMapper mapper)
        {
            _deptService = deptService;
            _mapper = mapper;
        }

        [A1AuthorizePermission(Permissions = "Department,Create")]
        [HttpPost("departmentcreate")]
        public async Task<IActionResult> Create([FromBody] DepartmentModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            var deptmodel = _mapper.Map<Department>(model);
            int companyId = int.Parse(HttpContext.User.Claims.Where( c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var result = await _deptService.Create(deptmodel,companyId);
            if(!result.Success)
            {
                return BadRequest(new { Success = false, Message = result.Message});
            }
            var departmentmodel = _mapper.Map<DepartmentModel>(result._dept);
            return Ok(new { Success = true, result = departmentmodel});
        }

        [A1AuthorizePermission(Permissions = "Department,Update")]
        [HttpPut("departmentupdate/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DepartmentModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            var deptmodel = _mapper.Map<Department>(model);
            var result = await _deptService.Update(id,deptmodel);
            if(!result.Success)
            {
                return BadRequest(new { Success = false, Message = result.Message});
            }
            var departmentmodel = _mapper.Map<DepartmentModel>(result._dept);
            return Ok(new { Success = true, result = departmentmodel});
        }

        [A1AuthorizePermission(Permissions = "Department,Read")]
        [HttpGet("departmentRead")]
        public async Task<IActionResult> Read()
        {
            int companyId = int.Parse(HttpContext.User.Claims.Where( c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var result = await _deptService.Read(companyId);
            var branchmodel = _mapper.Map<IEnumerable<Department>>(result);
            return Ok(new {Success = true, result = branchmodel});
        }

        [A1AuthorizePermission(Permissions = "Department,Delete")]
        [HttpDelete("departmentdelete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _deptService.Delete(id);
            if(!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [A1AuthorizePermission(Permissions = "Department,Read")]
        [HttpGet("getdepartment/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _deptService.GetById(id);
            if(!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}