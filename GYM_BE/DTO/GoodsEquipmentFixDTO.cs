namespace GYM_BE.DTO
{
    public class GoodsEquipmentFixDTO : BaseDTO
    {
        public string? Code { get; set; }
        public long? EquipmentId  { get; set; }
       public string? EquipmentName  { get; set; }
       public int? TypeId  { get; set; }
       public string? TypeName  { get; set; }
       public DateTime? StartDate  { get; set; }
       public DateTime? EndDate  { get; set; }
       public long? ResultId  { get; set; }
       public string? Result  { get; set; }
       public decimal? Money  { get; set; }
       public DateTime? ExpectedUseTime  { get; set; }
       public long? EmployeeId  { get; set; }
       public string? EmployeeName  { get; set; }
       public string? Note  { get; set; }
    }
}

