using API;
using GYM_BE.Core.Dto;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GYM_BE.All.GoodsEquipmentFix
{
    [ApiExplorerSettings(GroupName = "017-GOODS-GOODS_EQUIPMENT_FIX")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GoodsEquipmentFixController : Controller
    {
        private readonly FullDbContext _dbContext;
        private readonly IGoodsEquipmentFixRepository _GoodsEquipmentFixRepository;
        private readonly AppSettings _appSettings;

        public GoodsEquipmentFixController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options)
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _GoodsEquipmentFixRepository = new GoodsEquipmentFixRepository(_dbContext);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(PaginationDTO<GoodsEquipmentFixDTO> pagination)
        {
            var response = await _GoodsEquipmentFixRepository.QueryList(pagination);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _GoodsEquipmentFixRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GoodsEquipmentFixDTO model)
        {
            var response = await _GoodsEquipmentFixRepository.Create(model, "root");
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<GoodsEquipmentFixDTO> models)
        {
            var response = await _GoodsEquipmentFixRepository.CreateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(GoodsEquipmentFixDTO model)
        {
            var response = await _GoodsEquipmentFixRepository.Update(model, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<GoodsEquipmentFixDTO> models)
        {
            var response = await _GoodsEquipmentFixRepository.UpdateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(GoodsEquipmentFixDTO model)
        {
            if (model.Id != null)
            {
                var response = await _GoodsEquipmentFixRepository.Delete((long)model.Id);
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
            var response = await _GoodsEquipmentFixRepository.DeleteIds(model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var response = await _GoodsEquipmentFixRepository.ToggleActiveIds(model.Ids, model.ValueToBind, "root");
            return Ok(response);
        }

    }
}
