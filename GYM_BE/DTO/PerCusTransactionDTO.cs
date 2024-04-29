namespace GYM_BE.DTO
{
    public class PerCusTransactionDTO : BaseDTO
    {
       public long? CustomerId  { get; set; }

       public long? TransForm  { get; set; }

       public string? Code  { get; set; }

       public DateTime? TransDate  { get; set; }


    }
}

