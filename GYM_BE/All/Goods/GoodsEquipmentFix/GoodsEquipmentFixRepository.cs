using GYM_BE.Core.Dto;
using GYM_BE.Core.Generic;
using GYM_BE.DTO;
using GYM_BE.Entities;
using Microsoft.EntityFrameworkCore;

namespace GYM_BE.All.GoodsEquipmentFix
{
    public class GoodsEquipmentFixRepository : IGoodsEquipmentFixRepository
    {
        private readonly FullDbContext _dbContext;
       private readonly GenericRepository<GOODS_EQUIPMENT_FIX, GoodsEquipmentFixDTO> _genericRepository;

        public GoodsEquipmentFixRepository(FullDbContext context)
        {
            _dbContext = context;
            _genericRepository = new GenericRepository<GOODS_EQUIPMENT_FIX, GoodsEquipmentFixDTO>(_dbContext);
        }

        public async Task<FormatedResponse> QueryList(PaginationDTO<GoodsEquipmentFixDTO> pagination)
        {
            var joined = from p in _dbContext.GoodsEquipmentFixs.AsNoTracking()
                         from e in _dbContext.GoodsEquipments.AsNoTracking().Where(e => e.ID == p.EQUIPMENT_ID).DefaultIfEmpty()
                         from s1 in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.TYPE_ID).DefaultIfEmpty()
                         from s2 in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.RESULT_ID).DefaultIfEmpty()
                         from emp in _dbContext.PerEmployees.AsNoTracking().Where(e => e.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                         select new GoodsEquipmentFixDTO
                        {
                            Id = p.ID,
                            Code = p.CODE,
                            EquipmentId = p.EQUIPMENT_ID,
                            EquipmentName = e.NAME,
                            TypeId = p.TYPE_ID,
                            TypeName = s1.NAME,
                            StartDate = p.START_DATE,
                            EndDate = p.END_DATE,
                            ResultId = p.RESULT_ID,
                            Result = s2.NAME,
                            Money = p.MONEY,
                            ExpectedUseTime = p.EXPECTED_USE_TIME,
                            EmployeeId = p.EMPLOYEE_ID,
                            EmployeeName = emp.FULL_NAME,
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
                var list = new List<GOODS_EQUIPMENT_FIX>
                    {
                        (GOODS_EQUIPMENT_FIX)response
                    };
                var joined = (from p in list
                              from e in _dbContext.GoodsEquipments.AsNoTracking().Where(e => e.ID == p.EQUIPMENT_ID).DefaultIfEmpty()
                              from s1 in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.TYPE_ID).DefaultIfEmpty()
                              from s2 in _dbContext.SysOtherLists.AsNoTracking().Where(s => s.ID == p.RESULT_ID).DefaultIfEmpty()
                              from emp in _dbContext.PerEmployees.AsNoTracking().Where(e => e.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
                              select new GoodsEquipmentFixDTO
                              {
                                  Id = p.ID,
                                  Code = p.CODE,
                                  EquipmentId = p.EQUIPMENT_ID,
                                  EquipmentName = e.NAME,
                                  TypeId = p.TYPE_ID,
                                  TypeName = s1.NAME,
                                  StartDate = p.START_DATE,
                                  EndDate = p.END_DATE,
                                  ResultId = p.RESULT_ID,
                                  Result = s2.NAME,
                                  Money = p.MONEY,
                                  ExpectedUseTime = p.EXPECTED_USE_TIME,
                                  EmployeeId = p.EMPLOYEE_ID,
                                  EmployeeName = emp.FULL_NAME,
                                  Note = p.NOTE,
                              }).FirstOrDefault();

                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = "ENTITY_NOT_FOUND", ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> Create(GoodsEquipmentFixDTO dto, string sid)
        {
            var response = await _genericRepository.Create(dto, "root");
            return response;
        }

        public async Task<FormatedResponse> CreateRange(List<GoodsEquipmentFixDTO> dtos, string sid)
        {
            var add = new List<GoodsEquipmentFixDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(add, "root");
            return response;
        }

        public async Task<FormatedResponse> Update(GoodsEquipmentFixDTO dto, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.Update(dto, "root", patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(List<GoodsEquipmentFixDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange(dtos, "root", patchMode);
            return response;
        }

        public async Task<FormatedResponse> Delete(long id)
        {
            var response = await _genericRepository.Delete(id);
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

    }
}

