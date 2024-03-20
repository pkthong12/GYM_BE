using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.EntityFrameworkCore;

namespace GYM_BE.All.TrCenter
{
    public class TrCenterRepository : ITrCenterRepository
    {
        private readonly FullDbContext _dbContext;
       private readonly GenericRepository<TR_CENTER, TrCenterDTO> _genericRepository;

        public TrCenterRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<TR_CENTER, TrCenterDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(long id)
        {
            var joined = await(from p in _dbContext.TrCenters.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         select new TrCenterDTO
                         {
                             Id = p.ID,
                             CodeCenter = p.CODE_CENTER,
                             NameCenter = p.NAME_CENTER,
                             TrainingField = p.TRAINING_FIELD,
                             Address = p.ADDRESS,
                             Phone = p.PHONE,
                             Representative = p.REPRESENTATIVE,
                             ContactPerson = p.CONTACT_PERSON,
                             PhoneContactPerson = p.PHONE_CONTACT_PERSON,
                             Website = p.WEBSITE,
                             Note = p.NOTE,
                             AttachedFile = p.ATTACHED_FILE,
                             IsActive = p.IS_ACTIVE,
                             Status = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng",
                         }).ToListAsync();
            var response = new
            {
                List = joined,
                Count = joined.Count,
            };
            return new FormatedResponse() { InnerBody = response };
        }

        public async Task<FormatedResponse> GetById(long id)
        {
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<TR_CENTER>
                    {
                        (TR_CENTER)response
                    };
                var joined = (from l in list
                              // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new TrCenterDTO
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

        public async Task<FormatedResponse> Create(TrCenterDTO dto, string sid)
        {
            var response = await _genericRepository.Create(dto, "root");
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<TrCenterDTO> dtos, string sid)
        {
            var add = new List<TrCenterDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, "root");
            return response;
        }

        public async Task<FormatedResponse> Update(TrCenterDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, "root", patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<TrCenterDTO> dtos, string sid, bool patchMode = true)
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

