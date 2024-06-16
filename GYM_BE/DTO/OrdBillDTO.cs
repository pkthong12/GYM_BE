namespace GYM_BE.DTO
{
    public class OrdBillDTO : BaseDTO
    {
       public long? TypeTransfer  { get; set; }

       public long? CustomerId  { get; set; }

       public long? PerSellId  { get; set; }

       public long? MoneyHavePay  { get; set; }

       public long? TotalMoney  { get; set; }

       public long? PayMethod  { get; set; }

       public decimal? DiscPercent  { get; set; }
       public decimal? PercentVat { get; set; }
       public string? Code  { get; set; }
       public long? PkRef { get; set; }
       public long? VoucherId { get; set; }


    }
}

