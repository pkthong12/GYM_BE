using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.ENTITIES;

namespace GYM_BE.All.Gym.GymPackage
{
    public interface IGymPackageRepository : IGenericRepository<GYM_PACKAGE, GymPackageDTO>
    {
        Task<FormatedResponse> QueryList(PaginationDTO<GymPackageDTO> pagination);

    }
}
