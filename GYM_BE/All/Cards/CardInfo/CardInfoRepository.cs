using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using GYM_BE.ENTITIES;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace GYM_BE.All.CardInfo
{
    public class CardInfoRepository : ICardInfoRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<CARD_INFO, CardInfoDTO> _genericRepository;

        public CardInfoRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<CARD_INFO, CardInfoDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<CardInfoDTO> pagination)
        {
            var joined = from p in _dbContext.CardInfos.AsNoTracking()
                         from sh in _dbContext.GymShifts.AsNoTracking().Where(sh=> sh.ID == p.SHIFT_ID).DefaultIfEmpty()
                         from c in _dbContext.PerCustomers.AsNoTracking().Where(x => x.ID == p.CUSTOMER_ID).DefaultIfEmpty()
                         from s in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.CARD_TYPE_ID).DefaultIfEmpty()
                         from g in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.GENDER_ID).DefaultIfEmpty()
                         select new CardInfoDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             EffectDateString = p.EFFECTED_DATE!,
                             ExpiredDateString = p.EXPIRED_DATE!,
                             CardTypeName = s.NAME,
                             CustomerName = c.FULL_NAME,
                             GenderName = g.NAME,
                             LockerId = p.LOCKER_ID,
                             Status = p.IS_ACTIVE!.Value == true ? "Hoạt động" : "Ngừng hoạt động",
                             Note = p.NOTE,
                             CodeCus = c.CODE,
                             Wardrobe= p.WARDROBE,
                             Price = p.PRICE,
                             ShiftId = p.SHIFT_ID,
                             ShiftName = sh.NAME,
                         };
            var respose = await _genericRepository.PagingQueryList(joined, pagination);
            return new FormatedResponse
            {
                InnerBody = respose,
            };
        }

        public async Task<FormatedResponse> GetById(long id)
        {
            var joined = await (from p in _dbContext.CardInfos.AsNoTracking()
                                from sh in _dbContext.GymShifts.AsNoTracking().Where(sh => sh.ID == p.SHIFT_ID).DefaultIfEmpty()
                                from c in _dbContext.PerCustomers.AsNoTracking().Where(x => x.ID == p.CUSTOMER_ID).DefaultIfEmpty()
                                from s in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.CARD_TYPE_ID).DefaultIfEmpty()
                                from g in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == c.GENDER_ID).DefaultIfEmpty()
                                where p.ID == id
                                select new CardInfoDTO
                                {
                                    Id = p.ID,
                                    Code = p.CODE,
                                    EffectDateString = p.EFFECTED_DATE!,
                                    ExpiredDateString = p.EXPIRED_DATE!,
                                    CardTypeId = p.CARD_TYPE_ID,
                                    CardTypeName = s.NAME,
                                    CustomerId = p.CUSTOMER_ID,
                                    CustomerName = c.FULL_NAME,
                                    GenderName = g.NAME,
                                    LockerId = p.LOCKER_ID,
                                    Status = p.IS_ACTIVE!.Value == true ? "Hoạt động" : "Ngừng hoạt động",
                                    Note = p.NOTE,
                                    EffectedDate = p.EFFECTED_DATE,
                                    ExpiredDate = p.EXPIRED_DATE,
                                    Wardrobe = p.WARDROBE,
                                    Price = p.PRICE,
                                    ShiftId = p.SHIFT_ID,
                                    ShiftName = sh.NAME,
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

        public async Task<FormatedResponse> Create(CardInfoDTO dto, string sid)
        {
            dto.IsActive = true;
            dto.Code = CreateNewCode();
            var response = await _genericRepository.Create(dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<CardInfoDTO> dtos, string sid)
        {
            var add = new List<CardInfoDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(CardInfoDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<CardInfoDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> GetListCustomer()
        {
            var res = await (from p in _dbContext.PerCustomers.AsNoTracking()
                             where p.IS_ACTIVE == true
                             select new
                             {
                                 Id = p.ID,
                                 Name = p.FULL_NAME
                             }).ToListAsync();
            return new FormatedResponse() { InnerBody = res };
        }
        public string CreateNewCode()
        {
            string newCode = "";
            if (_dbContext.CardInfos.Count() == 0)
            {
                newCode = "CARD001";
            }
            else
            {
                string lastestData = _dbContext.CardInfos.OrderByDescending(t => t.CODE).First().CODE!.ToString();

                newCode = lastestData.Substring(0, 3) + (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
            }

            return newCode;

        }
    }
}

