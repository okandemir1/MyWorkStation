using System.ComponentModel.DataAnnotations.Schema;

namespace OkanDemir.Dto
{
    public class SubscriptionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SubscriptionTypeId { get; set; }
        public string SubscriptionTypeName { get; set; }
        public string FilePath { get; set; }
        public decimal Price { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool HasPayment { get; set; }
    }
}
