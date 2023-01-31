namespace OkanDemir.Model
{
    public class ArchiveCategory : Base.BaseEntityWithDate
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
