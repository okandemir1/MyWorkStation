using System.ComponentModel.DataAnnotations.Schema;

namespace OkanDemir.Dto
{
    public class SubscriptionTypeDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
