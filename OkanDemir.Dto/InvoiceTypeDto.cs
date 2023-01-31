using System.ComponentModel.DataAnnotations.Schema;

namespace OkanDemir.Dto
{
    public class InvoiceTypeDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ContractNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
