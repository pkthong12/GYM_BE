using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using GYM_BE.ENTITIES;
using Microsoft.EntityFrameworkCore;

namespace GYM_BE.All.GoodsEquipment
{
    public class GoodsEquipmentRepository : IGoodsEquipmentRepository
    {
        private readonly FullDbContext _dbContext;
       private readonly GenericRepository<GOODS_EQUIPMENT, GoodsEquipmentDTO> _genericRepository;

        public GoodsEquipmentRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<GOODS_EQUIPMENT, GoodsEquipmentDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<GoodsEquipmentDTO> pagination)
        {
            var joined = from p in _dbContext.GoodsEquipments.AsNoTracking()
                         from s1 in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.EQUIPMENT_TYPE).DefaultIfEmpty()
                         from s2 in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.STATUS_ID).DefaultIfEmpty()
                         from e in _dbContext.PerEmployees.AsNoTracking().Where(e => e.ID == p.MANAGER_ID).DefaultIfEmpty()
                         select new GoodsEquipmentDTO
                        {
                            Id = p.ID,
                            Code = p.CODE,
                            Name = p.NAME,
                            EquipmentType = p.EQUIPMENT_TYPE,
                            EquipmentTypeName = s1.NAME,
                            Manufacturer = p.MANUFACTURER,
                            PurchaseDate = p.PURCHASE_DATE,
                            StatusId = p.STATUS_ID,
                            Status = s2.NAME,
                            WarrantyExpiryDate = p.WARRANTY_EXPIRY_DATE,
                            Cost = p.COST,
                            Address = p.ADDRESS,
                            ManagerId = p.MANAGER_ID,
                            ManagerName = e.FULL_NAME,
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
                var list = new List<GOODS_EQUIPMENT>
                    {
                        (GOODS_EQUIPMENT)response
                    };
                var joined = (from p in list
                              from s1 in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.EQUIPMENT_TYPE).DefaultIfEmpty()
                              from s2 in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.STATUS_ID).DefaultIfEmpty()
                              from e in _dbContext.PerEmployees.AsNoTracking().Where(e => e.ID == p.MANAGER_ID).DefaultIfEmpty()
                              select new GoodsEquipmentDTO
                              {
                                  Id = p.ID,
                                  Code = p.CODE,
                                  Name = p.NAME,
                                  EquipmentType = p.EQUIPMENT_TYPE,
                                  EquipmentTypeName = s1.NAME,
                                  Manufacturer = p.MANUFACTURER,
                                  PurchaseDate = p.PURCHASE_DATE,
                                  StatusId = p.STATUS_ID,
                                  Status = s2.NAME,
                                  WarrantyExpiryDate = p.WARRANTY_EXPIRY_DATE,
                                  Cost = p.COST,
                                  Address = p.ADDRESS,
                                  ManagerId = p.MANAGER_ID,
                                  ManagerName = e.FULL_NAME,
                                  Note = p.NOTE,
                              }).FirstOrDefault();

                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = "ENTITY_NOT_FOUND", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> Create(GoodsEquipmentDTO dto, string sid)
        {
            var response = await _genericRepository.Create(dto, "root");
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<GoodsEquipmentDTO> dtos, string sid)
        {
            var add = new List<GoodsEquipmentDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, "root");
            return response;
        }

        public async Task<FormatedResponse> Update(GoodsEquipmentDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, "root", patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<GoodsEquipmentDTO> dtos, string sid, bool patchMode = true)
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

        public async Task<FormatedResponse> Delete(string id)
        {
            var response = await _genericRepository.Delete(id);
            return response;
        }
    }
}

