﻿using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;

namespace GYM_BE.All.SysOtherList
{
    public interface ISysOtherListRepository : IGenericRepository<SYS_OTHER_LIST, SysOtherListDTO>
    {
        Task<FormatedResponse> QueryList(PaginationDTO pagination);

    }
}
