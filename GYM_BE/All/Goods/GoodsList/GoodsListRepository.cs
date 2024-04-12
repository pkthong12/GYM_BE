using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.EntityFrameworkCore;

namespace GYM_BE.All.GoodsList
{
    public class GoodsListRepository : IGoodsListRepository
    {
        private readonly FullDbContext _dbContext;
       private readonly GenericRepository<GOODS_LIST, GoodsListDTO> _genericRepository;

        public GoodsListRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<GOODS_LIST, GoodsListDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<GoodsListDTO> pagination)
        {
            var joined = from p in _dbContext.GoodsLists.AsNoTracking()
                                       //tuy chinh
                       select new GoodsListDTO
                        {
                            Id = p.ID,
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
                var list = new List<GOODS_LIST>
                    {
                        (GOODS_LIST)response
                    };
                var joined = (from l in list
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new GoodsListDTO
                              {
                                  Id = l.ID
                              }).FirstOrDefault();

                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = "ENTITY_NOT_FOUND", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> Create(GoodsListDTO dto, string sid)
        {
            var response = await _genericRepository.Create(dto, "root");
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<GoodsListDTO> dtos, string sid)
        {
            var add = new List<GoodsListDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, "root");
            return response;
        }

        public async Task<FormatedResponse> Update(GoodsListDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, "root", patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<GoodsListDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}

