using API;
using GYM_BE.Core.Dto;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GYM_BE.All.TrCenter
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TrCenterController : Controller
    {
        private readonly FullDbContext _dbContext;
        private readonly ITrCenterRepository _TrCenterRepository;
        private readonly AppSettings _appSettings;

        public TrCenterController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options)
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _TrCenterRepository = new TrCenterRepository(_dbContext);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(long id)
        {
            var response = await _TrCenterRepository.QueryList(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _TrCenterRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TrCenterDTO model)
        {
            var response = await _TrCenterRepository.Create(model, "root");
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<TrCenterDTO> models)
        {
            var response = await _TrCenterRepository.CreateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(TrCenterDTO model)
        {
            var response = await _TrCenterRepository.Update(model, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<TrCenterDTO> models)
        {
            var response = await _TrCenterRepository.UpdateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(TrCenterDTO model)
        {
            if (model.Id != null)
            {
                var response = await _TrCenterRepository.Delete((long)model.Id);
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
            var response = await _TrCenterRepository.DeleteIds(model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var response = await _TrCenterRepository.ToggleActiveIds(model.Ids, model.ValueToBind, "root");
            return Ok(response);
        }

    }
}

