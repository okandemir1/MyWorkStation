using System.ComponentModel.DataAnnotations.Schema;

namespace OkanDemir.Dto
{
    public class MetionNoteDto
    {
        public List<NoteDto> Notes { get; set; }
        public MetionDto Metion { get; set; }
    }
}
