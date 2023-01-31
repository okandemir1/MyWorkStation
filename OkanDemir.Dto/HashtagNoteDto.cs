using System.ComponentModel.DataAnnotations.Schema;

namespace OkanDemir.Dto
{
    public class HashtagNoteDto
    {
        public List<NoteDto> Notes { get; set; }
        public HashtagDto Hashtag { get; set; }
    }
}
