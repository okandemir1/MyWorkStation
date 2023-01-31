namespace OkanDemir.Model
{
    using System;

    public class Metion : Base.BaseEntityWithDate
    {
        public int UserId { get; set; }
        public string Name { get; set; }
    }
}
