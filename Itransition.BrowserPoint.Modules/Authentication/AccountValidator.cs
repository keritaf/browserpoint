namespace Itransition.BrowserPoint.Modules.Authentication
{
    using System;
    using System.Linq;
    using System.Text;
    using Itransition.BrowserPoint.Model;
    using Itransition.BrowserPoint.Model.Entities;
    using Itransition.Course.WebReg.Modules.Extensions;

    public class AccountValidator
    {
        private readonly ModelContext context = new ModelContext();

        private string GenerateValidationCode(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user", "User cannot be null");
            }
            var userConfirmationStringBuilder = new StringBuilder();
            userConfirmationStringBuilder.Append(user.Id);
            userConfirmationStringBuilder.Append(user.Name);
            userConfirmationStringBuilder.Append(user.Email);
            userConfirmationStringBuilder.Append(user.Created.ToBinary());
            userConfirmationStringBuilder.Append(DateTime.UtcNow.ToBinary());
            var confirmationHash = userConfirmationStringBuilder.ToString().HashMD5();
            user.EmailConfirmationHash = confirmationHash;
            context.SaveChanges();
            return confirmationHash;
        }

        public string GenerateValidationCode(string email)
        {
            var user = context.Users.SingleOrDefault(t => t.Email == email);
            if (user==null)
                throw new ArgumentException("User with this email cannot be found","email");
            return GenerateValidationCode(user);
        }
    }
}
