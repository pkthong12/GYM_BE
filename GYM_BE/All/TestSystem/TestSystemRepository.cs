using API.DTO;
using CORE.AutoMapper;
using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.ENTITIES;
using Microsoft.EntityFrameworkCore;

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
            var join = await _genericRepository.Create(dto,"12344");
            return new FormatedResponse()
            {
                InnerBody = join,
                StatusCode = EnumStatusCode.StatusCode200
            };
        }

        public Task<FormatedResponse> CreateRange(List<TrCenterDTO> dtos, string sid)
        {
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> DeleteIds(List<long> ids)
        {
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> GetById(long id)
        {
            var join = await _dbContext.AD_PROGRAMS.AsNoTracking().FirstAsync();
            //AdProgramsDTO adProgramsDTO = new AdProgramsDTO()
            //{
            //    Code = "323",
            //};
            //var e = _genericRepository.Create(adProgramsDTO, "3232");
            return new FormatedResponse()
            {
                InnerBody = join,
                StatusCode = EnumStatusCode.StatusCode200
            };
        }

        public Task<FormatedResponse> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> ToggleActiveIds(List<long> ids, bool valueToBind, string sid)
        {
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> Update(TrCenterDTO dto, string sid, bool patchMode = true)
        {
            throw new NotImplementedException();
        }

        public Task<FormatedResponse> UpdateRange(List<TrCenterDTO> dtos, string sid, bool patchMode = true)
        {
            throw new NotImplementedException();
        }
    }
}
