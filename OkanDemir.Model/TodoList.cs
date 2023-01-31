using System;

namespace OkanDemir.Model
{
    public class TodoList : Base.BaseEntityWithDate
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }

        public int TodoProjectId { get; set; }
        public virtual TodoProject TodoProject { get; set; }

        public int TodoTableId { get; set; }
        public virtual TodoTable TodoTable { get; set; }

        public int TodoListTypeId { get; set; }
        public virtual TodoListType TodoListType { get; set; }

        public DateTime CreateDate { get; set; }

        public int Status { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
