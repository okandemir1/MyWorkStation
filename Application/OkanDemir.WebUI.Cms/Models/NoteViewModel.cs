using OkanDemir.Dto;

namespace OkanDemir.WebUI.Cms.Models
{
    public class NoteViewModel
    {
        public NoteViewModel()
        {
            Notes = new List<NoteDto>();
            Note = new NoteDto();
        }

        public List<NoteDto> Notes { get; set; }
        public NoteDto Note { get; set; }
        public DateTime Date { get; set; }
    }
}
