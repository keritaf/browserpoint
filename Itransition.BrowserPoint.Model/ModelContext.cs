namespace Itransition.BrowserPoint.Model
{
    using System.Data.Entity;
    using Itransition.BrowserPoint.Model.Entities;

    public class ModelContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Presentation> Presentations { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<SlideImage> SlideImages { get; set; }
        public DbSet<SlideText> SlideTexts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Image> Images { get; set; }

        public DbSet<PresentationTheme> Themes { get; set; }
        public DbSet<PresentationThemeSlide> ThemeSlides { get; set; }

    }
}
