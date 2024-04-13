namespace GYM_BE.DTO
{
    public class CardInfoDTO : BaseDTO
    {
        public string? Code { get; set; }
        public DateTime? EffectedDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public long? CardTypeId { get; set; }
        public string? CardTypeName { get; set; }
        public string? GenderName { get; set; }
        public long? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public long? LockerId { get; set; }
        public string? LockerName { get; set; }
        public string? Note { get; set; }
        public string? EffectDateString { get; set; }
        public string? ExpiredDateString { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
        public string? CodeCus { get; set; }
    }
}
