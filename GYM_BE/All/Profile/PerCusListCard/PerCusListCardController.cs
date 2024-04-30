using API;
using GYM_BE.Core.Dto;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GYM_BE.All.PerCusListCard
{
    [ApiExplorerSettings(GroupName = "035-PERSONAL-PER_CUS_LIST_CARD")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PerCusListCardController : Controller
    {
        private readonly FullDbContext _dbContext;
        private readonly IPerCusListCardRepository _PerCusListCardRepository;
        private readonly AppSettings _appSettings;

        public PerCusListCardController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options)
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _PerCusListCardRepository = new PerCusListCardRepository(_dbContext);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(PaginationDTO<PerCusListCardDTO> pagination)
        {
            var response = await _PerCusListCardRepository.QueryList(pagination);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _PerCusListCardRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PerCusListCardDTO model)
        {
            var response = await _PerCusListCardRepository.Create(model, "root");
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<PerCusListCardDTO> models)
        {
            var response = await _PerCusListCardRepository.CreateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PerCusListCardDTO model)
        {
            var response = await _PerCusListCardRepository.Update(model, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<PerCusListCardDTO> models)
        {
            var response = await _PerCusListCardRepository.UpdateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PerCusListCardDTO model)
        {
            if (model.Id != null)
            {
                var response = await _PerCusListCardRepository.Delete((long)model.Id);
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
            var response = await _PerCusListCardRepository.DeleteIds(model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var response = await _PerCusListCardRepository.ToggleActiveIds(model.Ids, model.ValueToBind, "root");
            return Ok(response);
        }

    }
}

