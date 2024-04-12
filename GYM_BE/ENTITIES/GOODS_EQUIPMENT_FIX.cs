using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.Entities
{
    [Table("GOODS_EQUIPMENT_FIX")]
    public class GOODS_EQUIPMENT_FIX : BASE_ENTITY
    {
       public int? TYPE_ID  { get; set; }

       public long? RESULT_ID  { get; set; }

       public long? EMPLOYEE_ID  { get; set; }

       public long? EQUIPMENT_ID  { get; set; }

       public string? NOTE  { get; set; }

       public decimal? MONEY  { get; set; }

       public DateTime? EXPECTED_USE_TIME  { get; set; }

       public DateTime? START_DATE  { get; set; }

       public DateTime? END_DATE  { get; set; }


    }
}

