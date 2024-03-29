﻿using API;
using GYM_BE.All.SysOtherList;
using GYM_BE.All.SysOtherListType;
using GYM_BE.Core.Dto;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GYM_BE.All.System.SysOtherList
{
    [ApiExplorerSettings(GroupName = "014-SYSTEM-SYS_OTHER_LIST")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SysOtherListController : Controller
    {
        private readonly FullDbContext _dbContext;
        private readonly ISysOtherListRepository _SysOtherListRepository;
        private readonly AppSettings _appSettings;

        public SysOtherListController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options)
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _SysOtherListRepository = new SysOtherListRepository(_dbContext);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(PaginationDTO pagination)
        {
            var response = await _SysOtherListRepository.QueryList(pagination);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _SysOtherListRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SysOtherListDTO model)
        {
            var response = await _SysOtherListRepository.Create(model, "root");
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<SysOtherListDTO> models)
        {
            var response = await _SysOtherListRepository.CreateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SysOtherListDTO model)
        {
            var response = await _SysOtherListRepository.Update(model, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<SysOtherListDTO> models)
        {
            var response = await _SysOtherListRepository.UpdateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SysOtherListDTO model)
        {
            if (model.Id != null)
            {
                var response = await _SysOtherListRepository.Delete((long)model.Id);
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
            var response = await _SysOtherListRepository.DeleteIds(model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var response = await _SysOtherListRepository.ToggleActiveIds(model.Ids, model.ValueToBind, "root");
            return Ok(response);
        }
    }
}
