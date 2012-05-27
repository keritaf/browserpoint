namespace Itransition.BrowserPoint.Model.Entities
{
    public class SlideImage
    {
        public int Id { get; set; }

        public virtual Image Image { get; set; }

        public virtual BlockInfo BlockAttributes { get; set; }

        public virtual string Attributes { get; set; }

    }
}