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
                             CardCode = i.CODE,
                             CodeCus = c.CODE,
                             CustomerName = c.FULL_NAME,
                             CardTypeName = s.NAME,
                             TimeEnd = p.TIME_END,
                             GenderName = g.NAME,
                             TimeStart = p.TIME_START,
                             DayCheckIn = p.DAY_CHECK_IN,
                             DayCheckInString = p.DAY_CHECK_IN!.Value.ToString("dd/MM/yyyy"),
                             TimeStartString = p.TIME_START!.Value.ToString("HH:mm:ss"),
                             TimeEndString = p.TIME_END!.Value.ToString("HH:mm:ss"),
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
                                   CardCode = i.CODE,
                                   CodeCus = c.CODE,
                                   CustomerName = c.FULL_NAME,
                                   CardTypeName = s.NAME,
                                   TimeEnd = p.TIME_END,
                                   GenderName = g.NAME,
                                   TimeStart = p.TIME_START,
                                   DayCheckIn = p.DAY_CHECK_IN,
                                   DayCheckInString = p.DAY_CHECK_IN!.Value.ToString("dd/MM/yyyy"),
                                   TimeStartString = p.TIME_START!.Value.ToString("HH:mm:ss"),
                                   TimeEndString = p.TIME_END!.Value.ToString("HH:mm:ss"),
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

        public async Task<FormatedResponse> CheckIn(string cardCode, string sid)
        {
            var checkExistCard =await _dbContext.CardInfos.AnyAsync(x => x.CODE!.ToUpper() == cardCode.ToUpper());
            if (!checkExistCard)
            {
                return new FormatedResponse() { MessageCode = "ENTITY_NOT_FOUND", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
            var cardId = _dbContext.CardInfos.FirstOrDefault(x => x.CODE!.ToUpper() == cardCode.ToUpper())!.ID;
            var checkExsist = _dbContext.CardCheckIns.Where(x => x.CARD_INFO_ID == cardId && x.DAY_CHECK_IN!.Value.Date == DateTime.Now.Date).ToList();
            if(checkExsist.Count() == 0)
            {
                var checkIn = new CardCheckInDTO
                {
                    CardInfoId = cardId,
                    TimeStart = DateTime.Now,
                    DayCheckIn = DateTime.Now.Date,
                };
                var response = await _genericRepository.Create(checkIn, sid);
                return response;
            }
            else
            {
                var checkIn = _dbContext.CardCheckIns.FirstOrDefault(x => x.CARD_INFO_ID == cardId && x.DAY_CHECK_IN!.Value.Date == DateTime.Now.Date);
                var card = new CardCheckInDTO();
                card.Id = checkIn!.ID;
                card.DayCheckIn = checkIn.DAY_CHECK_IN!.Value;
                card.CardInfoId = cardId;
                card.TimeStart = checkIn.TIME_START;
                card.TimeEnd = DateTime.Now;
                var response = await _genericRepository.Update(card, sid, true);
                return response;
            }
        }
    }
}

