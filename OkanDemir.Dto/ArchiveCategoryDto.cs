namespace OkanDemir.Dto
{
    public class ArchiveCategoryDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public bool IsActive { get; set; }
    }
}
