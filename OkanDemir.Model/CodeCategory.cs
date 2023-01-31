namespace OkanDemir.Model
{
    using System;

    public class CodeCategory : Base.BaseEntityWithDate
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
