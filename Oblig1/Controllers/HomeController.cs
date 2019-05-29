using System.Web.Mvc;

namespace Oblig1.Controllers
{
    /// <summary>
    /// Controller for any general views.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// General message view, that shows a header and if any corresponding error messages.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="message">Error message with font colored red.</param>
        /// <returns>A view with a simple message.</returns>
        public ActionResult ShowMessage(string header, string message)
        {
            ViewBag.Header = header;
            ViewBag.Message = message;
            return View();
        }

        /// <summary>
        /// Our contact information
        /// </summary>
        /// <returns></returns>
        public ActionResult ContactInformation()
        {
            return View();
        }
    }
}