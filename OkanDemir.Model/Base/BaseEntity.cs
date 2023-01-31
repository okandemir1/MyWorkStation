using System;
using System.ComponentModel.DataAnnotations;

namespace OkanDemir.Model.Base
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
    public abstract class BaseEntityWithDate : BaseEntity
    {
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; } = DateTime.Now;
    }
    public abstract class BaseEntityWithDeletable : BaseEntity
    {
        public bool IsDeletable { get; set; } = true;
    }
}