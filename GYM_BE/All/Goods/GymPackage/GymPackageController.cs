using API;
using GYM_BE.All.System.Common.Middleware;
using GYM_BE.All.System.SysUser;
using GYM_BE.All.SysUser;
using GYM_BE.Core.Dto;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GYM_BE.All.Gym.GymPackage
{
    [ApiExplorerSettings(GroupName = "034-GYM-GYM_PACKAGE")]
    [ApiController]
    [GymAuthorize]
    [Route("api/[controller]/[action]")]
    public class GymPackageController : Controller
    {
        private readonly FullDbContext _dbContext;
        private readonly IGymPackageRepository _GymPackageRepository;
        private readonly AppSettings _appSettings;

        public GymPackageController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options)
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _GymPackageRepository = new GymPackageRepository(_dbContext);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(PaginationDTO<GoodsPackageDTO> pagination)
        {
            var response = await _GymPackageRepository.QueryList(pagination);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _GymPackageRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GoodsPackageDTO model)
        {
            var response = await _GymPackageRepository.Create(model, "root");
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<GoodsPackageDTO> models)
        {
            var response = await _GymPackageRepository.CreateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(GoodsPackageDTO model)
        {
            var response = await _GymPackageRepository.Update(model, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<GoodsPackageDTO> models)
        {
            var response = await _GymPackageRepository.UpdateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(GoodsPackageDTO model)
        {
            if (model.Id != null)
            {
                var response = await _GymPackageRepository.Delete((long)model.Id);
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
            var response = await _GymPackageRepository.DeleteIds(model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var response = await _GymPackageRepository.ToggleActiveIds(model.Ids, model.ValueToBind, "root");
            return Ok(response);
        }
    }
}
