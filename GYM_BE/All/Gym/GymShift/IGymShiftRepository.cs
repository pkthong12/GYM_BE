using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.ENTITIES;

namespace GYM_BE.All.Gym.GymShift
{
    public interface IGymShiftRepository : IGenericRepository<GYM_SHIFT, GymShiftDTO>
    {
        Task<FormatedResponse> QueryList(PaginationDTO<GymShiftDTO> pagination);

    }
}
