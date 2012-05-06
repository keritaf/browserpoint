namespace Itransition.BrowserPoint.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using DataAnnotationsExtensions;

    /// <summary>
    /// Service user entity.
    /// </summary>
    public class User
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// User name, is used to login.
        /// </summary>
        [Required]
        public String Name { get; set; }

        /// <summary>
        /// User email, used for sending mail to user.
        /// Account confirmation link is sent to this address too.
        /// </summary>
        [Required]
        [Email]
        public String Email { get; set; }

        /// <summary>
        /// Password hash, salt is added automatically by BCrypt.
        /// </summary>
        [Required]
        public String PasswordHash { get; set; }

        /// <summary>
        /// Hash, used for account activation using email.
        /// </summary>
        public String EmailConfirmationHash { get; set; }

        /// <summary>
        /// Flag, showing status of email confirmation.
        /// Set to <c>true</c> when user follows the link in confirmation email.
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// User account lock flag.
        /// Is set to <c>true</c> when administrator locks user.
        /// </summary>
        public bool IsLocked { get; set; }


        /// <summary>
        /// Date and time, when user account was created.
        /// </summary>
        [Column(TypeName = "datetime2")]
        public DateTime Created { get; set; }

        /// <summary>
        /// Last login time.
        /// </summary>
        [Column(TypeName = "datetime2")]
        public DateTime LastLogin { get; set; }

        /// <summary>
        /// User last activity time. Used to count online members count.
        /// </summary>
        [Column(TypeName = "datetime2")]
        public DateTime LastActivity { get; set; }

        /// <summary>
        /// Last user account lockout time.
        /// </summary>
        [Column(TypeName = "datetime2")]
        public DateTime LastLockout { get; set; }

        /// <summary>
        /// User roles.
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; }
    }
}
