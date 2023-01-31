namespace OkanDemir.Model
{
    public class TodoImage : Base.BaseEntityWithDate
    {
        public int UserId { get; set; }
        public string ImagePath { get; set; }
        public int TodoListId { get; set; }
        public int TodoProjectId { get; set; }
    }
}
