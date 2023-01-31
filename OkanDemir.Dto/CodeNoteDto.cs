namespace OkanDemir.Dto
{
    public class CodeNoteDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CodeCategoryId { get; set; }
        public string CodeCategoryName { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
    }
}
