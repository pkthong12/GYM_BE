﻿using GYM_BE.All.System.Authentication;
using GYM_BE.All.SysUser;
using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using GYM_BE.ENTITIES;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.CompilerServices;

namespace GYM_BE.All.System.SysUser
{
    public class SysUserRepository : ISysUserRepository
    {
        private readonly FullDbContext _dbContext;
        private readonly GenericRepository<SYS_USER, SysUserDTO> _genericRepository;

        public SysUserRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<SYS_USER, SysUserDTO>(_dbContext);

        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<SysUserDTO> pagination)
        {
            var joined = from l in _dbContext.SysUsers.AsNoTracking()
                         from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == l.GROUP_ID).DefaultIfEmpty()
                         from e in _dbContext.PerEmployees.AsNoTracking().Where(e => e.ID == l.EMPLOYEE_ID).DefaultIfEmpty()
                         select new SysUserDTO
                         {
                             Id = l.ID,
                             Username = l.USERNAME,
                             Fullname = l.FULLNAME,
                             GroupName = s.NAME,
                             Avatar = l.AVATAR,
                             EmployeeId = l.EMPLOYEE_ID,
                             EmployeeCode = e.CODE
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
                var list = new List<SYS_USER>
                    {
                        (SYS_USER)response
                    };
                var joined = (from l in list
                              from s in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == l.GROUP_ID).DefaultIfEmpty()
                              from e in _dbContext.PerEmployees.AsNoTracking().Where(e => e.ID == l.EMPLOYEE_ID).DefaultIfEmpty()
                              select new SysUserDTO
                              {
                                  Id = l.ID,
                                  Username = l.USERNAME,
                                  Fullname = l.FULLNAME,
                                  GroupName = s.NAME,
                                  Avatar = l.AVATAR,
                                  EmployeeId = l.EMPLOYEE_ID,
                                  EmployeeCode = e.CODE
                              }).FirstOrDefault();

                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = "ENTITY_NOT_FOUND", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> Create(SysUserDTO dto, string sid)
        {
            //dto.Passwordhash = BCrypt.Net.BCrypt.HashPassword(dto.Passwordhash);
            dto.Decentralization = dto.DecentralizationList == null ? "" : string.Join(",", dto.DecentralizationList);
            var response = await _genericRepository.Create(dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<SysUserDTO> dtos, string sid)
        {
            var add = new List<SysUserDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(SysUserDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<SysUserDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange(dtos, sid, patchMode);
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

        public Task<FormatedResponse> Delete(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> ClientsLogin(string UserName, string password)
        {
            try
            {
                UserName = UserName.ToLower().Trim();
                var r = await _dbContext.SysUsers.Where(p => p.USERNAME!.ToLower() == UserName).FirstOrDefaultAsync();
                if (r != null)
                {
                    var userID = r.ID;
                    var data = new AuthResponse()
                    {
                        Id = r.ID,
                        UserName = r.USERNAME!,
                        Password = r.PASSWORDHASH??"",
                        FullName = r.FULLNAME!,
                        IsAdmin = r.IS_ADMIN,
                        IsRoot = r.IS_ROOT,
                        Avatar = r.AVATAR!,
                        EmployeeId = r.EMPLOYEE_ID,
                        IsLock = r.IS_LOCK,
                        Decentralization = r.DECENTRALIZATION != null ? r.DECENTRALIZATION.Split(",").ToList() : new List<string>()
                    };
                    return new FormatedResponse() { InnerBody = data };
                }
                else
                {
                    return new FormatedResponse() { MessageCode = "ERROR_USERNAME_INCORRECT" };
                }
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message };
            }

        }
    }
}
