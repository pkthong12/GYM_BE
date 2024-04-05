﻿using API;
using GYM_BE.All.SysUser;
using GYM_BE.Core.Dto;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GYM_BE.All.System.SysUser
{
    [ApiExplorerSettings(GroupName = "017-SYSTEM-SYS_USER")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SysUserController : Controller
    {
        private readonly FullDbContext _dbContext;
        private readonly ISysUserRepository _SysUserRepository;
        private readonly AppSettings _appSettings;

        public SysUserController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options)
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _SysUserRepository = new SysUserRepository(_dbContext);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(PaginationDTO pagination)
        {
            var response = await _SysUserRepository.QueryList(pagination);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _SysUserRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SysUserDTO model)
        {
            var response = await _SysUserRepository.Create(model, "root");
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<SysUserDTO> models)
        {
            var response = await _SysUserRepository.CreateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SysUserDTO model)
        {
            var response = await _SysUserRepository.Update(model, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<SysUserDTO> models)
        {
            var response = await _SysUserRepository.UpdateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SysUserDTO model)
        {
            if (model.Id != null)
            {
                var response = await _SysUserRepository.Delete(model.Id);
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
            var response = await _SysUserRepository.DeleteIds(model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var response = await _SysUserRepository.ToggleActiveIds(model.Ids, model.ValueToBind, "root");
            return Ok(response);
        }
    }
}
