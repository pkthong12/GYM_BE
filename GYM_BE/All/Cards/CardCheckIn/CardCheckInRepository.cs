using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using GYM_BE.ENTITIES;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GYM_BE.All.CardCheckIn
{
    public class CardCheckInRepository : ICardCheckInRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<CARD_CHECK_IN, CardCheckInDTO> _genericRepository;

        public CardCheckInRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<CARD_CHECK_IN, CardCheckInDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<CardCheckInDTO> pagination)
        {
            var joined = from p in _dbContext.CardCheckIns.AsNoTracking()
                         from i in _dbContext.CardInfos.Where(x => x.ID == p.CARD_INFO_ID).DefaultIfEmpty()
                         from c in _dbContext.PerCustomers.Where(x => x.ID == i.CUSTOMER_ID).DefaultIfEmpty()
                         from s in _dbContext.SysOtherLists.Where(x => x.ID == i.CARD_TYPE_ID).DefaultIfEmpty()
                         from g in _dbContext.SysOtherLists.Where(x => x.ID == c.GENDER_ID).DefaultIfEmpty()
                         select new CardCheckInDTO
                         {
                             Id = p.ID,
                             CodeCus = c.CODE,
                             CustomerName = c.FULL_NAME,
                             CardTypeName = s.NAME,
                             TimeEnd = p.TIME_END,
                             GenderName = g.NAME,
                             TimeStart = p.TIME_START,
                             TimeStartString = p.TIME_START!.Value.ToString("dd/MM/yyyy"),
                             TimeEndString = p.TIME_END!.Value.ToString("dd/MM/yyyy"),
                         };
            var respose = await _genericRepository.PagingQueryList(joined, pagination);
            return new FormatedResponse
            {
                InnerBody = respose,
            };
        }

        public async Task<FormatedResponse> GetById(long id)
        {
            var joined = await (from p in _dbContext.CardCheckIns.AsNoTracking()
                               from i in _dbContext.CardInfos.Where(x => x.ID == p.CARD_INFO_ID).DefaultIfEmpty()
                               from c in _dbContext.PerCustomers.Where(x => x.ID == i.CUSTOMER_ID).DefaultIfEmpty()
                               from s in _dbContext.SysOtherLists.Where(x => x.ID == i.CARD_TYPE_ID).DefaultIfEmpty()
                               from g in _dbContext.SysOtherLists.Where(x => x.ID == c.GENDER_ID).DefaultIfEmpty()
                               where p.ID == id
                               select new CardCheckInDTO
                               {
                                   Id = p.ID,
                                   CodeCus = c.CODE,
                                   CustomerName = c.FULL_NAME,
                                   CardTypeName = s.NAME,
                                   TimeEnd = p.TIME_END,
                                   GenderName = g.NAME,
                                   TimeStart = p.TIME_START,
                                   TimeStartString = p.TIME_START!.Value.ToString("dd/MM/yyyy"),
                                   TimeEndString = p.TIME_END!.Value.ToString("dd/MM/yyyy"),
                               }).FirstOrDefaultAsync();
            if (joined != null)
            {
                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = "ENTITY_NOT_FOUND", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> Create(CardCheckInDTO dto, string sid)
        {
            var response = await _genericRepository.Create(dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<CardCheckInDTO> dtos, string sid)
        {
            var add = new List<CardCheckInDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(CardCheckInDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<CardCheckInDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange(dtos, sid, patchMode);
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

        public Task<FormatedResponse> Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}

