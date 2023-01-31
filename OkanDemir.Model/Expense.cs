namespace OkanDemir.Model
{
    using System;

    public class Expense : Base.BaseEntityWithDate
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string FilePath { get; set; }
        public bool HasPayment { get; set; }
    }
}
