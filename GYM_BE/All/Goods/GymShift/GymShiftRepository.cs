﻿using GYM_BE.All.Gym.GymPackage;
using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using GYM_BE.ENTITIES;
using Microsoft.EntityFrameworkCore;

namespace GYM_BE.All.Gym.GymShift
{
    public class GymShiftRepository : IGymShiftRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<GOODS_SHIFT, GoodsShiftDTO> _genericRepository;

        public GymShiftRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<GOODS_SHIFT, GoodsShiftDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<GoodsShiftDTO> pagination)
        {
            var joined = from p in _dbContext.GymShifts.AsNoTracking()
                         select new GoodsShiftDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Name = p.NAME,
                             TotalDays = p.TOTAL_DAYS,
                             HoursStart = p.HOURS_START,
                             HoursStartString = p.HOURS_START! == null ? "" : p.HOURS_START!.Value.ToString("HH:mm"),
                             HoursEnd = p.HOURS_END,
                             HoursEndString = p.HOURS_END! == null ? "" : p.HOURS_END!.Value.ToString("HH:mm"),
                             Note = p.NOTE,
                             IsActive = p.IS_ACTIVE,
                             Status = p.IS_ACTIVE!.Value ? "Áp dụng" : "Ngừng áp dụng"
                         };
            var respose = await _genericRepository.PagingQueryList(joined, pagination);
            return new FormatedResponse
            {
                InnerBody = respose,
            };
        }

        public async Task<FormatedResponse> GetById(long id)
        {
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<GOODS_SHIFT>
                    {
                        (GOODS_SHIFT)response
                    };
                var joined = (from l in list
                              select new GoodsShiftDTO
                              {
                                  Id = l.ID,
                                  Code = l.CODE,
                                  Name = l.NAME,
                                  TotalDays = l.TOTAL_DAYS,
                                  HoursStart = l.HOURS_START,
                                  HoursStartString = l.HOURS_START! == null ? "" : l.HOURS_START!.Value.ToString("HH:mm"),
                                  HoursEnd = l.HOURS_END,
                                  HoursEndString = l.HOURS_END! == null ? "" : l.HOURS_END!.Value.ToString("HH:mm"),
                                  Note = l.NOTE,
                                  IsActive = l.IS_ACTIVE,
                                  Status = l.IS_ACTIVE!.Value ? "Áp dụng" : "Ngừng áp dụng"
                              }).FirstOrDefault();

                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = "ENTITY_NOT_FOUND", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> Create(GoodsShiftDTO dto, string sid)
        {
            dto.IsActive = true;
            var response = await _genericRepository.Create(dto, "root");
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<GoodsShiftDTO> dtos, string sid)
        {
            var add = new List<GoodsShiftDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, "root");
            return response;
        }

        public async Task<FormatedResponse> Update(GoodsShiftDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, "root", patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<GoodsShiftDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange(dtos, "root", patchMode);
            return response;
        }

        public async Task<FormatedResponse> Delete(long id)
        {
            var response = await _genericRepository.Delete(id);
            return response;
        }

        public async Task<FormatedResponse> Delete(string id)
        {
            var response = await _genericRepository.Delete(id);
            return response;
        }

        public async Task<FormatedResponse> DeleteIds(List<long> ids)
        {
            var response = await _genericRepository.DeleteIds(ids);
            return response;
        }

        public async Task<FormatedResponse> ToggleActiveIds(List<long> ids, bool valueToBind, string sid)
        {
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> GetList()
        {
            var res = await (from p in _dbContext.GymShifts.AsNoTracking()
                             where p.IS_ACTIVE == true 
                             select new GoodsShiftDTO
                             {
                                 Id = p.ID,
                                 Code = p.CODE,
                                 Name = p.NAME,
                             }).ToListAsync();
            return new FormatedResponse() { InnerBody = res };
        }
    }
}
