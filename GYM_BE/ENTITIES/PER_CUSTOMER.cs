using GYM_BE.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.ENTITIES;
[Table("PER_CUSTOMER")]

public class PER_CUSTOMER : BASE_ENTITY
{
    public string? AVATAR { get; set; }
    public string? CODE { get; set; }
    public long? CUSTOMER_CLASS_ID { get; set; }
    public string? FIRST_NAME { get; set; }
    public string? LAST_NAME { get; set; }
    public string? FULL_NAME { get; set; }
    public DateTime? BIRTH_DATE { get; set; }
    public long? GENDER_ID { get; set; }
    public string? ADDRESS { get; set; }
    public string? PHONE_NUMBER { get; set; }
    public string? EMAIL { get; set; }
    public long? NATIVE_ID { get; set; }
    public long? RELIGION_ID { get; set; }
    public long? BANK_ID { get; set; }
    public long? BANK_BRANCH { get; set; }
    public string? BANK_NO { get; set; }
    public string? NOTE { get; set; }
    public bool? IS_ACTIVE { get; set; }

}
