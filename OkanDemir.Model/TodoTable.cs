namespace OkanDemir.Model
{
    public class TodoTable : Base.BaseEntityWithDate
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public int TodoProjectId { get; set; }
        public int RankOrder { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
