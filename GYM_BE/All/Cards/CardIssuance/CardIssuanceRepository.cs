using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.EntityFrameworkCore;

namespace GYM_BE.All.CardIssuance
{
    public class CardIssuanceRepository : ICardIssuanceRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<CARD_ISSUANCE, CardIssuanceDTO> _genericRepository;

        public CardIssuanceRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<CARD_ISSUANCE, CardIssuanceDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<CardIssuanceDTO> pagination)
        {
            var joined = from p in _dbContext.CardIssuances.AsNoTracking()
                         from e in _dbContext.PerEmployees.AsNoTracking().Where(e=> e.ID == p.PER_SELL_ID).DefaultIfEmpty()
                         from e1 in _dbContext.PerEmployees.AsNoTracking().Where(e1=> e1.ID == p.PER_PT_ID).DefaultIfEmpty()
                         from c in _dbContext.CardInfos.AsNoTracking().Where(c=> c.ID == p.CARD_ID).DefaultIfEmpty()
                         from cu in _dbContext.PerCustomers.AsNoTracking().Where(cu=> cu.ID == p.CUSTOMER_ID).DefaultIfEmpty()
                         from l in _dbContext.GoodsLockers.AsNoTracking().Where(l=> l.ID== p.LOCKER_ID).DefaultIfEmpty()
                             //tuy chinh
                         select new CardIssuanceDTO
                         {
                             Id = p.ID,
                             DocumentNumber = p.DOCUMENT_NUMBER,
                             DocumentDate = p.DOCUMENT_DATE,
                             AfterDiscount = p.AFTER_DISCOUNT,
                             CardId = p.CARD_ID,
                             CardPrice = p.CARD_PRICE,
                             CustomerId = p.CUSTOMER_ID,
                             HourCard = p.HOUR_CARD,
                             HourCardBonus = p.HOUR_CARD_BONUS,
                             IsHavePt = p.IS_HAVE_PT,
                             IsRealPrice = p.IS_REAL_PRICE,
                             LockerId = p.LOCKER_ID,
                             MoneyHavePay = p.MONEY_HAVE_PAY,
                             PaidMoney = p.MONEY_HAVE_PAY,
                             PercentDiscount = p.PERCENT_DISCOUNT,
                             PercentVat = p.PERCENT_VAT,
                             PerPtId = p.PER_PT_ID,
                             PerSellId = p.PER_SELL_ID,
                             TotalHourCard = p.TOTAL_HOUR_CARD,
                             TotalPrice = p.TOTAL_PRICE,
                             Wardrobe = p.WARDROBE,
                             Note = p.NOTE,
                         };
            var respose = await _genericRepository.PagingQueryList(joined, pagination);
            return new FormatedResponse
            {
                InnerBody = respose,
            };
        }

        public async Task<FormatedResponse> GetById(long id)
        {
            var joined = await (from p in _dbContext.CardIssuances.AsNoTracking().Where(p=> p.ID == id)
                                from e in _dbContext.PerEmployees.AsNoTracking().Where(e => e.ID == p.PER_SELL_ID).DefaultIfEmpty()
                                from e1 in _dbContext.PerEmployees.AsNoTracking().Where(e1 => e1.ID == p.PER_PT_ID).DefaultIfEmpty()
                                from c in _dbContext.CardInfos.AsNoTracking().Where(c => c.ID == p.CARD_ID).DefaultIfEmpty()
                                from cu in _dbContext.PerCustomers.AsNoTracking().Where(cu => cu.ID == p.CUSTOMER_ID).DefaultIfEmpty()
                                from l in _dbContext.GoodsLockers.AsNoTracking().Where(l => l.ID == p.LOCKER_ID).DefaultIfEmpty()
                                    //tuy chinh
                                select new CardIssuanceDTO
                                {
                                    Id = p.ID,
                                    DocumentNumber = p.DOCUMENT_NUMBER,
                                    DocumentDate = p.DOCUMENT_DATE,
                                    AfterDiscount = p.AFTER_DISCOUNT,
                                    CardId = p.CARD_ID,
                                    CardPrice = p.CARD_PRICE,
                                    CustomerId = p.CUSTOMER_ID,
                                    HourCard = p.HOUR_CARD,
                                    HourCardBonus = p.HOUR_CARD_BONUS,
                                    IsHavePt = p.IS_HAVE_PT,
                                    IsRealPrice = p.IS_REAL_PRICE,
                                    LockerId = p.LOCKER_ID,
                                    MoneyHavePay = p.MONEY_HAVE_PAY,
                                    PaidMoney = p.MONEY_HAVE_PAY,
                                    PercentDiscount = p.PERCENT_DISCOUNT,
                                    PercentVat = p.PERCENT_VAT,
                                    PerPtId = p.PER_PT_ID,
                                    PerSellId = p.PER_SELL_ID,
                                    TotalHourCard = p.TOTAL_HOUR_CARD,
                                    TotalPrice = p.TOTAL_PRICE,
                                    Wardrobe = p.WARDROBE,
                                    Note = p.NOTE,
                                }).FirstAsync();
            if (joined != null)
            {
                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = "ENTITY_NOT_FOUND", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> Create(CardIssuanceDTO dto, string sid)
        {
            var response = await _genericRepository.Create(dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<CardIssuanceDTO> dtos, string sid)
        {
            var add = new List<CardIssuanceDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(CardIssuanceDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<CardIssuanceDTO> dtos, string sid, bool patchMode = true)
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

    }
}
