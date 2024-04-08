﻿namespace GYM_BE.DTO
{
    public class Per_CustomerDTO : BaseDTO
    {
        public string? Avatar { get; set; }
        public string? Code { get; set; }
        public long? CustomerClassId { get; set; }
        public string? CustomerClassName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public long? GenderId { get; set; }
        public string? GenderName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public long? NativeId { get; set; }
        public string? NativeName { get; set; }
        public long? ReligionId { get; set; }
        public string? ReligionName { get; set; }
        public long? BankId { get; set; }
        public string? BankName { get; set; }
        public long? BankBranch { get; set; }
        public string? BankBranchName { get; set; }
        public string? BankNo { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
    }
}
