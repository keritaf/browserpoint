namespace Itransition.BrowserPoint.Model.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Presentation entity.
    /// </summary>
    public class Presentation
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Presentation title.
        /// </summary>
        [Required]
        public String Title { get; set; }

        /// <summary>
        /// Presentation description.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Presentation owner.
        /// This user has full control over presentation attributes and content.
        /// </summary>
        public User Owner { get; set; }
    }
}
