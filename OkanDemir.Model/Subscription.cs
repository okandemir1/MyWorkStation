namespace OkanDemir.Model
{
    using System;

    public class Subscription : Base.BaseEntityWithDate
    {
        public int UserId { get; set; }
        public int SubscriptionTypeId { get; set; }
        public virtual SubscriptionType SubscriptionType { get; set; }
        public string FilePath { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool HasPayment { get; set; }
    }
}
