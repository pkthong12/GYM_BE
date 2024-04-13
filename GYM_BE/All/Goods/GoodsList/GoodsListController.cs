using API;
using GYM_BE.Core.Dto;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GYM_BE.All.GoodsList
{
    [ApiExplorerSettings(GroupName = "018-GOODS-GOODS_LIST")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GoodsListController : Controller
    {
        private readonly FullDbContext _dbContext;
        private readonly IGoodsListRepository _GoodsListRepository;
        private readonly AppSettings _appSettings;

        public GoodsListController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options)
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _GoodsListRepository = new GoodsListRepository(_dbContext);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(PaginationDTO<GoodsListDTO> pagination)
        {
            var response = await _GoodsListRepository.QueryList(pagination);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _GoodsListRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GoodsListDTO model)
        {
            var response = await _GoodsListRepository.Create(model, "root");
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<GoodsListDTO> models)
        {
            var response = await _GoodsListRepository.CreateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(GoodsListDTO model)
        {
            var response = await _GoodsListRepository.Update(model, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<GoodsListDTO> models)
        {
            var response = await _GoodsListRepository.UpdateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(GoodsListDTO model)
        {
            if (model.Id != null)
            {
                var response = await _GoodsListRepository.Delete((long)model.Id);
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
            var response = await _GoodsListRepository.DeleteIds(model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var response = await _GoodsListRepository.ToggleActiveIds(model.Ids, model.ValueToBind, "root");
            return Ok(response);
        }

    }
}
