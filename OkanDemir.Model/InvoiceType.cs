namespace OkanDemir.Model
{
    public class InvoiceType : Base.BaseEntityWithDate
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ContractNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
