using System.ComponentModel.DataAnnotations.Schema;

namespace OkanDemir.Dto
{
    public class NoteDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public bool IsAlert { get; set; }
        public DateTime AlertTime { get; set; }
        public bool IsImportant { get; set; }
        public bool SendSms { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
