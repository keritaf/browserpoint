namespace Itransition.BrowserPoint.Model.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class SlideText
    {
        [Key]
        public int Id { get; set; }

        public virtual string Type { get; set; }

        public virtual string Text { get; set; }

        public virtual BlockInfo BlockAttributes { get; set; }

        public virtual string Attributes { get; set; }
    }
}