namespace OkanDemir.Model
{
    using System;

    public class Hashtag : Base.BaseEntityWithDate
    {
        public int UserId { get; set; }
        public string Name { get; set; }
    }
}
