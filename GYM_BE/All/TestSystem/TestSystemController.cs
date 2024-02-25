﻿using API;
using GYM_BE.Core.Dto;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GYM_BE.All.TestSystem
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestSystemController : ControllerBase
    {
        private readonly FullDbContext _dbContext;
        private readonly ITestSystemRepository _TestSystemRepository;
        private readonly AppSettings _appSettings;

        public TestSystemController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options)
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _TestSystemRepository = new TestSystemRepository(_dbContext);
            _appSettings = options.Value;
        }
        [HttpPost]
        public async Task<IActionResult> QueryList(long id)
        {
            var response = await _TestSystemRepository.QueryList(id);
            return Ok(new FormatedResponse() { InnerBody = response });
        }
        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response =await _TestSystemRepository.GetById(id);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> Create(TrCenterDTO model)
        {
            var response = await _TestSystemRepository.Create(model, "root");
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<TrCenterDTO> models)
        {
            var response = await _TestSystemRepository.CreateRange(models, "root");
            return Ok(new FormatedResponse() { InnerBody = response });
        }
        [HttpPost]
        public async Task<IActionResult> Update(TrCenterDTO model)
        {
            var response = await _TestSystemRepository.Update(model, "root");
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<TrCenterDTO> models)
        {
            
            var response = await _TestSystemRepository.UpdateRange(models, "root");
            return Ok(new FormatedResponse() { InnerBody = response });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(TrCenterDTO model)
        {
            if (model.Id != null)
            {
                var response = await _TestSystemRepository.Delete((long)model.Id);
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
            var response = await _TestSystemRepository.DeleteIds(model.Ids);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var response = await _TestSystemRepository.ToggleActiveIds( model.Ids, model.ValueToBind, "root");
            return Ok(response);
        }
    }
}
