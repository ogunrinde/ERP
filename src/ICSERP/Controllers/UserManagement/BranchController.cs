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
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;

        private readonly IMapper _mapper;

        public BranchController(IBranchService branchService, IMapper mapper)
        {
            _branchService = branchService;
            _mapper = mapper;
        }

        [A1AuthorizePermission(Permissions = "Branch,Create")]
        [HttpPost("branchcreate")]
        public async Task<IActionResult> Create([FromBody] BranchModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            var branch = _mapper.Map<Branch>(model);
            int companyId = int.Parse(HttpContext.User.Claims.Where( c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var result = await _branchService.Create(branch, companyId);
            if(!result.Success)
            {
                return BadRequest(new { Success = false, Message = result.Message});
            }
            var branchmodel = _mapper.Map<BranchModel>(result._branch);
            return Ok(new { Success = true, result = branchmodel});
        }

        [A1AuthorizePermission(Permissions = "Branch,Update")]
        [HttpPut("branchupdate/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BranchModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            var branch = _mapper.Map<Branch>(model);
            var result = await _branchService.Update(id,branch);
            if(!result.Success)
            {
                return BadRequest(new { Success = false, Message = result.Message});
            }
            var branchmodel = _mapper.Map<BranchModel>(result._branch);
            return Ok(new {Success = true, result = branchmodel});
        }

        [A1AuthorizePermission(Permissions = "Branch,Read")]
        [HttpGet("branchRead")]
        public async Task<IActionResult> Read()
        {
            int companyId = int.Parse(HttpContext.User.Claims.Where( c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var result = await _branchService.Read(companyId);
            var branchmodel = _mapper.Map<IEnumerable<Branch>>(result);
            return Ok(new {Success = true, result = branchmodel});
        }

        [A1AuthorizePermission(Permissions = "Branch,Delete")]
        [HttpDelete("branchdelete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _branchService.Delete(id);
            if(!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [A1AuthorizePermission(Permissions = "Branch,Read")]
        [HttpGet("getbranch/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _branchService.GetById(id);
            if(!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}