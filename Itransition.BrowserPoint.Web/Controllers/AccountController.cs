namespace Itransition.BrowserPoint.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;
    using Itransition.BrowserPoint.Model;
    using Itransition.BrowserPoint.Modules.Authentication;
    using Itransition.BrowserPoint.Web.Models;

    public class AccountController : Controller
    {
        private readonly ModelContext context = new ModelContext();

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    var user = context.Users.Single(t => t.Name == model.UserName.ToLower());
                    if (!user.IsEmailConfirmed)
                    {
                        return View("EmailNotConfirmed", user);
                    }
                    if (user.IsLocked)
                    {
                        return View("AppError",
                                    new ErrorModel
                                        {
                                            Title = "Account locked",
                                            Message = "Your account is locked by site administrator.",
                                            ShowGotoMain = true
                                        });
                    }

                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }
            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName.ToLower(), model.Password, model.Email, null, null, false, null,
                                      out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    return GenerateConfirmationEmail(model.Email);
                }
                ModelState.AddModelError("", ErrorCodeToString(createStatus));
            }

            return View(model);
        }

        public ViewResult GenerateConfirmationEmail(string email)
        {
            var user = context.Users.SingleOrDefault(t => t.Email == email);
            if (user != null)
            {
                var emailMessage = new AccountConfirmationEmail { To = email, Token = new AccountValidator().GenerateValidationCode(user.Email) };
                emailMessage.SendAsync();
                return View("EmailTokenSent", null, email);
            }
            return View("AppError",
                        new ErrorModel { Title = "User not found", Message = "User with provided email not found" });
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded = false;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    if (currentUser != null)
                    {
                        changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                    }
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }
                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
            }
            return View(model);
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult ConfirmEmail(string token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return View("ConfirmEmail");
            }
            var user = context.Users.SingleOrDefault(t => t.EmailConfirmationHash == token);
            if (user == null)
            {
                return View("AppError",
                            new ErrorModel { Title = "Wrong email token", Message = "The token is not found in our database" });
            }
            user.IsEmailConfirmed = true;
            context.SaveChanges();
            FormsAuthentication.SetAuthCookie(user.Name, false);
            return RedirectToAction("Index", "Home");
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return
                        "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return
                        "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return
                        "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return
                        "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
