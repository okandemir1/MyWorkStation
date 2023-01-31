namespace OkanDemir.Model
{
    public class TodoProject : Base.BaseEntityWithDate
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
