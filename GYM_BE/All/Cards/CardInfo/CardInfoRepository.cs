using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using GYM_BE.ENTITIES;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.NetworkInformation;
using CORE.AutoMapper;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using DocumentFormat.OpenXml.Spreadsheet;

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

            // lưu lịch sử
            var createCard = new CARD_HISTORY()
            {
                CODE = dto.Code,
                CARD_TYPE_ID = dto.CardTypeId,
                EFFECTED_DATE = dto.EffectedDate,
                EXPIRED_DATE = dto.ExpiredDate,
                WARDROBE = dto.Wardrobe,
                IS_HAVE_PT = dto.IsHavePt,
                SHIFT_ID = dto.ShiftId,
                PRICE = dto.Price,
                NOTE = dto.Note,
                ACTION = 1,
                IS_ACTIVE = dto.IsActive,
                CREATED_DATE = DateTime.UtcNow,
                CREATED_BY = sid
            };

            var result = await _dbContext.CardHistorys.AddAsync(createCard);
            await _dbContext.SaveChangesAsync();
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
            var oldDate = await _dbContext.CardInfos.FirstOrDefaultAsync(x => x.ID == dto.Id);

            int different = 0;
            // so sanh 2 du lieu moi va cu
            if(oldDate!.CARD_TYPE_ID != dto.CardTypeId)
            {
                different++;
            }
            else if (oldDate!.EFFECTED_DATE != dto.EffectedDate)
            {
                different++;
            }
            else if (oldDate!.EXPIRED_DATE != dto.ExpiredDate)
            {
                different++;
            }
            else if (oldDate!.WARDROBE != dto.Wardrobe)
            {
                different++;
            }
            else if (oldDate!.IS_HAVE_PT != dto.IsHavePt)
            {
                different++;
            }
            else if (oldDate!.SHIFT_ID != dto.ShiftId)
            {
                different++;
            }
            else if (oldDate!.PRICE != dto.Price)
            {
                different++;
            }
            else if (oldDate!.NOTE != dto.Note)
            {
                different++;
            }
            else if (oldDate!.IS_ACTIVE != dto.IsActive)
            {
                different++;
            }
            if(different != 0)
            {
                // lưu lịch sử
                var updateCard = new CARD_HISTORY()
                {
                    CODE = dto.Code,
                    CARD_TYPE_ID = dto.CardTypeId,
                    EFFECTED_DATE = dto.EffectedDate,
                    EXPIRED_DATE = dto.ExpiredDate,
                    WARDROBE = dto.Wardrobe,
                    IS_HAVE_PT = dto.IsHavePt,
                    SHIFT_ID = dto.ShiftId,
                    PRICE = dto.Price,
                    NOTE = dto.Note,
                    ACTION = 2,
                    IS_ACTIVE = dto.IsActive,
                    CREATED_DATE = DateTime.UtcNow,
                    CREATED_BY = sid
                };

                var result = await _dbContext.CardHistorys.AddAsync(updateCard);
                await _dbContext.SaveChangesAsync();
            }
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<CardInfoDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange(dtos, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> DeleteNew(long id, string sid)
        {
            var oldDate = await _dbContext.CardInfos.FirstOrDefaultAsync(x => x.ID == id);
            // lưu lịch sử
            var updateCard = new CARD_HISTORY()
            {
                CODE = oldDate!.CODE,
                CARD_TYPE_ID = oldDate!.CARD_TYPE_ID,
                EFFECTED_DATE = oldDate!.EFFECTED_DATE,
                EXPIRED_DATE = oldDate!.EXPIRED_DATE,
                WARDROBE = oldDate!.WARDROBE,
                IS_HAVE_PT = oldDate!.IS_HAVE_PT,
                SHIFT_ID = oldDate!.SHIFT_ID,
                PRICE = oldDate!.PRICE,
                NOTE = oldDate!.NOTE,
                ACTION = 3,
                IS_ACTIVE = oldDate!.IS_ACTIVE,
                CREATED_DATE = DateTime.UtcNow,
                CREATED_BY = sid
            };

            var result = await _dbContext.CardHistorys.AddAsync(updateCard);
            await _dbContext.SaveChangesAsync();
            var response = await _genericRepository.Delete(id);
            return response;
        }

        public async Task<FormatedResponse> DeleteIdsNew(List<long> ids, string sid)
        {
            foreach(var id in ids)
            {
                var oldDate = await _dbContext.CardInfos.FirstOrDefaultAsync(x => x.ID == id);
                // lưu lịch sử
                var updateCard = new CARD_HISTORY()
                {
                    CODE = oldDate!.CODE,
                    CARD_TYPE_ID = oldDate!.CARD_TYPE_ID,
                    EFFECTED_DATE = oldDate!.EFFECTED_DATE,
                    EXPIRED_DATE = oldDate!.EXPIRED_DATE,
                    WARDROBE = oldDate!.WARDROBE,
                    IS_HAVE_PT = oldDate!.IS_HAVE_PT,
                    SHIFT_ID = oldDate!.SHIFT_ID,
                    PRICE = oldDate!.PRICE,
                    NOTE = oldDate!.NOTE,
                    ACTION = 3,
                    IS_ACTIVE = oldDate!.IS_ACTIVE,
                    CREATED_DATE = DateTime.UtcNow,
                    CREATED_BY = sid
                };

                var result = await _dbContext.CardHistorys.AddAsync(updateCard);
                await _dbContext.SaveChangesAsync();
            }
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
    }
}

