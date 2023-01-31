namespace OkanDemir.Model
{
    public class TodoListType : Base.BaseEntityWithDate
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
