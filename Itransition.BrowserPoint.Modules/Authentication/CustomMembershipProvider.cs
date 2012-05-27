namespace Itransition.BrowserPoint.Modules.Authentication
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Security;
    using BCrypt.Net;
    using Itransition.BrowserPoint.Model;
    using Itransition.BrowserPoint.Model.Entities;

    /// <summary>
    /// Membership provider for WebReg context
    /// </summary>
    public sealed class CustomMembershipProvider : MembershipProvider
    {
        private readonly ModelContext context = new ModelContext();

        private const String EmailRegex =
            @"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";

        public CustomMembershipProvider()
        {
            ApplicationName = @"Itransition.Course.WebReg";
        }
        /// <summary>
        /// Adds a new membership user to the data source.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser"/> object populated with the information for the newly created user.
        /// </returns>
        /// <param name="username">The user name for the new user. </param>
        /// <param name="password">The password for the new user. </param>
        /// <param name="email">The e-mail address for the new user.</param>
        /// <param name="passwordQuestion">The password question for the new user.</param>
        /// <param name="passwordAnswer">The password answer for the new user</param>
        /// <param name="isApproved">Whether or not the new user is approved to be validated.</param>
        /// <param name="providerUserKey">The unique identifier from the membership data source for the user.</param>
        /// <param name="status">A <see cref="T:System.Web.Security.MembershipCreateStatus"/> enumeration value indicating whether the user was created successfully.</param>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            if (String.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be null or empty", "username");
            }
            if (String.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password cannot be null or empty", "password");
            }
            if (String.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty", "email");
            }

            if (context.Users.Count(t => t.Name == username) != 0)
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }
            if (context.Users.Count(t => t.Email == email) != 0)
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }
            if (!ValidatePassword(password))
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            if (!ValidateEmail(email))
            {
                status = MembershipCreateStatus.InvalidEmail;
                return null;
            }

            var user = new User
            {
                Name = username,
                PasswordHash = BCrypt.HashPassword(password),
                Email = email,
                Created = DateTime.UtcNow,
                LastActivity = new DateTime(1980, 1, 1),
                LastLockout = new DateTime(1980, 1, 1),
                LastLogin = new DateTime(1980, 1, 1),
                IsEmailConfirmed = isApproved
            };
            context.Users.Add(user);
            context.SaveChanges();

            status = MembershipCreateStatus.Success;
            return ConvertUserToMembershipUser(user);
        }

        private static bool ValidateEmail(String email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                return false;
            }
            var emailValidator = new Regex(EmailRegex, RegexOptions.IgnoreCase);
            return emailValidator.IsMatch(email);
        }

        private bool ValidatePassword(String password)
        {
            if (String.IsNullOrWhiteSpace(password))
                return false;
            if (password.Length < MinRequiredPasswordLength)
                return false;
            var passwordValidator = new Regex(@"\W");
            if (passwordValidator.Matches(password).Count < MinRequiredNonAlphanumericCharacters)
                return false;
            passwordValidator = new Regex(PasswordStrengthRegularExpression);
            return passwordValidator.IsMatch(password);
        }

        /// <summary>
        /// Processes a request to update the password question and answer for a membership user.
        /// </summary>
        /// <returns>
        /// true if the password question and answer are updated successfully; otherwise, false.
        /// </returns>
        /// <param name="username">The user to change the password question and answer for. </param><param name="password">The password for the specified user. </param><param name="newPasswordQuestion">The new password question for the specified user. </param><param name="newPasswordAnswer">The new password answer for the specified user. </param>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new InvalidOperationException("Password question change is not allowed for this provider");
        }

        /// <summary>
        /// Gets the password for the specified user name from the data source.
        /// </summary>
        /// <returns>
        /// The password for the specified user name.
        /// </returns>
        /// <param name="username">The user to retrieve the password for. </param>
        /// <param name="answer">The password answer for the user. </param>
        public override string GetPassword(string username, string answer)
        {
            throw new InvalidOperationException("GetPassword is not allowed for this provider");
        }

        /// <summary>
        /// Processes a request to update the password for a membership user.
        /// </summary>
        /// <returns>
        /// true if the password was updated successfully; otherwise, false.
        /// </returns>
        /// <param name="username">The user to update the password for. </param>
        /// <param name="oldPassword">The current password for the specified user. </param>
        /// <param name="newPassword">The new password for the specified user. </param>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var user = context.Users.SingleOrDefault(t => t.Name == username);
            if (user == null)
            {
                return false;
            }
            if (!ValidateContextUser(user, oldPassword))
                return false;
            user.PasswordHash = BCrypt.HashPassword(newPassword);
            context.SaveChanges();
            return true;
        }

        private bool ValidateContextUser(User user, string password)
        {
            if (user == null || String.IsNullOrWhiteSpace(password))
            {
                return false;
            }
            return BCrypt.Verify(password, user.PasswordHash);
        }

        /// <summary>
        /// Resets a user's password to a new, automatically generated password.
        /// </summary>
        /// <returns>
        /// The new password for the specified user.
        /// </returns>
        /// <param name="username">The user to reset the password for. </param><param name="answer">The password answer for the specified user. </param>
        public override string ResetPassword(string username, string answer)
        {
            throw new InvalidOperationException("Password reset is not allowed for this provider.");
        }

        /// <summary>
        /// Updates information about a user in the data source.
        /// </summary>
        /// <param name="user">A <see cref="T:System.Web.Security.MembershipUser"/> object that represents the user to update and the updated information for the user. </param>
        public override void UpdateUser(MembershipUser user)
        {
            int userId = user.ProviderUserKey is int ? (int)user.ProviderUserKey : 0;
            if (userId == 0)
            {
                return;
            }
            var contextUser = context.Users.SingleOrDefault(t => t.Id == userId);
            if (contextUser == null)
            {
                return;
            }
            contextUser.IsLocked = user.IsLockedOut;
            contextUser.IsEmailConfirmed = user.IsApproved;
            contextUser.LastActivity = user.LastActivityDate;
            contextUser.LastLockout = user.LastLockoutDate;
            contextUser.LastLogin = user.LastLoginDate;
            contextUser.Created = user.CreationDate;
        }

        /// <summary>
        /// Verifies that the specified user name and password exist in the data source.
        /// </summary>
        /// <returns>
        /// true if the specified username and password are valid; otherwise, false.
        /// </returns>
        /// <param name="username">The name of the user to validate. </param><param name="password">The password for the specified user. </param>
        public override bool ValidateUser(string username, string password)
        {
            var user = context.Users.SingleOrDefault(t => t.Name == username);
            if (user == null)
            {
                return false;
            }
            return BCrypt.Verify(password, user.PasswordHash);
        }

        /// <summary>
        /// Clears a lock so that the membership user can be validated.
        /// </summary>
        /// <returns>
        /// true if the membership user was successfully unlocked; otherwise, false.
        /// </returns>
        /// <param name="userName">The membership user whose lock status you want to clear.</param>
        public override bool UnlockUser(string userName)
        {
            var user = context.Users.SingleOrDefault(t => t.Name == userName);
            if (user == null)
                return false;
            user.IsLocked = false;
            context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Gets user information from the data source based on the unique identifier for the membership user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser"/> object populated with the specified user's information from the data source.
        /// </returns>
        /// <param name="providerUserKey">The unique identifier for the membership user to get information for.</param><param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            int key = providerUserKey is int ? (int)providerUserKey : 0;
            if (key == 0)
                return null;
            var user = context.Users.SingleOrDefault(t => t.Id == key);
            if (user == null)
                return null;
            if (userIsOnline)
            {
                user.LastActivity = DateTime.UtcNow;
                context.SaveChanges();
            }
            return ConvertUserToMembershipUser(user);
        }

        /// <summary>
        /// Gets information from the data source for a user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser"/> object populated with the specified user's information from the data source.
        /// </returns>
        /// <param name="username">The name of the user to get information for. </param><param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user. </param>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var user = context.Users.SingleOrDefault(t => t.Name == username);
            if (user == null)
            {
                return null;
            }
            if (userIsOnline)
            {
                user.LastActivity = DateTime.UtcNow;
                context.SaveChanges();
            }
            return ConvertUserToMembershipUser(user);
        }

        /// <summary>
        /// Gets the user name associated with the specified e-mail address.
        /// </summary>
        /// <returns>
        /// The user name associated with the specified e-mail address. If no match is found, return null.
        /// </returns>
        /// <param name="email">The e-mail address to search for. </param>
        public override string GetUserNameByEmail(string email)
        {
            var user = context.Users.SingleOrDefault(t => t.Email == email);
            return user == null ? null : user.Name;
        }

        /// <summary>
        /// Removes a user from the membership data source. 
        /// </summary>
        /// <returns>
        /// true if the user was successfully deleted; otherwise, false.
        /// </returns>
        /// <param name="username">The name of the user to delete.</param><param name="deleteAllRelatedData">true to delete data related to the user from the database; false to leave data related to the user in the database.</param>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            var userToDelete = context.Users.SingleOrDefault(t => t.Name == username);
            if (userToDelete != null)
            {
                context.Users.Remove(userToDelete);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a collection of all the users in the data source in pages of data.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection"/> collection that contains a page of <paramref name="pageSize"/><see cref="T:System.Web.Security.MembershipUser"/> objects beginning at the page specified by <paramref name="pageIndex"/>.
        /// </returns>
        /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex"/> is zero-based.</param><param name="pageSize">The size of the page of results to return.</param><param name="totalRecords">The total number of matched users.</param>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = context.Users.Count();
            var users = context.Users.OrderBy(t => t.Name).Skip(pageIndex * pageSize).Take(pageSize);
            var collection = new MembershipUserCollection();
            foreach (var user in users)
            {
                collection.Add(ConvertUserToMembershipUser(user));
            }
            return collection;
        }

        /// <summary>
        /// Gets the number of users currently accessing the application.
        /// </summary>
        /// <returns>
        /// The number of users currently accessing the application.
        /// </returns>
        public override int GetNumberOfUsersOnline()
        {
            var onlineStartingTime = DateTime.UtcNow.Subtract(UserIsOnlineTime);
            return context.Users.Count(t => t.LastActivity > onlineStartingTime);
        }

        private TimeSpan UserIsOnlineTime
        {
            get { return new TimeSpan(0, 10, 0); }
        }

        /// <summary>
        /// Gets a collection of membership users where the user name contains the specified user name to match.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection"/> collection that contains a page of <paramref name="pageSize"/><see cref="T:System.Web.Security.MembershipUser"/> objects beginning at the page specified by <paramref name="pageIndex"/>.
        /// </returns>
        /// <param name="usernameToMatch">The user name to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex"/> is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var users = context.Users.Where(t => t.Name == usernameToMatch);
            totalRecords = users.Count();
            var page = users.OrderBy(t => t.Name).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable();
            var collection = new MembershipUserCollection();
            foreach (var user in page)
            {
                collection.Add(ConvertUserToMembershipUser(user));
            }
            return collection;
        }

        /// <summary>
        /// Gets a collection of membership users where the e-mail address contains the specified e-mail address to match.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection"/> collection that contains a page of <paramref name="pageSize"/><see cref="T:System.Web.Security.MembershipUser"/> objects beginning at the page specified by <paramref name="pageIndex"/>.
        /// </returns>
        /// <param name="emailToMatch">The e-mail address to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex"/> is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var users = context.Users.Where(t => t.Email == emailToMatch);
            totalRecords = users.Count();
            var page = users.OrderBy(t => t.Name).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable();
            var collection = new MembershipUserCollection();
            foreach (var user in page)
            {
                collection.Add(ConvertUserToMembershipUser(user));
            }
            return collection;
        }

        private MembershipUser ConvertUserToMembershipUser(User user)
        {
            return new MembershipUser(
                providerName: GetType().Name,
                providerUserKey: user.Id,
                creationDate: user.Created,
                name: user.Name,
                email: user.Email,
                passwordQuestion: String.Empty,
                comment: String.Empty,
                isApproved: user.IsEmailConfirmed,
                isLockedOut: user.IsLocked,
                lastActivityDate: user.LastActivity,
                lastLoginDate: user.LastLogin,
                lastLockoutDate: user.LastLockout,
                lastPasswordChangedDate: user.Created
                );
        }

        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to retrieve their passwords.
        /// </summary>
        /// <returns>
        /// true if the membership provider is configured to support password retrieval; otherwise, false. The default is false.
        /// </returns>
        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }

        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to reset their passwords.
        /// </summary>
        /// <returns>
        /// true if the membership provider supports password reset; otherwise, false. The default is true.
        /// </returns>
        public override bool EnablePasswordReset
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether the membership provider is configured to require the user to answer a password question for password reset and retrieval.
        /// </summary>
        /// <returns>
        /// true if a password answer is required for password reset and retrieval; otherwise, false. The default is true.
        /// </returns>
        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }

        /// <summary>
        /// The name of the application using the custom membership provider.
        /// </summary>
        /// <returns>
        /// The name of the application using the custom membership provider.
        /// </returns>
        public override string ApplicationName { get; set; }

        /// <summary>
        /// Gets the number of invalid password or password-answer attempts allowed before the membership user is locked out.
        /// </summary>
        /// <returns>
        /// The number of invalid password or password-answer attempts allowed before the membership user is locked out.
        /// </returns>
        public override int MaxInvalidPasswordAttempts
        {
            get { return 3; }
        }

        /// <summary>
        /// Gets the number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.
        /// </summary>
        /// <returns>
        /// The number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.
        /// </returns>
        public override int PasswordAttemptWindow
        {
            get { return 5; }
        }

        /// <summary>
        /// Gets a value indicating whether the membership provider is configured to require a unique e-mail address for each user name.
        /// </summary>
        /// <returns>
        /// true if the membership provider requires a unique e-mail address; otherwise, false. The default is true.
        /// </returns>
        public override bool RequiresUniqueEmail
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating the format for storing passwords in the membership data store.
        /// </summary>
        /// <returns>
        /// One of the <see cref="T:System.Web.Security.MembershipPasswordFormat"/> values indicating the format for storing passwords in the data store.
        /// </returns>
        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Hashed; }
        }

        /// <summary>
        /// Gets the minimum length required for a password.
        /// </summary>
        /// <returns>
        /// The minimum length required for a password. 
        /// </returns>
        public override int MinRequiredPasswordLength
        {
            get { return 4; }
        }

        /// <summary>
        /// Gets the minimum number of special characters that must be present in a valid password.
        /// </summary>
        /// <returns>
        /// The minimum number of special characters that must be present in a valid password.
        /// </returns>
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets the regular expression used to evaluate a password.
        /// </summary>
        /// <returns>
        /// A regular expression used to evaluate a password.
        /// </returns>
        public override string PasswordStrengthRegularExpression
        {
            get { return @".+"; }
        }
    }
}
