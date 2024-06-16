using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;

namespace GYM_BE.All.OrdBill
{
    public interface IOrdBillRepository: IGenericRepository<ORD_BILL, OrdBillDTO>
    {
        Task<FormatedResponse> QueryList(PaginationDTO<OrdBillDTO> pagination);
    }
}

