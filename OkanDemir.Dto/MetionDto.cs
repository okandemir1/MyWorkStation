using System.ComponentModel.DataAnnotations.Schema;

namespace OkanDemir.Dto
{
    public class MetionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
    }
}
