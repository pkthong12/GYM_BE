﻿using API;
using GYM_BE.All.Gym.GymPackage;
using GYM_BE.Core.Dto;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GYM_BE.All.Gym.GymShift
{
    [ApiExplorerSettings(GroupName = "021-GYM-GYM_SHIFT")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GymShiftController : Controller
    {
        private readonly FullDbContext _dbContext;
        private readonly IGymShiftRepository _GymShiftRepository;
        private readonly AppSettings _appSettings;

        public GymShiftController(
            DbContextOptions<FullDbContext> dbOptions,
            IOptions<AppSettings> options)
        {
            _dbContext = new FullDbContext(dbOptions, options);
            _GymShiftRepository = new GymShiftRepository(_dbContext);
            _appSettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> QueryList(PaginationDTO<GymShiftDTO> pagination)
        {
            var response = await _GymShiftRepository.QueryList(pagination);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(long id)
        {
            var response = await _GymShiftRepository.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GymShiftDTO model)
        {
            var response = await _GymShiftRepository.Create(model, "root");
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRange(List<GymShiftDTO> models)
        {
            var response = await _GymShiftRepository.CreateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(GymShiftDTO model)
        {
            var response = await _GymShiftRepository.Update(model, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRange(List<GymShiftDTO> models)
        {
            var response = await _GymShiftRepository.UpdateRange(models, "root");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(GymShiftDTO model)
        {
            if (model.Id != null)
            {
                var response = await _GymShiftRepository.Delete((long)model.Id);
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
            var response = await _GymShiftRepository.DeleteIds(model.Ids);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActiveIds(GenericToggleIsActiveDTO model)
        {
            var response = await _GymShiftRepository.ToggleActiveIds(model.Ids, model.ValueToBind, "root");
            return Ok(response);
        }
    }
}