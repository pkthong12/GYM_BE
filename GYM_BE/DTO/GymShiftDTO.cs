namespace GYM_BE.DTO
{
    public class GymShiftDTO : BaseDTO
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? TotalDays { get; set; }
        public DateTime? HoursStart { get; set; }
        public DateTime? HoursEnd { get; set; }
        public bool? IsActive { get; set; }
        public string? Note { get; set; }
    }
}
