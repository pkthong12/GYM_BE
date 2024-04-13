using API;
using GYM_BE.Core.Dto;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GYM_BE.All.CardCheckIn
{
    [ApiExplorerSettings(GroupName = "002-CARD-CARD_CHECK_IN")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CardCheckInController : Controller
    {
        private readonly FullDbContext _dbContext;
        private readonly ICardCheckInRepository _CardCheckInRepository;
        private readonly AppSettings _appSettings;

        public CardCheckInController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options)
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _CardCheckInRepository = new CardCheckInRepository(_dbContext);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(PaginationDTO<CardCheckInDTO> pagination)
        {
            var response = await _CardCheckInRepository.QueryList(pagination);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _CardCheckInRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CardCheckInDTO model)
        {
            var response = await _CardCheckInRepository.Create(model, "root");
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<CardCheckInDTO> models)
        {
            var response = await _CardCheckInRepository.CreateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CardCheckInDTO model)
        {
            var response = await _CardCheckInRepository.Update(model, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<CardCheckInDTO> models)
        {
            var response = await _CardCheckInRepository.UpdateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CardCheckInDTO model)
        {
            if (model.Id != null)
            {
                var response = await _CardCheckInRepository.Delete((long)model.Id);
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
            var response = await _CardCheckInRepository.DeleteIds(model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var response = await _CardCheckInRepository.ToggleActiveIds(model.Ids, model.ValueToBind, "root");
            return Ok(response);
        }

    }
}

