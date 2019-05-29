using BLL.BLL;
using BLL.Interfaces;
using Model.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Oblig1.Controllers
{
    /// <summary>
    /// Controller for managing generation/viewing of menu objects.
    /// </summary>
    public class MenuController : Controller
    {
        private IMenuBLL _menuLogic;

        public MenuController()
        {
            this._menuLogic = new MenuBLL();
        }

        public MenuController(IMenuBLL menuStub)
        {
            this._menuLogic = menuStub;
        }

        /// <summary>
        /// Lager en liste av sjangerne i databasetabellen Sjangere.
        /// Sender så listen inn i GetDropdownList.cshtml og returnerer
        /// partialView'et som genereres der.
        /// Creates a list of the genres in the database table "Genres". The list is then
        /// sent to a partial view.
        /// </summary>
        /// <returns>A partial view that contains a list of action links that are used in dropdown menus</returns>
        public PartialViewResult GetDropdownList()
        {
            List<GenreVM> list = _menuLogic.GetDropdownList(2);
            return PartialView("GetDropdownList", list);
        }
    }
}