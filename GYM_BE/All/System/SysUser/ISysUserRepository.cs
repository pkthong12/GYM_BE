﻿using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using GYM_BE.ENTITIES;

namespace GYM_BE.All.SysUser
{
    
    public interface ISysUserRepository : IGenericRepository<SYS_USER, SysUserDTO>
    {
        Task<FormatedResponse> QueryList(PaginationDTO pagination);

    }
}
