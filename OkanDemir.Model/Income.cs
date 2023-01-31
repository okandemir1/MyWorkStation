namespace OkanDemir.Model
{
    using System;

    public class Income : Base.BaseEntityWithDate
    {
        public int UserId { get; set; }
        public int IncomeTypeId { get; set; }
        public virtual IncomeType IncomeType { get; set; }
        public decimal Price { get; set; }
        public DateTime PaymentDate { get; set; }
        public string FilePath { get; set; }
        public bool HasPayment { get; set; }
    }
}
