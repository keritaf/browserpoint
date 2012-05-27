namespace Itransition.BrowserPoint.Model.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// User-added image urls.
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Image id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Image URI address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// User, who added image link.
        /// </summary>
        public virtual User User { get; set; }
    }
}
