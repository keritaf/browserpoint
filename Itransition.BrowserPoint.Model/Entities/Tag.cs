namespace Itransition.BrowserPoint.Model.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Tag
    {
        /// <summary>
        /// Tag string.
        /// </summary>
        [Key]
        public virtual string Title { get; set; }

        public virtual Presentation Presentation { get; set; }        
    }
}
