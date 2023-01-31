using OkanDemir.Model.Base;

namespace OkanDemir.Model
{
    public class Archive : BaseEntityWithDate
    {
        public int ArchiveCategoryId { get; set; }
        public int UserId { get; set; }
        public string Domain { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string BirthDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
