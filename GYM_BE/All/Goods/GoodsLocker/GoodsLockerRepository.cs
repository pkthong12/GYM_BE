using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.EntityFrameworkCore;

namespace GYM_BE.All.GoodsLocker
{
    public class GoodsLockerRepository : IGoodsLockerRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<GOODS_LOCKER, GoodsLockerDTO> _genericRepository;

        public GoodsLockerRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<GOODS_LOCKER, GoodsLockerDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<GoodsLockerDTO> pagination)
        {
            var joined = from p in _dbContext.GoodsLockers.AsNoTracking()
                         from s in _dbContext.SysOtherLists.AsNoTracking().Where(s=> s.ID == p.AREA).DefaultIfEmpty()
                         from s1 in _dbContext.SysOtherLists.AsNoTracking().Where(s1=> s1.ID == p.STATUS_ID).DefaultIfEmpty()
                             //tuy chinh
                         select new GoodsLockerDTO
                         {
                             Id = p.ID,
                             Code= p.CODE,
                             Price= p.PRICE,
                             Area= p.AREA,
                             AreaName = s.NAME,
                             MaintenanceFromDate = p.MAINTENANCE_FROM_DATE,
                             MaintenanceToDate = p.MAINTENANCE_TO_DATE,
                             StatusId = p.STATUS_ID,
                             StatusName = s1.NAME,
                             Note= p.NOTE
                         };
            var respose = await _genericRepository.PagingQueryList(joined, pagination);
            return new FormatedResponse
            {
                InnerBody = respose,
            };
        }

        public async Task<FormatedResponse> GetById(long id)
        {
           
                var joined = (from p in _dbContext.GoodsLockers.AsNoTracking().Where(p=> p.ID == id)
                              from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.AREA).DefaultIfEmpty()
                              from s1 in _dbContext.SysOtherLists.AsNoTracking().Where(s1 => s1.ID == p.STATUS_ID).DefaultIfEmpty()
                              
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new GoodsLockerDTO
                              {
                                  Id = p.ID,
                                  Code = p.CODE,
                                  Price = p.PRICE,
                                  Area = p.AREA,
                                  AreaName = s.NAME,
                                  MaintenanceFromDate = p.MAINTENANCE_FROM_DATE,
                                  MaintenanceToDate = p.MAINTENANCE_TO_DATE,
                                  StatusId = p.STATUS_ID,
                                  StatusName = s1.NAME,
                                  Note = p.NOTE
                              }).FirstOrDefault();

            if (joined != null)
            {
                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = "ENTITY_NOT_FOUND", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> Create(GoodsLockerDTO dto, string sid)
        {
            dto.Code = CreateNewCode();
            var response = await _genericRepository.Create(dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<GoodsLockerDTO> dtos, string sid)
        {
            var add = new List<GoodsLockerDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GoodsLockerDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<GoodsLockerDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange(dtos, sid, patchMode);
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
        public string CreateNewCode()
        {
            string newCode = "";
            if (_dbContext.GoodsLockers.Count() == 0)
            {
                newCode = "LOC001";
            }
            else
            {
                string lastestData = _dbContext.GoodsLockers.OrderByDescending(t => t.CODE).First().CODE!.ToString();

                newCode = lastestData.Substring(0, 3) + (int.Parse(lastestData.Substring(lastestData.Length - 4)) + 1).ToString("D3");
            }

            return newCode;

        }
    }
}

