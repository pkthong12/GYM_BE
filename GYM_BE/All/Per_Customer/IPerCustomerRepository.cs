using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.ENTITIES;

namespace GYM_BE.All.PerCustomer
{
    public interface IPerCustomerRepository: IGenericRepository<PER_CUSTOMER, Per_CustomerDTO>
    {
        Task<FormatedResponse> QueryList(PaginationDTO pagination);
    }
}

