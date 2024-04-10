﻿namespace GYM_BE.DTO
{
    public class GymPackageDTO : BaseDTO
    {
        public string? Code { get; set; }
        public decimal? Money { get; set; }
        public double? Period { get; set; }
        public long? ShiftId { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }
}