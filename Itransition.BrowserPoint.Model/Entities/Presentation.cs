namespace Itransition.BrowserPoint.Model.Entities
{
    using System;
    using System.Collections.Generic;
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
        public virtual User Author { get; set; }

        /// <summary>
        /// Users, who follow this presentation.
        /// </summary>
        public virtual ICollection<User> Followers { get; set; }

        [InverseProperty("Presentation")]
        public virtual ICollection<Tag> Tags { get; set; }

        [InverseProperty("Presentations")]
        public virtual PresentationTheme Theme { get; set; }
    }
}
