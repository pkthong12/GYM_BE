namespace GYM_BE.DTO
{
    public class GoodsLockerDTO : BaseDTO
    {
        public string? Code { get; set; }
        public long? Area { get; set; }
        public string? AreaName { get; set; }
        public long? Price { get; set; }
        public long? StatusId { get; set; }
        public string? StatusName { get; set; }
        public string? MaintenanceFromDate { get; set; }
        public string? MaintenanceToDate { get; set; }
        public string? Note { get; set; }
    }
}

