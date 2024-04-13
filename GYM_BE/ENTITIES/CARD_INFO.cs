using GYM_BE.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.ENTITIES;
[Table("CARD_INFO")]
public class CARD_INFO : BASE_ENTITY
{
    public string? CODE { get; set; }
    public DateTime? EFFECTED_DATE { get; set; }
    public DateTime? EXPIRED_DATE { get; set; }
    public long? CARD_TYPE_ID { get; set; }
    public long? CUSTOMER_ID { get; set; }
    public long? LOCKER_ID { get; set; }
    public string? NOTE { get; set; }
    public bool? IS_ACTIVE { get; set; }
    public DateTime? TIME_START { get; set; }
    public DateTime? TIME_END { get; set; }
}
