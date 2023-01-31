using System.ComponentModel.DataAnnotations.Schema;

namespace OkanDemir.Dto
{
    public class ArchiveDto
    {
        public int Id { get; set; }
        public int ArchiveCategoryId { get; set; }
        public int UserId { get; set; }
        public string Domain { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string BirthDate { get; set; }
        public bool IsActive { get; set; }
        
        [NotMapped]
        public string Key { get; set; }
    }
}
