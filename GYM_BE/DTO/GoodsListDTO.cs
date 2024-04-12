namespace GYM_BE.DTO
{
    public class GoodsListDTO : BaseDTO
    {
       public long? Quantity  { get; set; }

       public long? MeasureId  { get; set; }

       public long? ImportPrice  { get; set; }

       public long? Price  { get; set; }

       public long? ProductTypeId  { get; set; }

       public bool? IsActive  { get; set; }

       public long? StockId  { get; set; }

       public string? Note  { get; set; }

       public string? Code  { get; set; }

       public string? Name  { get; set; }

       public string? Supplier  { get; set; }

       public DateTime? ExpireDate  { get; set; }

       public DateTime? InputDay  { get; set; }


    }
}

