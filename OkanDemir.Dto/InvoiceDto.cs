using System.ComponentModel.DataAnnotations.Schema;

namespace OkanDemir.Dto
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int InvoiceTypeId { get; set; }
        public string InvoiceTypeName { get; set; }
        public decimal Price { get; set; }
        public DateTime LastPaymentDate { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoicePaymentDate { get; set; }
        public string? InvoiceFile { get; set; }
        public bool HasPayment { get; set; }
        public bool IsActive { get; set; }
    }
}
