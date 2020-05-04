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
    public class UnitController : ControllerBase
    {
        private readonly IUnitService _unitService;

        private readonly IMapper _mapper;

        public UnitController(IUnitService unitService, IMapper mapper)
        {
            _unitService = unitService;
            _mapper = mapper;
        }

        [A1AuthorizePermission(Permissions = "Unit,Create")]
        [HttpPost("unitcreate")]
        public async Task<IActionResult> Create([FromBody] UnitModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            var unitmodel = _mapper.Map<Unit>(model);
            int companyId = int.Parse(HttpContext.User.Claims.Where( c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var result = await _unitService.Create(unitmodel,companyId);
            if(!result.Success)
            {
                return BadRequest(new { Success = false, Message = result.Message});
            }
            var umodel = _mapper.Map<UnitModel>(result._unit);
            return Ok(new { Success = true, result = umodel});
        }

        [A1AuthorizePermission(Permissions = "Unit,Update")]
        [HttpPut("unitupdate")]
        public async Task<IActionResult> Update(int id, [FromBody] UnitModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            var unit = _mapper.Map<Unit>(model);
            var result = await _unitService.Update(id,unit);
            if(!result.Success)
            {
                return BadRequest(new { Success = false, Message = result.Message});
            }
            var unitmodel = _mapper.Map<UnitModel>(result._unit);
            return Ok(new {Success = true, result = unitmodel});
        }

        [A1AuthorizePermission(Permissions = "Unit,Read")]
        [HttpGet("unitRead")]
        public async Task<IActionResult> Read()
        {
            int companyId = int.Parse(HttpContext.User.Claims.Where( c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var result = await _unitService.Read(companyId);
            var model = _mapper.Map<IEnumerable<UnitModel>>(result);
            return Ok(new {Success = true, result = model});
        }

        [A1AuthorizePermission(Permissions = "Unit,Delete")]
        [HttpDelete("unitdelete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _unitService.Delete(id);
            if(!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [A1AuthorizePermission(Permissions = "Unit,Read")]
        [HttpGet("getunit/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _unitService.GetById(id);
            if(!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}