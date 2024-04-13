using API;
using GYM_BE.Core.Dto;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GYM_BE.All.CardInfo
{
    [ApiExplorerSettings(GroupName = "004-CARD-CARD_INFO")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CardInfoController : Controller
    {
        private readonly FullDbContext _dbContext;
        private readonly ICardInfoRepository _CardInfoRepository;
        private readonly AppSettings _appSettings;

        public CardInfoController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options)
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _CardInfoRepository = new CardInfoRepository(_dbContext);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(PaginationDTO<CardInfoDTO> pagination)
        {
            var response = await _CardInfoRepository.QueryList(pagination);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _CardInfoRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CardInfoDTO model)
        {
            var response = await _CardInfoRepository.Create(model, "root");
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<CardInfoDTO> models)
        {
            var response = await _CardInfoRepository.CreateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CardInfoDTO model)
        {
            var response = await _CardInfoRepository.Update(model, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<CardInfoDTO> models)
        {
            var response = await _CardInfoRepository.UpdateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CardInfoDTO model)
        {
            if (model.Id != null)
            {
                var response = await _CardInfoRepository.Delete((long)model.Id);
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
            var response = await _CardInfoRepository.DeleteIds(model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var response = await _CardInfoRepository.ToggleActiveIds(model.Ids, model.ValueToBind, "root");
            return Ok(response);
        } 
        [HttpGet]
        public async Task<IActionResult> GetListCustomer()
        {
            var response = await _CardInfoRepository.GetListCustomer();
            return Ok(response);
        }

    }
}

