namespace Itransition.BrowserPoint.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// User role entity.
    /// </summary>
    public class Role
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Role name.
        /// </summary>
        [Required]
        public String Name { get; set; }

        /// <summary>
        /// Role description.
        /// Displayed in user administration section.
        /// </summary>
        public String Descriptioin { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
