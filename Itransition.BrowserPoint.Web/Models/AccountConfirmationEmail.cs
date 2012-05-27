namespace Itransition.BrowserPoint.Web.Models
{
    using Postal;

    public class AccountConfirmationEmail:Email
    {
        public string To { get; set; }
        public string Token { get; set; }
    }
}