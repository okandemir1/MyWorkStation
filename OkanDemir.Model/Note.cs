namespace OkanDemir.Model
{
    using System;

    public class Note : Base.BaseEntityWithDate
    {
        public int UserId { get; set; }
        public string Description { get; set; }
        public bool IsAlert { get; set; }
        public DateTime AlertTime { get; set; }
        public bool IsImportant { get; set; }
        public bool SendSms { get; set; }
    }
}
