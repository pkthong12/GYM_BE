using API;
using GYM_BE.All.System.Common.Middleware;
using GYM_BE.Core.Dto;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GYM_BE.All.PerEmployee
{
    [ApiExplorerSettings(GroupName = "003-PERSONAL-PER_EMPLOYEE")]
    [ApiController]
    [GymAuthorize]
    [Route("api/[controller]/[action]")]
    public class PerEmployeeController : Controller
    {
        private readonly FullDbContext _dbContext;
        private readonly IPerEmployeeRepository _PerEmployeeRepository;
        private readonly AppSettings _appSettings;

        public PerEmployeeController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options)
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _PerEmployeeRepository = new PerEmployeeRepository(_dbContext);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(PaginationDTO<PerEmployeeDTO> pagination)
        {
            var response = await _PerEmployeeRepository.QueryList(pagination);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _PerEmployeeRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PerEmployeeDTO model)
        {
            var response = await _PerEmployeeRepository.Create(model, "root");
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<PerEmployeeDTO> models)
        {
            var response = await _PerEmployeeRepository.CreateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PerEmployeeDTO model)
        {
            var response = await _PerEmployeeRepository.Update(model, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<PerEmployeeDTO> models)
        {
            var response = await _PerEmployeeRepository.UpdateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PerEmployeeDTO model)
        {
            if (model.Id != null)
            {
                var response = await _PerEmployeeRepository.Delete((long)model.Id);
                return Ok(response);
            }
            else
            {
                return Ok(new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "DELETE_REQUEST_NULL_ID" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIds(IdsRequest model)
        {
            var response = await _PerEmployeeRepository.DeleteIds(model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var response = await _PerEmployeeRepository.ToggleActiveIds(model.Ids, model.ValueToBind, "root");
            return Ok(response);
        }
    }
}

