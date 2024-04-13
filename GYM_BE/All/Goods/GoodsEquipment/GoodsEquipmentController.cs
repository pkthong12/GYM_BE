using API;
using GYM_BE.Core.Dto;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GYM_BE.All.GoodsEquipment
{
    [ApiExplorerSettings(GroupName = "016-GOODS-GOODS_EQUIPMENT")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GoodsEquipmentController : Controller
    {
        private readonly FullDbContext _dbContext;
        private readonly IGoodsEquipmentRepository _GoodsEquipmentRepository;
        private readonly AppSettings _appSettings;

        public GoodsEquipmentController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options)
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _GoodsEquipmentRepository = new GoodsEquipmentRepository(_dbContext);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(PaginationDTO<GoodsEquipmentDTO> pagination)
        {
            var response = await _GoodsEquipmentRepository.QueryList(pagination);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _GoodsEquipmentRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GoodsEquipmentDTO model)
        {
            var response = await _GoodsEquipmentRepository.Create(model, "root");
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<GoodsEquipmentDTO> models)
        {
            var response = await _GoodsEquipmentRepository.CreateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(GoodsEquipmentDTO model)
        {
            var response = await _GoodsEquipmentRepository.Update(model, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<GoodsEquipmentDTO> models)
        {
            var response = await _GoodsEquipmentRepository.UpdateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(GoodsEquipmentDTO model)
        {
            if (model.Id != null)
            {
                var response = await _GoodsEquipmentRepository.Delete((long)model.Id);
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
            var response = await _GoodsEquipmentRepository.DeleteIds(model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var response = await _GoodsEquipmentRepository.ToggleActiveIds(model.Ids, model.ValueToBind, "root");
            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetListByTypeCode(string typeCode)
        {
            var response = await _GoodsEquipmentRepository.GetListByCode(typeCode);
            return Ok(response);
        }
    }
}

