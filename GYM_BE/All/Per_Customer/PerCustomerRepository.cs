﻿using Azure.Core;
using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using GYM_BE.ENTITIES;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;

namespace GYM_BE.All.PerCustomer
{
    public class PerCustomerRepository : IPerCustomerRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<PER_CUSTOMER, Per_CustomerDTO> _genericRepository;

        public PerCustomerRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<PER_CUSTOMER, Per_CustomerDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO pagination)
        {
            var joined = from p in _dbContext.PerCustomers.AsNoTracking()
                         from gr in _dbContext.SysOtherLists.Where(x => x.ID == p.CUSTOMER_CLASS_ID).DefaultIfEmpty()
                         from gender in _dbContext.SysOtherLists.Where(x => x.ID == p.GENDER_ID).DefaultIfEmpty()
                         from nav in _dbContext.SysOtherLists.Where(x => x.ID == p.NATIVE_ID).DefaultIfEmpty()
                         from re in _dbContext.SysOtherLists.Where(x => x.ID == p.RELIGION_ID).DefaultIfEmpty()
                         from b in _dbContext.SysOtherLists.Where(x => x.ID == p.BANK_ID).DefaultIfEmpty()
                         from bb in _dbContext.SysOtherLists.Where(x => x.ID == p.BANK_BRANCH).DefaultIfEmpty()
                         select new Per_CustomerDTO
                         {
                             Id = p.ID,
                             Avatar = p.AVATAR,
                             FullName = p.FULL_NAME,
                             Code = p.CODE,
                             CustomerClassId = p.CUSTOMER_CLASS_ID,
                             CustomerClassName = gr.NAME,
                             BirthDate = p.BIRTH_DATE,
                             GenderId = p.GENDER_ID,
                             GenderName = gender.NAME,
                             Address = p.ADDRESS,
                             PhoneNumber = p.PHONE_NUMBER,
                             Email = p.EMAIL,
                             NativeId = p.NATIVE_ID,
                             NativeName = nav.NAME,
                             ReligionId = p.RELIGION_ID,
                             ReligionName = re.NAME,
                             BankId = p.BANK_ID,
                             BankName = b.NAME,
                             BankBranch = p.BANK_BRANCH,
                             BankBranchName = bb.NAME,
                             BankNo = p.BANK_NO,
                             Note = p.NOTE,
                             Status = p.IS_ACTIVE == true ? "Hoạt động" : "Ngừng hoạt động"
                         };
            var respose = await _genericRepository.PagingQueryList(joined, pagination);
            return new FormatedResponse
            {
                InnerBody = respose,
            };
        }

        public async Task<FormatedResponse> GetById(long id)
        {
            var joined = await (from l in _dbContext.PerCustomers.AsNoTracking().Where(x => x.ID == id)
                          from gr in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == l.CUSTOMER_CLASS_ID).DefaultIfEmpty()
                          from gender in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == l.GENDER_ID).DefaultIfEmpty()
                          from nav in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == l.NATIVE_ID).DefaultIfEmpty()
                          from re in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == l.RELIGION_ID).DefaultIfEmpty()
                          from b in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == l.BANK_ID).DefaultIfEmpty()
                          from bb in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == l.BANK_BRANCH).DefaultIfEmpty()
                          select new Per_CustomerDTO
                          {
                              Id = l.ID,
                              Avatar = l.AVATAR,
                              FullName = l.FULL_NAME,
                              Code = l.CODE,
                              CustomerClassId = l.CUSTOMER_CLASS_ID,
                              CustomerClassName = gr.NAME,
                              BirthDate = l.BIRTH_DATE,
                              GenderId = l.GENDER_ID,
                              GenderName = gender.NAME,
                              Address = l.ADDRESS,
                              PhoneNumber = l.PHONE_NUMBER,
                              Email = l.EMAIL,
                              NativeId = l.NATIVE_ID,
                              NativeName = nav.NAME,
                              ReligionId = l.RELIGION_ID,
                              ReligionName = re.NAME,
                              BankId = l.BANK_ID,
                              BankName = b.NAME,
                              BankBranch = l.BANK_BRANCH,
                              BankBranchName = bb.NAME,
                              BankNo = l.BANK_NO,
                              Note = l.NOTE,
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

        public async Task<FormatedResponse> Create(Per_CustomerDTO dto, string sid)
        {
            var response = await _genericRepository.Create(dto, "root");
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<Per_CustomerDTO> dtos, string sid)
        {
            var add = new List<Per_CustomerDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, "root");
            return response;
        }

        public async Task<FormatedResponse> Update(Per_CustomerDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, "root", patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<Per_CustomerDTO> dtos, string sid, bool patchMode = true)
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
            var response = await _genericRepository.ToggleActiveIds(ids, valueToBind, sid);
            return response;
        }

        public Task<FormatedResponse> Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}

