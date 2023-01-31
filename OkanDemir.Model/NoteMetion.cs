namespace OkanDemir.Model
{
    using System;

    public class NoteMetion : Base.BaseEntityWithDate
    {
        public int UserId { get; set; }
        public int NoteId { get; set; }
        public int MetionId { get; set; }
    }
}
