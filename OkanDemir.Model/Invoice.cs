namespace OkanDemir.Model
{
    using System;

    public class Invoice : Base.BaseEntityWithDate
    {
        public int UserId { get; set; }
        public int InvoiceTypeId { get; set; }
        public virtual InvoiceType InvoiceType { get; set; }
        public decimal Price { get; set; }
        public DateTime LastPaymentDate { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoicePaymentDate { get; set; }
        public string InvoiceFile { get; set; }
        public bool HasPayment { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
