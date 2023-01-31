using System.ComponentModel.DataAnnotations.Schema;

namespace OkanDemir.Dto
{
    public class IncomeDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int IncomeTypeId { get; set; }
        public decimal Price { get; set; }
        public DateTime PaymentDate { get; set; }
        public string FilePath { get; set; }
        public bool HasPayment { get; set; }
        public string IncomeTypeName { get; set; }
    }
}
