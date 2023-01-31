namespace OkanDemir.Model
{
    using System;

    public class NoteHashtag : Base.BaseEntityWithDate
    {
        public int UserId { get; set; }
        public int NoteId { get; set; }
        public int HashtagId { get; set; }
    }
}
