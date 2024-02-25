using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;

namespace GYM_BE.All.TestSystem
{
    public class TestSystemRepository : ITestSystemRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<TR_CENTER, TrCenterDTO> _genericRepository;

        public TestSystemRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<TR_CENTER, TrCenterDTO>(context);
        }
        public async Task<FormatedResponse> Create(TrCenterDTO dto, string sid)
        {
            var join = await _genericRepository.Create(dto,sid);
            return new FormatedResponse()
            {
                InnerBody = join,
                StatusCode = EnumStatusCode.StatusCode200
            };
        }

        public async Task<FormatedResponse> CreateRange(List<TrCenterDTO> dtos, string sid)
        {
            throw new NotImplementedException();
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

        public async Task<FormatedResponse> GetById(long id)
        {
            var join = 0;
            return new FormatedResponse()
            {
                InnerBody = join,
                StatusCode = EnumStatusCode.StatusCode200
            };
        }

        public Task<FormatedResponse> QueryList(long id)
        {
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> ToggleActiveIds(List<long> ids, bool valueToBind, string sid)
        {
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Update(TrCenterDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update( dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<TrCenterDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange( dtos, sid, patchMode);
            return response;
        }
    }
}
