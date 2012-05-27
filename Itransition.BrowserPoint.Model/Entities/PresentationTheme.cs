namespace Itransition.BrowserPoint.Model.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class PresentationTheme
    {
        [Key]
        public int Id { get; set; }

        public virtual string Title { get; set; }

        public virtual string ThemeFolder { get; set; }

        public virtual ICollection<PresentationThemeSlide> PresentationThemeSlides { get; set; }

        public virtual ICollection<Presentation> Presentations { get; set; }
    }
}
