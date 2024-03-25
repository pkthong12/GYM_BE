using GYM_BE.All.SysOtherList;
using GYM_BE.All.SysOtherListType;
using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.EntityFrameworkCore;

namespace GYM_BE.All.System.SysOtherList
{
    public class SysOtherListRepository : ISysOtherListRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<SYS_OTHER_LIST, SysOtherListDTO> _genericRepository;

        public SysOtherListRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<SYS_OTHER_LIST, SysOtherListDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO pagination)
        {
            var joined = from p in _dbContext.SysOtherLists.AsNoTracking()
                         from t in _dbContext.SysOtherListTypes.AsNoTracking().Where(x => x.ID == p.TYPE_ID).DefaultIfEmpty()
                         select new SysOtherListDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Name = p.NAME,
                             TypeId = p.TYPE_ID,
                             TypeName = t.NAME,
                             Note = p.NOTE,
                             Orders = p.ORDERS,
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
                var list = new List<SYS_OTHER_LIST>
                    {
                        (SYS_OTHER_LIST)response
                    };
                var joined = (from l in list
                              from t in _dbContext.SysOtherListTypes.AsNoTracking().Where(x => x.ID == l.TYPE_ID).DefaultIfEmpty()
                              select new SysOtherListDTO
                              {
                                  Id = l.ID,
                                  Code = l.CODE,
                                  Name = l.NAME,
                                  TypeId = l.TYPE_ID,
                                  TypeName = t.NAME,
                                  Note = l.NOTE,
                                  Orders = l.ORDERS,
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

        public async Task<FormatedResponse> Create(SysOtherListDTO dto, string sid)
        {
            dto.IsActive = true;
            var response = await _genericRepository.Create(dto, "root");
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<SysOtherListDTO> dtos, string sid)
        {
            var add = new List<SysOtherListDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, "root");
            return response;
        }

        public async Task<FormatedResponse> Update(SysOtherListDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, "root", patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<SysOtherListDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange(dtos, "root", patchMode);
            return response;
        }

        public async Task<FormatedResponse> Delete(long id)
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
    }
}
