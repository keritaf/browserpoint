namespace Itransition.BrowserPoint.Model.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class PresentationThemeSlide
    {
        [Key]
        public virtual PresentationTheme PresentationTheme { get; set; }

        [Key]
        public virtual string Type { get; set; }

        public virtual ICollection<SlideImage> StaticImages { get; set; }

        public virtual ICollection<SlideText> StaticTexts { get; set; }


    }
}
