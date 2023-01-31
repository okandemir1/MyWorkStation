using System.ComponentModel.DataAnnotations.Schema;

namespace OkanDemir.Dto
{
    public class IncomeTypeDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Period { get; set; }
        public decimal TotalPrice { get; set; }
        public string ContactName { get; set; }
        public string ContactMail { get; set; }
        public string ContactPhone { get; set; }
        public bool IsActive { get; set; }
    }
}
