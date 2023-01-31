namespace OkanDemir.Model
{
    using System;

    public class CodeNote : Base.BaseEntityWithDate
    {
        public int UserId { get; set; }
        public int CodeCategoryId { get; set; }
        public virtual CodeCategory CodeCategory { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
    }
}
