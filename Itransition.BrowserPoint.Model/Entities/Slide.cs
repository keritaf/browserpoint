namespace Itransition.BrowserPoint.Model.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Slide
    {
        [Key]
        public virtual Presentation Presentation { get; set; }

        [Key]
        public int Order { get; set; }

        public virtual string Type { get; set; }

        public virtual ICollection<SlideText> Texts { get; set; }

        public virtual ICollection<SlideImage> Images { get; set; }
    }
}
