using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.EntityFrameworkCore;

namespace GYM_BE.All.PerEmployee
{
    public class PerEmployeeRepository : IPerEmployeeRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<PER_EMPLOYEE, PerEmployeeDTO> _genericRepository;

        public PerEmployeeRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<PER_EMPLOYEE, PerEmployeeDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<PerEmployeeDTO> pagination)
        {
            var joined = from p in _dbContext.PerEmployees.AsNoTracking()
                         from s1 in _dbContext.SysOtherLists.AsNoTracking().Where(s1 => s1.ID == p.GENDER_ID).DefaultIfEmpty()
                         from s2 in _dbContext.SysOtherLists.AsNoTracking().Where(s2=> s2.ID == p.STAFF_GROUP_ID).DefaultIfEmpty()

                             //tuy chinh
                         select new PerEmployeeDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             FullName = p.FULL_NAME,
                             IdNo = p.ID_NO,
                             Address = p.ADDRESS,
                             BirthDate = p.BIRTH_DATE,
                             PhoneNumber = p.PHONE_NUMBER,
                             Email = p.EMAIL,
                             GenderId = p.GENDER_ID,
                             GenderName = s1.NAME,
                             StaffGroupId = p.STAFF_GROUP_ID,
                             StaffGroupName = s2.NAME,
                             StatusId = p.STATUS_ID,
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
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<PER_EMPLOYEE>
                    {
                        (PER_EMPLOYEE)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new PerEmployeeDTO
                              {
                                  Id = l.ID,
                                  Code = l.CODE,
                                  FullName = l.FULL_NAME,
                                  IdNo = l.ID_NO,
                                  Address = l.ADDRESS,
                                  BirthDate = l.BIRTH_DATE,
                                  PhoneNumber = l.PHONE_NUMBER,
                                  Email = l.EMAIL,
                                  GenderId = l.GENDER_ID,
                                  StaffGroupId = l.STAFF_GROUP_ID,
                                  StatusId = l.STATUS_ID,
                                  Note = l.NOTE,
                              }).FirstOrDefault();

                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = "ENTITY_NOT_FOUND", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> Create(PerEmployeeDTO dto, string sid)
        {
            var response = await _genericRepository.Create(dto, "root");
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<PerEmployeeDTO> dtos, string sid)
        {
            var add = new List<PerEmployeeDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, "root");
            return response;
        }

        public async Task<FormatedResponse> Update(PerEmployeeDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, "root", patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<PerEmployeeDTO> dtos, string sid, bool patchMode = true)
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

        public Task<FormatedResponse> Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}

