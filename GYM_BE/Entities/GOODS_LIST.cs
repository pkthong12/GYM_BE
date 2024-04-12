using System.ComponentModel.DataAnnotations.Schema;

namespace GYM_BE.Entities
{
    [Table("GOODS_LIST")]
    public class GOODS_LIST : BASE_ENTITY
    {
       public long? QUANTITY  { get; set; }

       public long? MEASURE_ID  { get; set; }

       public long? IMPORT_PRICE  { get; set; }

       public long? PRICE  { get; set; }

       public long? PRODUCT_TYPE_ID  { get; set; }

       public bool? IS_ACTIVE  { get; set; }

       public long? STOCK_ID  { get; set; }

       public string? NOTE  { get; set; }

       public string? CODE  { get; set; }

       public string? NAME  { get; set; }

       public string? SUPPLIER  { get; set; }

       public DateTime? EXPIRE_DATE  { get; set; }

       public DateTime? INPUT_DAY  { get; set; }


    }
}

