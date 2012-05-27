namespace Itransition.BrowserPoint.Web.Models
{
    /// <summary>
    /// Model of error page (AppError view).
    /// </summary>
    public class ErrorModel
    {
        /// <summary>
        /// Error title. Displayed as view header.
        /// Use it to give basic understanding of what happened.
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Error message can be used to further describe the error and situation, which caused it.
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// Error details should contain recommendations on how to react and overcome the problem.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Flag, set to <c>true</c> when a link to the main page should be added to the error page.
        /// </summary>
        public bool ShowGotoMain { get; set; }

        /// <summary>
        /// Flag, set to <c>true</c> when a link to the previous page should be added to the error page.
        /// </summary>
        public bool ShowGotoBack { get; set; }
    }
}