using BLL.BLL;
using BLL.Interfaces;
using Model.ViewModels;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace Oblig1.Controllers
{
    /// <summary>
    /// Controller that is related to the user objects; creation, log in and log out.
    /// </summary>
    public class UserController : Controller
    {
        private IUserBLL _userBLL;

        public UserController()
        {
            _userBLL = new UserBLL();
        }

        public UserController(IUserBLL stub)
        {
            _userBLL = stub;
        }

        /// <summary>
        /// Gets a view for registration of the user.
        /// </summary>
        /// <returns>Form for user registration</returns>
        public ActionResult RegisterUser()
        {
            return View();
        }

        /// <summary>
        /// Accepts a View-Model representation of the corresponding user that is used to
        /// create a database row corresponding to one user.
        /// </summary>
        /// <param name="inUser">User information to be entered into the database table</param>
        /// <returns>View with feedback from the database</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterUser(UserVM inUser)
        {
            if (ModelState.IsValid)
            {
                var result = _userBLL.CreateUser(inUser);

                return result == ""
                    ? RedirectToAction("ShowMessage", "Home", new { header = "Takk for at du registrerte deg hos oss!", message = "" })
                    : RedirectToAction("ShowMessage", "Home", new { header = "Noe gikk galt under opprettelsen av kunden!", message = result });
            }

            return View();
        }

        /// <summary>
        /// Gets the view for logging in users.
        /// </summary>
        /// <returns>Form with e-mail and password field</returns>
        [ChildActionOnly]
        public ActionResult LogOnModal()
        {
            return PartialView("LogOnModal");
        }

        /// <summary>
        /// Accepts log in information, checks e-mail address and password with the supplied e-mail and
        /// password. If successful, a session variable with the corresponding user is created. If
        /// the information is not valid, an error message is returned to the view.
        /// </summary>
        /// <param name="userLogin">Contains the e-mail address and password provided by the user</param>
        /// <returns>The previous page if successful or the same view if the provided
        /// information is invalid</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOnModal(LogInVM userLogin)
        {
            if (ModelState.IsValid)
            {
                UserVM user = _userBLL.UserLogIn(userLogin);

                if (user != null)
                {
                    Session["Kunde"] = user;

                    if (user.Admin)
                    {
                        return Json(JsonConvert.SerializeObject("admin"));
                    }
                    return Redirect(Request.UrlReferrer.PathAndQuery);
                }
            }
            string result = userLogin.FindEmptyFields(userLogin);

            if (result != null)
            {
                return Json(JsonConvert.SerializeObject(result));
            }
            return Json(JsonConvert.SerializeObject("Feil brukernavn eller passord! Brukeren kan også være inaktiv."));
        }

        /// <summary>
        /// Logs out user by setting the user session variable equal to null. It also
        /// clears the shopping cart by setting its session variable equal to null.
        /// </summary>
        /// <returns>The previous view</returns>
        public ActionResult LogOutUser()
        {
            Session["Kunde"] = null;
            Session["ShoppingCart"] = null;
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        /// <summary>
        /// Checks the e-mail address dynamically with the database when the user
        /// enters an e-mail address in the form.
        /// </summary>
        /// <param name="email">The e-mail provided in the form</param>
        /// <returns>A JSON serialized string, possibly containing an error message</returns>
        [HttpPost]
        public ActionResult CheckEmail(string email)
        {
            bool result = _userBLL.FindEmail(email);
            return Json(JsonConvert.SerializeObject(result));
        }

        /// <summary>
        /// Returns a view for changing the password.
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassWord()
        {
            return View(new UserVM());
        }

        /// <summary>
        /// If the provided e-mail address is found, the user is
        /// redirected to another view to check the input specified by the user.
        /// If the e-mail address is not found, an error message is shown to the user.
        /// </summary>
        /// <param name="user">User to change password</param>
        /// <returns>A redirect to the appropriate view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassWord(UserVM user)
        {
            if (_userBLL.FindEmail(user.Email))
            {
                return RedirectToAction("CheckUser", user);
            }
            else
            {
                return RedirectToAction("ShowMessage", "Home", new { header = "Email adressen er feil", message = "" });
            }
        }

        /// <summary>
        /// Verifies information provided by the user. If the information is correct,
        /// the user is redirected to the view for resetting password. If not, an error
        /// message is shown.
        /// </summary>
        /// <param name="user">The user to check</param>
        /// <returns>A view/redirect depending on the input</returns>
        public ActionResult CheckUser(UserVM user)
        {
            if (user.FirstName == null)
            {
                return View(user);
            }

            //Check FirstName, SurName and PhoneNR
            if (_userBLL.CheckPersonalia(user))
            {
                return RedirectToAction("ResetPassWord", user);
            }
            else
            {
                return RedirectToAction("ShowMessage", "Home", new { header = "Noe gikk feil", message = "Pass på at all personalien er korrekt og prøv igjen." });
            }
        }

        /// <summary>
        /// Resets the password for the user and returns a redirect with a message if
        /// successful, if not, the same view is shown.
        /// </summary>
        /// <param name="user">The user to reset password</param>
        /// <returns>A view or redirect depending on the input</returns>
        public ActionResult ResetPassWord(UserVM user)
        {
            if (user.PassWord == null)
            {
                return View(user);
            }

            if (user.PassWord == user.PassWordRepeat)
            {
                if (_userBLL.ResetPassword(user))
                {
                    return RedirectToAction("ShowMessage", "Home", new { header = "Passord er nå resatt", message = "Du kan nå logge på igjen med din bruker, med nytt passord." });
                }
            }
            return View(user);
        }
    }
}