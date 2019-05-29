using BLL.BLL;
using DAL.Stub;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.ViewModels;
using MvcContrib.TestHelper;
using Newtonsoft.Json;
using Oblig1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace UnitTest
{
    /// <summary>
    /// The AdminController has a quite big constructor where all the BLLs and Stubs
    /// are entered. This makes it so that we can reuse it for all the test methods.
    /// </summary>
    [TestClass]
    public class AdminControllerTest
    {
        private AdminController testAdminController = new AdminController(
            new ChangeLogBLL(new ChangeLogStub()),
            new ErrorLogBLL(new ErrlorLogStub()),
            new FilmBLL(new FilmStub(), new GenreStub()),
            new MenuBLL(new GenreStub(), new UserStub(), new FilmStub(), new OrderStub(), new ChangeLogStub()),
            new OrderBLL(new OrderStub()),
            new PriceBLL(new PriceStub()),
            new UserBLL(new UserStub())
            );

        /// <summary>
        /// Initializes the controller, such that values may be set.
        /// For example sessions.
        /// </summary>
        public TestControllerBuilder mock = new TestControllerBuilder();

        public AdminControllerTest()
        {
            mock.InitializeController(testAdminController);
        }

        public void SessionNull()
        {
            testAdminController.Session["Kunde"] = null;
        }

        public void NotAdmin()
        {
            testAdminController.Session["Kunde"] = new UserVM()
            {
                Id = 0
            };
        }

        public void AdminCheck(int id = 1)
        {
            testAdminController.Session["Kunde"] = new UserVM()
            {
                Id = id
            };
        }

        /// <summary>
        /// Adapted from: https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions-1/unit-testing/creating-unit-tests-for-asp-net-mvc-applications-cs
        /// </summary>
        /// <param name="result"></param>
        public void AssertAdminSessFail(RedirectToRouteResult result)
        {
            Assert.AreEqual(result.RouteValues["controller"], "Film");
            Assert.AreEqual(result.RouteValues["action"], "Frontpage");
        }

        public void AssertDBEXE(RedirectToRouteResult result)
        {
            Assert.AreEqual(result.RouteValues["action"], "AdminFrontPage");
        }

        //DBFAIL: stubs Throw a DatabaseErrorException.
        //NotAdmin: User  is not a admin.
        //SesssionNULL: If the user is not logged in.
        //NotValid: The validation of the models are wrong.

        //--------------------------------ADMIN

        #region Admin Test Methods

        [TestMethod]
        public void AdminFrontPage_View_OK()
        {
            //Arrange
            AdminCheck();
            AdminFrontPageVM expected = new AdminFrontPageVM()
            {
                UserCount = 10,
                FilmCount = 20,
                OrdersCount = 30,
                ChangeCount = 40
            };

            //ACT
            var result = (ViewResult)testAdminController.AdminFrontPage();
            var resultVM = (AdminFrontPageVM)result.Model;

            //ASSERT
            Assert.AreEqual(result.ViewName, "");
            Assert.AreEqual(expected.UserCount, resultVM.UserCount);
            Assert.AreEqual(expected.FilmCount, resultVM.FilmCount);
            Assert.AreEqual(expected.OrdersCount, resultVM.OrdersCount);
            Assert.AreEqual(expected.ChangeCount, resultVM.ChangeCount);
        }

        [TestMethod]
        public void AdminFrontPage_View_NotAdmin()
        {
            //Arrange
            NotAdmin();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.AdminFrontPage();

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void AdminFrontPage_DBFAIL()
        {
            //Arrange
            AdminCheck(666);

            //ACT
            var result = (RedirectToRouteResult)testAdminController.AdminFrontPage();

            //ASSERT
            Assert.AreEqual(result.RouteValues["controller"], "Home");
            Assert.AreEqual(result.RouteValues["action"], "ShowMessage");
        }

        #endregion Admin Test Methods

        //--------------------------------USER

        #region User Test Methods

        [TestMethod]
        public void AllUsers_OK()
        {
            //Arrange
            AdminCheck();
            var expected = ExpectedUserList();

            //ACT
            var result = (ViewResult)testAdminController.AllUsers();
            var resultList = (List<UserVM>)result.Model;

            //Assert
            Assert.AreEqual(result.ViewName, "Users");

            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Id, resultList[i].Id);
                Assert.AreEqual(expected[i].FirstName, resultList[i].FirstName);
                Assert.AreEqual(expected[i].Address, resultList[i].Address);
                Assert.AreEqual(expected[i].Postal, resultList[i].Postal);
                Assert.AreEqual(expected[i].PostalNr, resultList[i].PostalNr);
                Assert.AreEqual(expected[i].CreatedDate, resultList[i].CreatedDate);
                Assert.AreEqual(expected[i].PassWord, resultList[i].PassWord);
                Assert.AreEqual(expected[i].PassWordRepeat, resultList[i].PassWordRepeat);
                Assert.AreEqual(expected[i].Email, resultList[i].Email);
                Assert.AreEqual(expected[i].PhoneNr, resultList[i].PhoneNr);
            }
        }

        [TestMethod]
        public void AllUsers_DBFAIL()
        {
            //Arrange
            AdminCheck(666);

            //ACT
            var result = (RedirectToRouteResult)testAdminController.AllUsers();

            //ASSERT
            AssertDBEXE(result);
        }

        [TestMethod]
        public void AllUsers_notAdmin()
        {
            //Arrange
            NotAdmin();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.AllUsers();

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void AllUsers_SessionNull()
        {
            //Arrange
            SessionNull();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.AllUsers();

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void CreateUser_OK()
        {
            // Arrange
            AdminCheck();

            // Act
            var result = (ViewResult)testAdminController.CreateUser();

            // Assert
            Assert.AreEqual(result.ViewName, "");
        }

        [TestMethod]
        public void CreateUser_DBFAIL()
        {
            // Arrange
            AdminCheck(666);

            // Act
            var result = (RedirectToRouteResult)testAdminController.CreateUser();

            // Assert
            AssertDBEXE(result);
        }

        [TestMethod]
        public void CreateUser_NotAdmin()
        {
            // Arrange
            NotAdmin();

            // Act
            var result = (RedirectToRouteResult)testAdminController.CreateUser();

            // Assert
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void CreateUser_SessionNULL()
        {
            // Arrange
            SessionNull();

            // Act
            var result = (RedirectToRouteResult)testAdminController.CreateUser();

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void CreateUser_post_OK()
        {
            // Arrange

            // Act
            var result = (RedirectToRouteResult)testAdminController.CreateUser(ExpectedUser);

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(result.RouteValues.Values.First(), "AllUsers");
        }

        [TestMethod]
        public void CreateUser_DBFAIL_POST()
        {
            //ASSSERT
            var user = ExpectedUser;
            user.Id = 666;
            // Act
            var result = (RedirectToRouteResult)testAdminController.CreateUser(ExpectedUser);

            //ASSERT
            AssertDBEXE(result);
        }

        [TestMethod]
        public void CreateUser_Fail_post()
        {
            // Arrange
            var user = ExpectedUser;
            user.Id = 0;
            // Act
            var result = (ViewResult)testAdminController.CreateUser(user);

            // Assert
            Assert.AreEqual(result.ViewName, "");
        }

        [TestMethod]
        public void CreateUser_NotValid_post()
        {
            // Arrange
            testAdminController.ViewData.ModelState.AddModelError("fail", "ID = 0");
            // Act
            var result = (ViewResult)testAdminController.CreateUser(ExpectedUser);

            // Assert
            Assert.AreEqual(result.ViewName, "");
        }

        [TestMethod]
        public void SearchUsers_OK()
        {
            //Arrange
            AdminCheck();
            var searchString = "Tønsberg";

            var expected = ExpectedUserList();

            //Act
            var result = (ViewResult)testAdminController.SearchUsers(searchString);
            var test = (List<UserVM>)result.Model;

            //Viewresult
            Assert.AreEqual(result.ViewName, "Users");
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Id, test[i].Id);
                Assert.AreEqual(expected[i].FirstName, test[i].FirstName);
                Assert.AreEqual(expected[i].Address, test[i].Address);
                Assert.AreEqual(expected[i].Postal, test[i].Postal);
                Assert.AreEqual(expected[i].PostalNr, test[i].PostalNr);
                Assert.AreEqual(expected[i].CreatedDate, test[i].CreatedDate);
                Assert.AreEqual(expected[i].PassWord, test[i].PassWord);
                Assert.AreEqual(expected[i].PassWordRepeat, test[i].PassWordRepeat);
                Assert.AreEqual(expected[i].Email, test[i].Email);
                Assert.AreEqual(expected[i].PhoneNr, test[i].PhoneNr);
            };
        }

        [TestMethod]
        public void SearchUsers_notAdmin()
        {
            //Arrange
            NotAdmin();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.SearchUsers("test");

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void SearchUsers_DBFAIL()
        {
            //Arrange
            AdminCheck(666);

            //ACT
            var result = (RedirectToRouteResult)testAdminController.SearchUsers("fail");

            //ASSERT
            AssertDBEXE(result);
        }

        [TestMethod]
        public void SearchUsers_SessionNull()
        {
            //Arrange
            SessionNull();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.SearchUsers("test");

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void EditUser_OK()
        {
            //Arrange
            AdminCheck();

            // Act
            var result = (ViewResult)testAdminController.EditUser(1);

            // Assert
            Assert.AreEqual(result.ViewName, "");
        }

        [TestMethod]
        public void EditUser_DBFAIL()
        {
            //Arrange
            AdminCheck(666);

            // Act
            var result = (RedirectToRouteResult)testAdminController.EditUser(1);

            // Assert
            AssertDBEXE(result);
        }

        [TestMethod]
        public void EditUser_notAdmin()
        {
            //Arrange
            NotAdmin();

            // Act
            var result = (RedirectToRouteResult)testAdminController.EditUser(1);

            // Assert
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void EditUser_sessionNull()
        {
            //Arrange
            SessionNull();

            // Act
            var result = (RedirectToRouteResult)testAdminController.EditUser(1);

            // Assert
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void EditUser_OK_Post()
        {
            var user = new EditUserVM()
            {
                FirstName = "Mark",
                SurName = "Rogen",
                PostalNr = "5314",
                Postal = "Bergen",
                Address = "Hansaroad 45",
                Email = "Rogen@gmail.com",
                PhoneNr = "81549300",
                Id = 5,
            };

            // Arrange
            testAdminController.Session["Kunde"] = new UserVM()
            {
                Admin = true
            };

            var result = (RedirectToRouteResult)testAdminController.EditUser(user);

            Assert.AreEqual(testAdminController.TempData["Message"], "Bruker med id: " + user.Id + " har nå blitt endret.");
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(result.RouteValues.Values.First(), "AllUsers");
        }

        [TestMethod]
        public void EditUser_DBFAIL_Post()
        {
            // Arrange
            var user = new EditUserVM()
            {
                Id = 666,
            };

            //ACT
            var result = (RedirectToRouteResult)testAdminController.EditUser(user);

            //ASSERT
            AssertDBEXE(result);
        }

        [TestMethod]
        public void EditUser_Fail_Post()
        {
            // Arrange
            var user = new EditUserVM()
            {
                FirstName = "Mark",
                SurName = "Rogen",
                PostalNr = "5314",
                Postal = "Bergen",
                Address = "Hansaroad 45",
                Email = "Rogen@gmail.com",
                PhoneNr = "81549300",
                Id = 0,
            };

            var result = (RedirectToRouteResult)testAdminController.EditUser(user);

            Assert.AreEqual(testAdminController.TempData["Message"], "Noe gikk galt under endringen.");
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(result.RouteValues.Values.First(), "AllUsers");
        }

        [TestMethod]
        public void EditUser_NotValid_Post()
        {
            // Arrange
            var user = new EditUserVM()
            {
                FirstName = "Mark",
                SurName = "Rogen",
                PostalNr = "5314",
                Postal = "Bergen",
                Address = "Hansaroad 45",
                Email = "Rogen@gmail.com",
                PhoneNr = "81549300",
                Id = 5,
            };
            testAdminController.ViewData.ModelState.AddModelError("fail", "ID = 0");
            var result = (ViewResult)testAdminController.EditUser(user);

            Assert.AreEqual(result.ViewName, "");
        }

        [TestMethod]
        public void ToggleActivateUser_OK()
        {
            //Arrange
            AdminCheck();
            var expectedResult = ExpectedUserList();
            expectedResult[0].Active = false;
            testAdminController.TempData["UserVMs"] = ExpectedUserList();

            //Act
            var result = (ViewResult)testAdminController.ToggleActivateUser(1);
            var resultList = (List<UserVM>)result.Model;
            var temp1 = testAdminController.TempData["message"];

            //ASSERT
            Assert.AreEqual(temp1, "Brukeren er nå deaktivert/aktivert.");
            Assert.AreEqual(result.ViewName, "Users");

            int count = expectedResult.Count < resultList.Count ? resultList.Count : expectedResult.Count;
            for (var i = 0; i < count; i++)
            {
                Assert.AreEqual(expectedResult[i].Id, resultList[i].Id);
                Assert.AreEqual(expectedResult[i].FirstName, resultList[i].FirstName);
                Assert.AreEqual(expectedResult[i].Address, resultList[i].Address);
                Assert.AreEqual(expectedResult[i].Postal, resultList[i].Postal);
                Assert.AreEqual(expectedResult[i].PostalNr, resultList[i].PostalNr);
                Assert.AreEqual(expectedResult[i].CreatedDate, resultList[i].CreatedDate);
                Assert.AreEqual(expectedResult[i].PassWord, resultList[i].PassWord);
                Assert.AreEqual(expectedResult[i].PassWordRepeat, resultList[i].PassWordRepeat);
                Assert.AreEqual(expectedResult[i].Email, resultList[i].Email);
                Assert.AreEqual(expectedResult[i].PhoneNr, resultList[i].PhoneNr);
            }
        }

        [TestMethod]
        public void ToggleActivateUser_Fail()
        {
            //Arrange
            AdminCheck();
            testAdminController.TempData["UserVMs"] = ExpectedUserList();
            var expectedResult = ExpectedUserList();

            //Act
            var result = (ViewResult)testAdminController.ToggleActivateUser(0);
            var resultList = (List<UserVM>)result.Model;
            var temp1 = testAdminController.TempData["message"];
            var temp2 = testAdminController.TempData["errormessage"];

            //ASSERT
            Assert.AreEqual(temp1, "Noe gikk galt under endringen.");
            Assert.AreEqual(temp2, "Feilmelding lagret til logg.");
            Assert.AreEqual(result.ViewName, "Users");
        }

        [TestMethod]
        public void ToggleActivateUser_NotAdmin()
        {
            //Arrange
            NotAdmin();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.ToggleActivateUser(1);

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void ToggleActivateUser_DBFAIL()
        {
            //Arrange
            AdminCheck(666);

            //ACT
            var result = (RedirectToRouteResult)testAdminController.ToggleActivateUser(1);

            //ASSERT
            AssertDBEXE(result);
        }

        [TestMethod]
        public void ToggleActivateUser_SessionNull()
        {
            //Arrange
            SessionNull();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.ToggleActivateUser(1);

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void ResetPassWord_OK()
        {
            //Arrange
            AdminCheck();
            //
            var result = (ViewResult)testAdminController.ResetPassWord(1);

            // Assert
            Assert.AreEqual(result.ViewName, "");
        }

        [TestMethod]
        public void ResetPassWord_notAdmin()
        {
            //Arrange
            NotAdmin();
            //
            var result = (RedirectToRouteResult)testAdminController.ResetPassWord(1);

            // Assert
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void ResetPassWord_sessionNull()
        {
            //Arrange
            SessionNull();
            //
            var result = (RedirectToRouteResult)testAdminController.ResetPassWord(1);

            // Assert
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void ResetPassWord_DBFAIL()
        {
            //Arrange
            AdminCheck(666);
            var passVM = new PassWordChangeVM()
            {
                Id = 666
            };

            var result = (RedirectToRouteResult)testAdminController.ResetPassWord(passVM);
            var result2 = (RedirectToRouteResult)testAdminController.ResetPassWord(1);

            //ASSERT
            AssertDBEXE(result);
            AssertDBEXE(result2);
        }

        [TestMethod]
        public void ResetPassWord_POST_OK()
        {
            //Arrange
            AdminCheck();
            var passVM = new PassWordChangeVM()
            {
                Id = 1
            };

            var result = (ViewResult)testAdminController.ResetPassWord(passVM);

            Assert.AreEqual(testAdminController.TempData["Message"], "Endring av passord for bruker med id: 1 velykket.");
            Assert.AreEqual(result.ViewName, "EditUser");
        }

        [TestMethod]
        public void ResetPassWord_POST_FAIL()
        {
            //Arrange
            AdminCheck();
            var passVM = new PassWordChangeVM()
            {
                Id = 0
            };

            //ACT
            var result = (RedirectToRouteResult)testAdminController.ResetPassWord(passVM);

            //ASSERT
            Assert.AreEqual(testAdminController.TempData["Message"], "Noe gikk galt under passordendringen av kunden!");
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(result.RouteValues.Values.First(), "AllUsers");
        }

        [TestMethod]
        public void ResetPassWord_POST_NotValid()
        {
            //Arrange
            AdminCheck();
            testAdminController.ViewData.ModelState.AddModelError("fail", "");

            //ACT
            var result = (ViewResult)testAdminController.ResetPassWord(new PassWordChangeVM());
            //ASSERT
            Assert.AreEqual(result.ViewName, "");
        }

        #endregion User Test Methods

        //--------------------------------FILM TEST METHODS

        #region Film Test Methods

        [TestMethod]
        public void AddFilm_show_view_SessionNULL()
        {
            // Arrange
            SessionNull();

            var expected = ExEmptyAddFilmVM();

            // Act
            var result = (RedirectToRouteResult)testAdminController.AddFilm();

            // Assert
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void AddFilm_show_view_NotAdmin()
        {
            // Arrange
            NotAdmin();

            var expected = ExEmptyAddFilmVM();

            // Act
            var result = (RedirectToRouteResult)testAdminController.AddFilm();

            // Assert
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void AddFilm_post_OK()
        {
            // Act
            var result = (RedirectToRouteResult)testAdminController.AddFilm(ExAddFilmVM);

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(result.RouteValues.Values.First(), "ListAllFilms");
            Assert.AreEqual(testAdminController.TempData["message"], "Registrering av ny film fullført!");
        }

        [TestMethod]
        public void AddFilm_fail_post()
        {
            // Arrange
            var controller = testAdminController;

            // Act
            var result = (RedirectToRouteResult)controller.AddFilm(ExFailAddFilmVM);

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(result.RouteValues.Values.First(), "AddFilm");
            Assert.AreEqual(controller.TempData["message"], "Feil ved registrering av ny film!");
            Assert.AreEqual(controller.TempData["errormessage"], "Feilmelding lagret til logg.");
        }

        [TestMethod]
        public void AddFilm_POST_notValid_NotModelNull()
        {
            // Arrange
            testAdminController.ViewData.ModelState.AddModelError("fail", "IkkeFilmTittel");

            // Act
            var result = (ViewResult)testAdminController.AddFilm(ExAddFilmVM);

            // Assert
            Assert.AreEqual(result.ViewName, "");
            Assert.AreEqual(testAdminController.TempData["message"], "Feil ved registrering av ny film!");
            Assert.AreEqual(testAdminController.TempData["errormessage"], "Vennligst sjekk at alle felter er utfylt");
        }

        [TestMethod]
        public void AddFilm_POST_notValid_modelNull()
        {
            // Arrange
            AdminCheck();
            testAdminController.ViewData.ModelState.AddModelError("fail", "IkkeFilmTittel");

            // Act
            testAdminController.EditFilm(0);
            var result = (RedirectToRouteResult)testAdminController.AddFilm(ExAddFilmVM);

            //Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(result.RouteValues.Values.First(), "ListAllFilms");
            Assert.AreEqual(testAdminController.TempData["message"], "Feil ved lasting av skjema for film!");
            Assert.AreEqual(testAdminController.TempData["errormessage"], "Feilmelding lagret til logg.");

            testAdminController.EditFilm(0);
        }

        [TestMethod]
        public void AddFilm_GET_modelNull_adminOK()
        {
            //Arrange
            AdminCheck();

            //Act
            testAdminController.EditFilm(0);
            var result = (RedirectToRouteResult)testAdminController.AddFilm();

            //Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(result.RouteValues.Values.First(), "ListAllFilms");
            Assert.AreEqual(testAdminController.TempData["message"], "Feil ved lasting av skjema for film!");
            Assert.AreEqual(testAdminController.TempData["errormessage"], "Feilmelding lagret til logg.");

            testAdminController.EditFilm(0);
        }

        [TestMethod]
        public void EditFilm_show_view_session_null()
        {
            //Arrange
            SessionNull();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.AdminFrontPage();

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void LogOut_show_Redirect()
        {
            //Using MvcContrib.Mvc3.TestHelper-c for session https://dzone.com/articles/aspnet-mvc-how-mock-session
            TestControllerBuilder builder = new TestControllerBuilder();

            // Arrange
            var controller = new AdminController();
            builder.InitializeController(controller);

            //ACT
            var result = (RedirectToRouteResult)controller.LogOut();

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(result.RouteValues.Values.First(), "Frontpage");
            ;
        }

        [TestMethod]
        public void ListAllFilms_show_view_OK()
        {
            //Arrange
            AdminCheck();
            var expected = ExAddFilmVMList();

            //Act
            var result = (ViewResult)testAdminController.ListAllFilms();
            var resultList = (List<AddFilmVM>)result.Model;

            //Assert
            Assert.AreEqual(result.ViewName, "AllFilms");
            for (var i = 0; i < resultList.Count; i++)
            {
                Assert.AreEqual(expected[i].FilmId, resultList[i].FilmId);
                Assert.AreEqual(expected[i].Title, resultList[i].Title);
                Assert.AreEqual(expected[i].Description, resultList[i].Description);
                Assert.AreEqual(expected[i].ImgURL, resultList[i].ImgURL);
                Assert.AreEqual(expected[i].FilmPriceClassId, resultList[i].FilmPriceClassId);
                CollectionAssert.AreEqual(expected[i].CurrFilmGenreIds, resultList[i].CurrFilmGenreIds);
                for (var j = 0; j < expected[i].CurrentGenres.Count; j++)
                {
                    Assert.AreEqual(expected[i].CurrentGenres[j].Id, resultList[i].CurrentGenres[j].Id);
                    Assert.AreEqual(expected[i].CurrentGenres[j].Name, resultList[i].CurrentGenres[j].Name);
                }
                Assert.AreEqual(expected[i].PriceId, resultList[i].PriceId);
                CollectionAssert.AreEqual(expected[i].GenreIDs, resultList[i].GenreIDs);
                CheckSLIObjects(expected[i].PriceSelectList, resultList[i].PriceSelectList);
                CheckSLIObjects(expected[i].GenreSelectList, resultList[i].GenreSelectList);
                Assert.AreEqual(expected[i].JsonSerialize, resultList[i].JsonSerialize);
                Assert.AreEqual(expected[i].Active, resultList[i].Active);
            }
        }

        [TestMethod]
        public void ListAllFilms_session_null()
        {
            //Arrange
            SessionNull();

            //Act
            var result = (RedirectToRouteResult)testAdminController.ListAllFilms();

            //Assert
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void ListAllFilms_session_notAdmin()
        {
            //Arrange
            NotAdmin();

            //Act
            var result = (RedirectToRouteResult)testAdminController.ListAllFilms();

            //Assert
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void AddFilm_show_view_OK()
        {
            // Arrange
            AdminCheck();
            var expected = ExEmptyAddFilmVM();

            // Act
            var result = (ViewResult)testAdminController.AddFilm();
            var resultVM = (AddFilmVM)result.Model;

            // Assert
            Assert.AreEqual(result.ViewName, "");
            CheckSLIObjects(expected.PriceSelectList, resultVM.PriceSelectList);
            CheckSLIObjects(expected.GenreSelectList, resultVM.GenreSelectList);
        }

        [TestMethod]
        public void AddFilm_modelNull_adminOK()
        {
            //Arrange
            AdminCheck();

            //ACT
            testAdminController.EditFilm(0);
            var result = (RedirectToRouteResult)testAdminController.AddFilm();

            //Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(result.RouteValues.Values.First(), "ListAllFilms");
            Assert.AreEqual(testAdminController.TempData["message"], "Feil ved lasting av skjema for film!");
            Assert.AreEqual(testAdminController.TempData["errormessage"], "Feilmelding lagret til logg.");

            //resets stub value
            testAdminController.EditFilm(0);
        }

        [TestMethod]
        public void EditFilm_show_view_notAdmin()
        {
            //Arrange
            NotAdmin();

            //Act
            var result = (RedirectToRouteResult)testAdminController.EditFilm(1);

            //Assert
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void EditFilm_show_view_SessionNULL()
        {
            //Arrange
            SessionNull();

            //Act
            var result = (RedirectToRouteResult)testAdminController.EditFilm(1);

            //Assert
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void EditFilm_show_view_admin_OK()
        {
            //Arrange
            AdminCheck();
            AddFilmVM expected = ExEditAddFilmVM();

            //Act
            var result = (ViewResult)testAdminController.EditFilm(1);
            AddFilmVM actual = (AddFilmVM)result.Model;

            //Assert
            Assert.AreEqual(expected.FilmId, actual.FilmId);
            Assert.AreEqual(expected.Title, actual.Title);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.ImgURL, actual.ImgURL);
            Assert.AreEqual(expected.FilmPriceClassId, actual.FilmPriceClassId);
            CollectionAssert.AreEqual(expected.CurrFilmGenreIds, actual.CurrFilmGenreIds);
            for (var j = 0; j < expected.CurrentGenres.Count; j++)
            {
                Assert.AreEqual(expected.CurrentGenres[j].Id, actual.CurrentGenres[j].Id);
                Assert.AreEqual(expected.CurrentGenres[j].Name, actual.CurrentGenres[j].Name);
            }
            Assert.AreEqual(expected.PriceId, actual.PriceId);
            CollectionAssert.AreEqual(expected.GenreIDs, actual.GenreIDs);
            CheckSLIObjects(expected.PriceSelectList, actual.PriceSelectList);
            CheckSLIObjects(expected.GenreSelectList, actual.GenreSelectList);
            Assert.AreEqual(expected.JsonSerialize, actual.JsonSerialize);
            Assert.AreEqual(expected.Active, actual.Active);
        }

        [TestMethod]
        public void EditFilm_show_view_admin_exception()
        {
            //Arrange
            AdminCheck();
            AddFilmVM expected = ExEditAddFilmVM();

            //Act
            var result = (RedirectToRouteResult)testAdminController.EditFilm(0);
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(result.RouteValues.Values.First(), "AdminFrontPage");
            Assert.AreEqual(testAdminController.TempData["message"], "Feil ved endring av film!");
            Assert.AreEqual(testAdminController.TempData["errormessage"], "Feilmelding lagret til logg.");
        }

        [TestMethod]
        public void EditFilm_Ok_post()
        {
            // Act
            var result = (RedirectToRouteResult)testAdminController.EditFilm(ExAddFilmVM);

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(result.RouteValues.Values.First(), "ListAllFilms");
            Assert.AreEqual(testAdminController.TempData["message"], "Endringer gjennomført");
        }

        [TestMethod]
        public void EditFilm_fail_post()
        {
            // Act
            var result = (RedirectToRouteResult)testAdminController.EditFilm(ExFailAddFilmVM);

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(result.RouteValues.Values.First(), "ListAllFilms");
            Assert.AreEqual(testAdminController.TempData["message"], "Feil ved endring av film!");
            Assert.AreEqual(testAdminController.TempData["errormessage"], "Feilmelding lagret til logg.");
        }

        [TestMethod]
        public void EditFilm_notValid_post()
        {
            // Arrange
            testAdminController.ViewData.ModelState.AddModelError("fail", "IkkeFilmTittel");
            // Act

            var result = (RedirectToRouteResult)testAdminController.EditFilm(ExNotValidAddFilmVM);

            // Assert
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(result.RouteValues.Values.First(), "ListAllFilms");
            Assert.AreEqual(testAdminController.TempData["message"], "Endringer kunne ikke gjennomføres.");
            Assert.AreEqual(testAdminController.TempData["errormessage"], "Vennligst sjekk at alle felter er utfylt.");
        }

        [TestMethod]
        public void ToggleActivateFilm_OK()
        {
            // Arrange
            AdminCheck();
            testAdminController.TempData["AddFilmVMs"] = ExAddFilmVMList();
            var expectedResult = ExAddFilmVMList();

            expectedResult[0].Active = !expectedResult[0].Active;

            // Act
            var result = (ViewResult)testAdminController.ToggleActivateFilm(1);
            var resultList = (List<AddFilmVM>)result.Model;

            // Assert
            Assert.AreEqual(result.ViewName, "AllFilms");
            Assert.AreEqual(testAdminController.TempData["message"], "Aktivering/deaktivering gjennomført");

            int count = expectedResult.Count < resultList.Count ? resultList.Count : expectedResult.Count;
            for (var i = 0; i < count; i++)
            {
                Assert.AreEqual(expectedResult[i].FilmId, resultList[i].FilmId);
                Assert.AreEqual(expectedResult[i].Title, resultList[i].Title);
                Assert.AreEqual(expectedResult[i].Description, resultList[i].Description);
                Assert.AreEqual(expectedResult[i].ImgURL, resultList[i].ImgURL);
                Assert.AreEqual(expectedResult[i].FilmPriceClassId, resultList[i].FilmPriceClassId);
                CollectionAssert.AreEqual(expectedResult[i].CurrFilmGenreIds, resultList[i].CurrFilmGenreIds);
                for (var j = 0; j < expectedResult[i].CurrentGenres.Count; j++)
                {
                    Assert.AreEqual(expectedResult[i].CurrentGenres[j].Id, resultList[i].CurrentGenres[j].Id);
                    Assert.AreEqual(expectedResult[i].CurrentGenres[j].Name, resultList[i].CurrentGenres[j].Name);
                }
                Assert.AreEqual(expectedResult[i].PriceId, resultList[i].PriceId);
                CollectionAssert.AreEqual(expectedResult[i].GenreIDs, resultList[i].GenreIDs);
                CheckSLIObjects(expectedResult[i].PriceSelectList, resultList[i].PriceSelectList);
                CheckSLIObjects(expectedResult[i].GenreSelectList, resultList[i].GenreSelectList);
                Assert.AreEqual(expectedResult[i].JsonSerialize, resultList[i].JsonSerialize);
                Assert.AreEqual(expectedResult[i].Active, resultList[i].Active);
            }
        }

        [TestMethod]
        public void ToggleActivateFilm_admin_fail()
        {
            //// Arrange
            AdminCheck();
            testAdminController.TempData["AddFilmVMs"] = ExAddFilmVMList();
            var expectedResult = ExAddFilmVMList();

            // Act
            var result = (ViewResult)testAdminController.ToggleActivateFilm(-1);
            var resultList = (List<AddFilmVM>)result.Model;

            // Assert
            Assert.AreEqual(result.ViewName, "AllFilms");
            Assert.AreEqual(testAdminController.TempData["message"], "Noe gikk galt under aktivering/deaktivering av film");

            int count = expectedResult.Count < resultList.Count ? resultList.Count : expectedResult.Count;

            for (var i = 0; i < count; i++)
            {
                Assert.AreEqual(expectedResult[i].FilmId, resultList[i].FilmId);
                Assert.AreEqual(expectedResult[i].Title, resultList[i].Title);
                Assert.AreEqual(expectedResult[i].Description, resultList[i].Description);
                Assert.AreEqual(expectedResult[i].ImgURL, resultList[i].ImgURL);
                Assert.AreEqual(expectedResult[i].FilmPriceClassId, resultList[i].FilmPriceClassId);
                CollectionAssert.AreEqual(expectedResult[i].CurrFilmGenreIds, resultList[i].CurrFilmGenreIds);
                for (var j = 0; j < expectedResult[i].CurrentGenres.Count; j++)
                {
                    Assert.AreEqual(expectedResult[i].CurrentGenres[j].Id, resultList[i].CurrentGenres[j].Id);
                    Assert.AreEqual(expectedResult[i].CurrentGenres[j].Name, resultList[i].CurrentGenres[j].Name);
                }
                Assert.AreEqual(expectedResult[i].PriceId, resultList[i].PriceId);
                CollectionAssert.AreEqual(expectedResult[i].GenreIDs, resultList[i].GenreIDs);
                CheckSLIObjects(expectedResult[i].PriceSelectList, resultList[i].PriceSelectList);
                CheckSLIObjects(expectedResult[i].GenreSelectList, resultList[i].GenreSelectList);
                Assert.AreEqual(expectedResult[i].JsonSerialize, resultList[i].JsonSerialize);
                Assert.AreEqual(expectedResult[i].Active, resultList[i].Active);
            }
        }

        [TestMethod]
        public void ToggleActivateFilm_sessionNULL()
        {
            // Arrange
            SessionNull();

            // Act
            var result = (RedirectToRouteResult)testAdminController.ToggleActivateFilm(0);

            // Assert
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void ToggleActivateFilm_notAdmin()
        {
            // Arrange
            NotAdmin();

            // Act
            var result = (RedirectToRouteResult)testAdminController.ToggleActivateFilm(0);

            // Assert
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void SearchFilm_FilmTittel_OK()
        {
            //Arrange
            var searchString = "FilmTittel";
            AdminCheck();

            var expected = ExSearch_FilmTittel();

            //Act
            var result = (ViewResult)testAdminController.SearchFilm(searchString);
            var resultList = (List<AddFilmVM>)result.Model;

            //Assert
            Assert.AreEqual(result.ViewName, "AllFilms");
            for (var i = 0; i < resultList.Count; i++)
            {
                Assert.AreEqual(expected[i].FilmId, resultList[i].FilmId);
                Assert.AreEqual(expected[i].Title, resultList[i].Title);
                Assert.AreEqual(expected[i].Description, resultList[i].Description);
                Assert.AreEqual(expected[i].ImgURL, resultList[i].ImgURL);
                Assert.AreEqual(expected[i].FilmPriceClassId, resultList[i].FilmPriceClassId);
                CollectionAssert.AreEqual(expected[i].CurrFilmGenreIds, resultList[i].CurrFilmGenreIds);
                for (var j = 0; j < expected[i].CurrentGenres.Count; j++)
                {
                    Assert.AreEqual(expected[i].CurrentGenres[j].Id, resultList[i].CurrentGenres[j].Id);
                    Assert.AreEqual(expected[i].CurrentGenres[j].Name, resultList[i].CurrentGenres[j].Name);
                }
                Assert.AreEqual(expected[i].PriceId, resultList[i].PriceId);
                CollectionAssert.AreEqual(expected[i].GenreIDs, resultList[i].GenreIDs);
                CheckSLIObjects(expected[i].PriceSelectList, resultList[i].PriceSelectList);
                CheckSLIObjects(expected[i].GenreSelectList, resultList[i].GenreSelectList);
                Assert.AreEqual(expected[i].JsonSerialize, resultList[i].JsonSerialize);
                Assert.AreEqual(expected[i].Active, resultList[i].Active);
            }
        }

        [TestMethod]
        public void SearchFilm_FilmTittel_SessionNULL()
        {
            // Arrange
            SessionNull();

            // Act
            var result = (RedirectToRouteResult)testAdminController.SearchFilm("FilmTittel");

            // Assert
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void SearchFilm_FilmTittel_notAdmin()
        {
            // Arrange
            NotAdmin();

            // Act
            var result = (RedirectToRouteResult)testAdminController.SearchFilm("FilmTittel");

            // Assert
            AssertAdminSessFail(result);
        }

        #endregion Film Test Methods

        //--------------------------------ORDER

        #region Order Test Methods

        [TestMethod]
        public void Show_All_Orders_OK()
        {
            //Arrange
            AdminCheck();
            var expectedResult = ExpectedAllOrdersResult();

            //ACT
            var result = (ViewResult)testAdminController.AllOrders();
            var resultList = (List<ExpandedOrderVM>)result.Model;

            //ASSERT
            Assert.AreEqual(result.ViewName, "Orders");

            int count = expectedResult.Count < resultList.Count ? resultList.Count : expectedResult.Count;
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(expectedResult[i].FirstName, resultList[i].FirstName);
                Assert.AreEqual(expectedResult[i].SurName, resultList[i].SurName);
                Assert.AreEqual(expectedResult[i].Email, resultList[i].Email);
                Assert.AreEqual(expectedResult[i].Order.OrderNr, resultList[i].Order.OrderNr);
                Assert.AreEqual(expectedResult[i].Order.Date, resultList[i].Order.Date);
                int innerCount = expectedResult[i].Order.OrderLines.Count < resultList[i].Order.OrderLines.Count
                    ? resultList[i].Order.OrderLines.Count
                    : expectedResult[i].Order.OrderLines.Count;
                for (int j = 0; j < innerCount; j++)
                {
                    Assert.AreEqual(expectedResult[i].Order.OrderLines[j].Title,
                        resultList[i].Order.OrderLines[j].Title);
                    Assert.AreEqual(expectedResult[i].Order.OrderLines[j].Price,
                        resultList[i].Order.OrderLines[j].Price);
                }
            }
        }

        [TestMethod]
        public void Show_All_Orders_NotAdmin()
        {
            //Arrange
            NotAdmin();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.AllOrders();

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void Show_All_Orders_SessionNULL()
        {
            //Arrange
            SessionNull();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.AllOrders();

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void Show_All_Orders_DBFAIL()
        {
            //Arrange
            AdminCheck(666);

            //ACT
            var result = (RedirectToRouteResult)testAdminController.AllOrders();

            //ASSERT
            AssertDBEXE(result);
        }

        [TestMethod]
        public void Search_Orders_OK()
        {
            //Arrange
            AdminCheck();
            var expectedResult = ExpectedOrderSearchList();

            var searchString = "ola@nordmann.no";
            var result = (ViewResult)testAdminController.SearchOrders(searchString);
            var resultList = (List<ExpandedOrderVM>)result.Model;

            Assert.AreEqual(result.ViewName, "Orders");
            string message = "Søkeresultat for: " + searchString;
            Assert.AreEqual(testAdminController.TempData["message"], message);
            int count = expectedResult.Count < resultList.Count ? resultList.Count : expectedResult.Count;
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(expectedResult[i].FirstName, resultList[i].FirstName);
                Assert.AreEqual(expectedResult[i].SurName, resultList[i].SurName);
                Assert.AreEqual(expectedResult[i].Email, resultList[i].Email);
                Assert.AreEqual(expectedResult[i].Order.OrderNr, resultList[i].Order.OrderNr);
                Assert.AreEqual(expectedResult[i].Order.Date, resultList[i].Order.Date);
                int innerCount = expectedResult[i].Order.OrderLines.Count < resultList[i].Order.OrderLines.Count
                    ? resultList[i].Order.OrderLines.Count
                    : expectedResult[i].Order.OrderLines.Count;
                for (int j = 0; j < innerCount; j++)
                {
                    Assert.AreEqual(expectedResult[i].Order.OrderLines[j].Title,
                        resultList[i].Order.OrderLines[j].Title);
                    Assert.AreEqual(expectedResult[i].Order.OrderLines[j].Price,
                        resultList[i].Order.OrderLines[j].Price);
                }
            }
        }

        [TestMethod]
        public void Search_Orders_NotAdmin()
        {
            //Arrange
            NotAdmin();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.SearchOrders(null);

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void Search_Orders_SessionNULL()
        {
            //Arrange
            SessionNull();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.SearchOrders(null);

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void Search_Orders_DBFAIL()
        {
            //Arrange
            AdminCheck(666);

            //ACT
            var result = (RedirectToRouteResult)testAdminController.SearchOrders(null);

            //ASSERT
            AssertDBEXE(result);
        }

        [TestMethod]
        public void Refund_Film_From_Order_OK()
        {
            //Arrange
            AdminCheck();
            testAdminController.TempData["ExpandedOrderVM"] = ExpectedAllOrdersResult();
            var expectedResult = ExpectedRefundFilmResult();

            int orderNr = 0;
            string filmTitle = "Mad Max";
            var result = (ViewResult)testAdminController.RefundFilm(orderNr, filmTitle);
            var resultList = (List<ExpandedOrderVM>)result.Model;

            Assert.AreEqual(result.ViewName, "Orders");
            string message = filmTitle + " ble refundert fra ordre " + orderNr;
            Assert.AreEqual(testAdminController.TempData["message"], message);
            int count = expectedResult.Count < resultList.Count ? resultList.Count : expectedResult.Count;
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(expectedResult[i].FirstName, resultList[i].FirstName);
                Assert.AreEqual(expectedResult[i].SurName, resultList[i].SurName);
                Assert.AreEqual(expectedResult[i].Email, resultList[i].Email);
                Assert.AreEqual(expectedResult[i].Order.OrderNr, resultList[i].Order.OrderNr);
                Assert.AreEqual(expectedResult[i].Order.Date, resultList[i].Order.Date);
                int innerCount = expectedResult[i].Order.OrderLines.Count < resultList[i].Order.OrderLines.Count
                    ? resultList[i].Order.OrderLines.Count
                    : expectedResult[i].Order.OrderLines.Count;
                for (int j = 0; j < innerCount; j++)
                {
                    Assert.AreEqual(expectedResult[i].Order.OrderLines[j].Title,
                        resultList[i].Order.OrderLines[j].Title);
                    Assert.AreEqual(expectedResult[i].Order.OrderLines[j].Price,
                        resultList[i].Order.OrderLines[j].Price);
                }
            }
        }

        [TestMethod]
        public void Refund_Film_From_Order_NotAdmin()
        {
            //Arrange
            NotAdmin();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.RefundFilm(0, null);

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void Refund_Film_From_Order_SessionNULL()
        {
            //Arrange
            SessionNull();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.RefundFilm(0, null);

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void Refund_Film_From_Order_Failed()
        {
            //Arrange
            AdminCheck();
            testAdminController.TempData["ExpandedOrderVM"] = ExpectedAllOrdersResult();

            var expectedResult = ExpectedAllOrdersResult();

            //Viewresult Admin.Orders()
            int orderNr = 2;
            string filmTitle = "Mad Max";
            var result = (ViewResult)testAdminController.RefundFilm(orderNr, filmTitle);
            var resultList = (List<ExpandedOrderVM>)result.Model;

            Assert.AreEqual(result.ViewName, "Orders");
            string errorMessage = "Noe gikk galt under fjerningen av film " + filmTitle + " fra ordre " + orderNr;
            Assert.AreEqual(testAdminController.TempData["errormessage"], errorMessage);
            int count = expectedResult.Count < resultList.Count ? resultList.Count : expectedResult.Count;
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(expectedResult[i].FirstName, resultList[i].FirstName);
                Assert.AreEqual(expectedResult[i].SurName, resultList[i].SurName);
                Assert.AreEqual(expectedResult[i].Email, resultList[i].Email);
                Assert.AreEqual(expectedResult[i].Order.OrderNr, resultList[i].Order.OrderNr);
                Assert.AreEqual(expectedResult[i].Order.Date, resultList[i].Order.Date);
                int innerCount = expectedResult[i].Order.OrderLines.Count < resultList[i].Order.OrderLines.Count
                    ? resultList[i].Order.OrderLines.Count
                    : expectedResult[i].Order.OrderLines.Count;
                for (int j = 0; j < innerCount; j++)
                {
                    Assert.AreEqual(expectedResult[i].Order.OrderLines[j].Title,
                        resultList[i].Order.OrderLines[j].Title);
                    Assert.AreEqual(expectedResult[i].Order.OrderLines[j].Price,
                        resultList[i].Order.OrderLines[j].Price);
                }
            }
        }

        [TestMethod]
        public void Refund_Film_From_Order_DBFAIL()
        {
            //Arrange
            AdminCheck(666);
            string title = "Mad max";
            int ordernr = 1;

            //ACT
            var result = (RedirectToRouteResult)testAdminController.RefundFilm(ordernr, title);

            //ASSERT
            AssertDBEXE(result);
        }

        [TestMethod]
        public void Refund_Order_OK()
        {
            //Arrange
            AdminCheck();
            testAdminController.TempData["ExpandedOrderVM"] = ExpectedAllOrdersResult();

            var expectedResult = ExpectedRefundOrderResult();

            //Viewresult Admin.Orders()
            int orderNr = 0;
            var result = (ViewResult)testAdminController.RefundOrder(orderNr);
            var resultList = (List<ExpandedOrderVM>)result.Model;

            Assert.AreEqual(result.ViewName, "Orders");
            string message = "Ordre " + orderNr + " har blitt refundert";
            Assert.AreEqual(testAdminController.TempData["message"], message);
            int count = expectedResult.Count < resultList.Count ? resultList.Count : expectedResult.Count;
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(expectedResult[i].FirstName, resultList[i].FirstName);
                Assert.AreEqual(expectedResult[i].SurName, resultList[i].SurName);
                Assert.AreEqual(expectedResult[i].Email, resultList[i].Email);
                Assert.AreEqual(expectedResult[i].Order.OrderNr, resultList[i].Order.OrderNr);
                Assert.AreEqual(expectedResult[i].Order.Date, resultList[i].Order.Date);
                int innerCount = expectedResult[i].Order.OrderLines.Count < resultList[i].Order.OrderLines.Count
                    ? resultList[i].Order.OrderLines.Count
                    : expectedResult[i].Order.OrderLines.Count;
                for (int j = 0; j < innerCount; j++)
                {
                    Assert.AreEqual(expectedResult[i].Order.OrderLines[j].Title,
                        resultList[i].Order.OrderLines[j].Title);
                    Assert.AreEqual(expectedResult[i].Order.OrderLines[j].Price,
                        resultList[i].Order.OrderLines[j].Price);
                }
            }
        }

        [TestMethod]
        public void Redund_Order_NotAdmin()
        {
            //Arrange
            NotAdmin();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.RefundOrder(0);

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void Refund_Order_SessionNULL()
        {
            //Arrange
            SessionNull();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.RefundOrder(0);

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void Refund_Order_Failed()
        {
            //Arrange
            AdminCheck();
            testAdminController.TempData["ExpandedOrderVM"] = ExpectedAllOrdersResult();

            var expectedResult = ExpectedAllOrdersResult();

            //Viewresult Admin.Orders()
            int orderNr = 2;
            var result = (ViewResult)testAdminController.RefundOrder(orderNr);
            var resultList = (List<ExpandedOrderVM>)result.Model;

            Assert.AreEqual(result.ViewName, "Orders");
            string errormessage = "Noe gikk galt under refunderingen av ordrenr: " + orderNr;
            Assert.AreEqual(testAdminController.TempData["errormessage"], errormessage);
            int count = expectedResult.Count < resultList.Count ? resultList.Count : expectedResult.Count;
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(expectedResult[i].FirstName, resultList[i].FirstName);
                Assert.AreEqual(expectedResult[i].SurName, resultList[i].SurName);
                Assert.AreEqual(expectedResult[i].Email, resultList[i].Email);
                Assert.AreEqual(expectedResult[i].Order.OrderNr, resultList[i].Order.OrderNr);
                Assert.AreEqual(expectedResult[i].Order.Date, resultList[i].Order.Date);
                int innerCount = expectedResult[i].Order.OrderLines.Count < resultList[i].Order.OrderLines.Count
                    ? resultList[i].Order.OrderLines.Count
                    : expectedResult[i].Order.OrderLines.Count;
                for (int j = 0; j < innerCount; j++)
                {
                    Assert.AreEqual(expectedResult[i].Order.OrderLines[j].Title,
                        resultList[i].Order.OrderLines[j].Title);
                    Assert.AreEqual(expectedResult[i].Order.OrderLines[j].Price,
                        resultList[i].Order.OrderLines[j].Price);
                }
            }
        }

        [TestMethod]
        public void Refund_Order_DBFAIL()
        {
            //Arrange
            AdminCheck(666);

            //ACT
            var result = (RedirectToRouteResult)testAdminController.RefundOrder(0);

            //ASSERT
            AssertDBEXE(result);
        }

        #endregion Order Test Methods

        //--------------------------------ERRORLOG

        #region ErrorLog Test Methods

        [TestMethod]
        public void AllErrorLogs_OK()
        {
            //Arrange
            AdminCheck();
            var expected = ExpectedErrorList;

            //ACT
            var result = (ViewResult)testAdminController.AllErrorLogs();
            var resultList = (List<ErrorLogVM>)result.Model;

            //Assert
            Assert.AreEqual(result.ViewName, "ErrorLog");

            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Id, resultList[i].Id);
                Assert.AreEqual(expected[i].Message, resultList[i].Message);
                Assert.AreEqual(expected[i].Parameter, resultList[i].Parameter);
                Assert.AreEqual(expected[i].Time, resultList[i].Time);
                Assert.AreEqual(expected[i].StackTrace, resultList[i].StackTrace);
            }
        }

        [TestMethod]
        public void AllErrorLogs_DBFAILDUMMY()
        {
            //Arrange
            AdminCheck(666);

            //ACT
            var result = (ViewResult)testAdminController.AllErrorLogs();

            //Assert
            Assert.AreEqual(result.ViewName, "ErrorLog");
        }

        [TestMethod]
        public void AllErrorLogs_NotAdmin()
        {
            //Arrange
            NotAdmin();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.AllErrorLogs();

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void AllErrorLogs_SessionNULL()
        {
            //Arrange
            SessionNull();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.AllErrorLogs();

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void SearchErrorLog_OK()
        {
            //Arrange
            AdminCheck();
            var expected = ExpectedErrorList;
            var searchString = "TestSTack";

            //ACT
            var result = (ViewResult)testAdminController.SearchErrorLog(searchString);
            var resultList = (List<ErrorLogVM>)result.Model;

            //Assert
            Assert.AreEqual(result.ViewName, "ErrorLog");
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Id, resultList[i].Id);
                Assert.AreEqual(expected[i].Message, resultList[i].Message);
                Assert.AreEqual(expected[i].Parameter, resultList[i].Parameter);
                Assert.AreEqual(expected[i].Time, resultList[i].Time);
                Assert.AreEqual(expected[i].StackTrace, resultList[i].StackTrace);
            }
        }

        [TestMethod]
        public void SearchErrorLog_NotAdmin()
        {
            //Arrange
            NotAdmin();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.SearchErrorLog(null);

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void SearchErrorLog_SessionNULL()
        {
            //Arrange
            SessionNull();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.SearchErrorLog(null);

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void SearchErrorLog_DBFAIL()
        {
            //Arrange
            AdminCheck(666);

            //ACT
            var result = (RedirectToRouteResult)testAdminController.SearchErrorLog(null);

            //ASSERT
            AssertDBEXE(result);
        }

        #endregion ErrorLog Test Methods

        //--------------------------------CHANGELOG

        #region ChangeLog Test Methods

        [TestMethod]
        public void AllChangeLogs_OK()
        {
            //Arrange
            AdminCheck();
            var expected = ExpectedChangeList();

            //ACT
            var result = (ViewResult)testAdminController.AllChangeLogs();
            var resultList = (List<ChangeLogVM>)result.Model;

            //Assert
            Assert.AreEqual(result.ViewName, "ChangeLog");
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].DateChanged, resultList[i].DateChanged);
                Assert.AreEqual(expected[i].EntityName, resultList[i].EntityName);
                Assert.AreEqual(expected[i].NewValue, resultList[i].NewValue);
                Assert.AreEqual(expected[i].OldValue, resultList[i].OldValue);
                Assert.AreEqual(expected[i].PrimaryKeyValue, resultList[i].PrimaryKeyValue);
                Assert.AreEqual(expected[i].PropertyName, resultList[i].PropertyName);
            }
        }

        [TestMethod]
        public void AllChangeLogs_NotAdmin()
        {
            //Arrange
            NotAdmin();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.AllChangeLogs();

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void AllChangeLogs_SessionNULL()
        {
            //Arrange
            SessionNull();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.AllChangeLogs();

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void AllChangeLogs_DBFAIL()
        {
            //Arrange
            AdminCheck(666);

            //ACT
            var result = (RedirectToRouteResult)testAdminController.AllChangeLogs();

            //ASSERT
            AssertDBEXE(result);
        }

        [TestMethod]
        public void SearchChangeLog_OK()
        {
            //Arrange
            AdminCheck();
            var expected = ExpectedChangeList();
            var searchString = "OldVal";

            //ACT
            var result = (ViewResult)testAdminController.SearchChangeLog(searchString);
            var resultList = (List<ChangeLogVM>)result.Model;

            //Assert
            Assert.AreEqual(result.ViewName, "ChangeLog");
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].DateChanged, resultList[i].DateChanged);
                Assert.AreEqual(expected[i].EntityName, resultList[i].EntityName);
                Assert.AreEqual(expected[i].NewValue, resultList[i].NewValue);
                Assert.AreEqual(expected[i].OldValue, resultList[i].OldValue);
                Assert.AreEqual(expected[i].PrimaryKeyValue, resultList[i].PrimaryKeyValue);
                Assert.AreEqual(expected[i].PropertyName, resultList[i].PropertyName);
            }
        }

        [TestMethod]
        public void SearchChangeLog_NotAdmin()
        {
            //Arrange
            NotAdmin();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.SearchChangeLog(null);

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void SearchChangeLog_SessionNULL()
        {
            //Arrange
            SessionNull();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.SearchChangeLog(null);

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void SearchChangeLog_DBFAIL()
        {
            //Arrange
            AdminCheck(666);

            //ACT
            var result = (RedirectToRouteResult)testAdminController.SearchChangeLog(null);

            //ASSERT
            AssertDBEXE(result);
        }

        #endregion ChangeLog Test Methods

        //--------------------------------PRICECLASS

        #region PriceClass Test Methods

        [TestMethod]
        public void PriceClassesList_OK()
        {
            //ARRANGE
            AdminCheck();
            var expected = ExpectedPriceClassList();

            //ACT
            var result = (ViewResult)testAdminController.PriceClassesList();
            var resultList = (List<PriceClassVM>)result.Model;

            //ASSERT
            Assert.AreEqual(result.ViewName, "");
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Id, resultList[i].Id);
                Assert.AreEqual(expected[i].Price, resultList[i].Price);
            }
        }

        [TestMethod]
        public void PriceClassesList_NotAdmin()
        {
            //ARRANGE
            NotAdmin();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.PriceClassesList();

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void PriceClassesList_SessionNULL()
        {
            //ARRANGE
            SessionNull();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.PriceClassesList();

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void PriceClassesList_DBFAIL()
        {
            //Arrange
            AdminCheck(666);

            //ACT
            var result = (RedirectToRouteResult)testAdminController.PriceClassesList();

            //ASSERT
            AssertDBEXE(result);
        }

        [TestMethod]
        public void PriceClassesChange_OK()
        {
            //ARRANGE
            AdminCheck();
            var expected = new List<PriceClassChangeVM>()
            {
                new PriceClassChangeVM()
                {
                    Id = 1,
                    Price = 50,
                    NewPrice = 0
                },
                new PriceClassChangeVM()
                {
                    Id = 2,
                    Price = 100,
                    NewPrice = 0
                },
                new PriceClassChangeVM()
                {
                    Id = 3,
                    Price = 150,
                    NewPrice = 0
                }
            };

            //ACT
            var result = (ViewResult)testAdminController.PriceClassesChange();
            var resultList = (List<PriceClassChangeVM>)result.Model;

            //ASSERT
            Assert.AreEqual(result.ViewName, "");
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Id, resultList[i].Id);
                Assert.AreEqual(expected[i].Price, resultList[i].Price);
                Assert.AreEqual(expected[i].NewPrice, resultList[i].NewPrice);
            }
        }

        [TestMethod]
        public void PriceClassesChange_NotAdmin()
        {
            //ARRANGE
            NotAdmin();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.PriceClassesChange();

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void PriceClassesChange_SessionNull()
        {
            //ARRANGE
            SessionNull();

            //ACT
            var result = (RedirectToRouteResult)testAdminController.PriceClassesChange();

            //ASSERT
            AssertAdminSessFail(result);
        }

        [TestMethod]
        public void PriceClassesChange_DBFAIL()
        {
            //Arrange
            AdminCheck(666);

            //ACT
            var result = (RedirectToRouteResult)testAdminController.PriceClassesChange();

            //ASSERT
            AssertDBEXE(result);
        }

        [TestMethod]
        public void PriceClassesChange_POST_OK()
        {
            //ARRANGE
            string command = "Lagre Nye Priser";
            var changeList = new List<PriceClassChangeVM>()
            {
                new PriceClassChangeVM()
                {
                    Id = 1,
                    Price = 50,
                    NewPrice = 0
                },
                new PriceClassChangeVM()
                {
                    Id = 2,
                    Price = 100,
                    NewPrice = 120
                },
                new PriceClassChangeVM()
                {
                    Id = 3,
                    Price = 150,
                    NewPrice = 170
                }
            };

            //ACT
            var result = (RedirectToRouteResult)testAdminController.PriceClassesChange(changeList, command);

            //ASSERT
            Assert.AreEqual(testAdminController.TempData["Message"], "Prisendring gjennomført!");
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(result.RouteValues.Values.First(), "PriceClassesList");
        }

        [TestMethod]
        public void PriceClassesChange_POST_Fail()
        {
            //ARRANGE
            string command = "Lagre Nye Priser";
            var changeFailList = new List<PriceClassChangeVM>()
            {
                new PriceClassChangeVM()
                {
                    Id = 0,
                    Price = 50,
                    NewPrice = 100
                },
                new PriceClassChangeVM()
                {
                    Id = 2,
                    Price = 100,
                    NewPrice = 120
                },
                new PriceClassChangeVM()
                {
                    Id = 3,
                    Price = 150,
                    NewPrice = 170
                }
            };

            //ACT
            var result = (RedirectToRouteResult)testAdminController.PriceClassesChange(changeFailList, command);

            //ASSERT
            Assert.AreEqual(testAdminController.TempData["Message"], "Noe gikk galt under endring av priser!");
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(result.RouteValues.Values.First(), "PriceClassesList");
        }

        [TestMethod]
        public void PriceClassesChange_POST_Validation_Error()
        {
            //ARRANGE
            var changeList = new List<PriceClassChangeVM>();
            string command = "Lagre Nye Priser";
            testAdminController.ViewData.ModelState.AddModelError("Ny Pris", "Ny pris ugyldig");

            //ACT
            var resultat = (ViewResult)testAdminController.PriceClassesChange(changeList, command);

            //ASSERT
            Assert.IsTrue(resultat.ViewData.ModelState.Count == 1);
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void PriceClassesChange_POST_Abort()
        {
            //ARRANGE
            var changeList = new List<PriceClassChangeVM>();
            string command = "Avbryt";

            //ACT
            var result = (RedirectToRouteResult)testAdminController.PriceClassesChange(changeList, command);

            //ASSERT
            Assert.AreEqual(result.RouteName, "");
            Assert.AreEqual(result.RouteValues.Values.First(), "PriceClassesList");
        }

        #endregion PriceClass Test Methods

        //--------------------------------EXPECTEDHELPERS----------------------------------//

        #region Expected Helpers

        //--------------------------------FILM HELPER METHODS
        /// <summary>
        /// Adapted from: https://stackoverflow.com/questions/4161175/how-do-i-get-the-count-of-attributes-that-an-object-has
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        public void CheckSLIObjects(List<SelectListItem> o1, List<SelectListItem> o2)
        {
            for (var i = 0; i < o1.Count; i++)
            {
                Assert.AreEqual(o1[i].Text, o2[i].Text);
                Assert.AreEqual(o1[i].Value, o2[i].Value);
            }
        }

        public List<AddFilmVM> ExAddFilmVMList()
        {
            var addFilmVMListe = new List<AddFilmVM>();

            List<SelectListItem> PriceClassSelectList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "1",
                    Text = "Prisklasse 1"
                },
                new SelectListItem
                {
                    Value = "2",
                    Text = "Prisklasse 2"
                }
            };

            List<SelectListItem> GenreSelectList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "1",
                    Text = "Sjanger 1"
                },
                new SelectListItem
                {
                    Value = "2",
                    Text = "Sjanger 2"
                },
                new SelectListItem
                {
                    Value = "3",
                    Text = "Sjanger 3"
                },
                new SelectListItem
                {
                    Value = "4",
                    Text = "Sjanger 4"
                }
            };

            List<GenreVM> Genres = new List<GenreVM>
            {
                new GenreVM
                {
                    Id = 1,
                    Name = "Sjanger 1"
                },
                new GenreVM
                {
                    Id = 2,
                    Name = "Sjanger 2"
                }
            };

            int[] genreIds = { 1, 2 };

            List<GenreVM> Genres2 = new List<GenreVM>
            {
                new GenreVM
                {
                    Id = 3,
                    Name = "Sjanger 3"
                },
                new GenreVM
                {
                    Id = 4,
                    Name = "Sjanger 4"
                }
            };

            int[] genreIds2 = { 3, 4 };

            AddFilmVM newFilm = new AddFilmVM()
            {
                FilmId = 1,
                Title = "FilmTittel",
                Description = "Beskrivelse av filmen",
                ImgURL = "www.google.com/bilde.jpg",
                CurrentGenres = Genres2,
                Active = true,
                FilmPriceClassId = 1,
                PriceId = 1,
                PriceSelectList = PriceClassSelectList,
                GenreIDs = genreIds2,
                GenreSelectList = GenreSelectList
            };
            newFilm.JsonSerialize = JsonConvert.SerializeObject(newFilm);

            AddFilmVM newFilm2 = new AddFilmVM()
            {
                FilmId = 2,
                Title = "IkkeFilmTittel",
                Description = "Beskrivelse av filmen 2",
                ImgURL = "www.google.com/bilde2.jpg",
                CurrentGenres = Genres,
                Active = true,
                FilmPriceClassId = 1,
                PriceId = 1,
                PriceSelectList = PriceClassSelectList,
                GenreIDs = genreIds,
                GenreSelectList = GenreSelectList
            };
            newFilm2.JsonSerialize = JsonConvert.SerializeObject(newFilm2);

            List<int> ids = new List<int>();
            for (int i = 0; i < Genres.Count; i++)
            {
                ids.Add(Genres[i].Id);
            }
            newFilm.CurrFilmGenreIds = ids;

            var size = 10;
            for (var i = 0; i < size; i++)
            {
                addFilmVMListe.Add(newFilm);
                addFilmVMListe.Add(newFilm2);
            }

            return addFilmVMListe;
        }

        public AddFilmVM ExEmptyAddFilmVM()
        {
            var addFilmVM = new AddFilmVM();

            List<SelectListItem> PriceClassSelectList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "1",
                    Text = "Prisklasse 1"
                },
                new SelectListItem
                {
                    Value = "2",
                    Text = "Prisklasse 2"
                }
            };

            List<SelectListItem> GenreSelectList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "1",
                    Text = "Sjanger 1"
                },
                new SelectListItem
                {
                    Value = "2",
                    Text = "Sjanger 2"
                }
            };
            addFilmVM.PriceSelectList = PriceClassSelectList;
            addFilmVM.GenreSelectList = GenreSelectList;

            return addFilmVM;
        }

        public AddFilmVM ExAddFilmVM = new AddFilmVM()
        {
            Active = true,
            CurrFilmGenreIds = null,
            CurrentGenres = null,
            Description = "Beskrivelse",
            FilmId = 0,
            FilmPriceClassId = 0,
            GenreIDs = new int[] { 1, 2 },
            GenreSelectList = null,
            ImgURL = "www.google.com/bilde.jpg",
            JsonSerialize = null,
            PriceId = 3,
            PriceSelectList = null,
            Title = "FilmTittel"
        };

        public AddFilmVM ExFailAddFilmVM = new AddFilmVM()
        {
            Active = true,
            CurrFilmGenreIds = null,
            CurrentGenres = null,
            Description = "Beskrivelse",
            FilmId = 0,
            FilmPriceClassId = 0,
            GenreIDs = new int[] { 1, 2 },
            GenreSelectList = null,
            ImgURL = "www.google.com/bilde.jpg",
            JsonSerialize = null,
            PriceId = 3,
            PriceSelectList = null,
            Title = "IkkeFilmTittel"
        };

        public AddFilmVM ExNotValidAddFilmVM = new AddFilmVM()
        {
            Active = true,
            CurrFilmGenreIds = null,
            CurrentGenres = null,
            Description = "Beskrivelse",
            FilmId = 0,
            FilmPriceClassId = 0,
            GenreIDs = new int[] { 1, 2 },
            GenreSelectList = null,
            ImgURL = "www.google.com/bilde.jpg",
            JsonSerialize = null,
            PriceId = 3,
            PriceSelectList = null,
            Title = ""
        };

        public AddFilmVM ExEditAddFilmVM()
        {
            AddFilmVM ExAddFilmVM = new AddFilmVM()
            {
                Active = true,
                CurrFilmGenreIds = null,
                CurrentGenres = null,
                Description = "Beskrivelse",
                FilmId = 0,
                FilmPriceClassId = 0,

                GenreSelectList = null,
                ImgURL = "www.google.com/bilde.jpg",
                JsonSerialize = null,
                PriceId = 1,
                PriceSelectList = null,
                Title = "FilmTittel"
            };
            List<int> ids = new List<int>();
            for (int i = 1; i < 4; i++)
            {
                ids.Add(i);
            }
            ExAddFilmVM.CurrFilmGenreIds = ids;

            List<GenreVM> Genres = new List<GenreVM>
            {
                new GenreVM
                {
                    Id = 1,
                    Name = "Sjanger 1"
                },
                new GenreVM
                {
                    Id = 2,
                    Name = "Sjanger 2"
                }
            };
            ExAddFilmVM.CurrentGenres = Genres;
            int[] genreIds = { 1, 2 };
            ExAddFilmVM.GenreIDs = genreIds;

            List<SelectListItem> PriceClassSelectList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "1",
                    Text = "Prisklasse 1"
                },
                new SelectListItem
                {
                    Value = "2",
                    Text = "Prisklasse 2"
                }
            };

            List<SelectListItem> GenreSelectList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "1",
                    Text = "Sjanger 1"
                },
                new SelectListItem
                {
                    Value = "2",
                    Text = "Sjanger 2"
                }
            };
            ExAddFilmVM.PriceSelectList = PriceClassSelectList;
            ExAddFilmVM.GenreSelectList = GenreSelectList;

            return ExAddFilmVM;
        }

        public List<AddFilmVM> ExSearch_FilmTittel()
        {
            var addFilmVMListe = new List<AddFilmVM>();

            List<SelectListItem> PriceClassSelectList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "1",
                    Text = "Prisklasse 1"
                },
                new SelectListItem
                {
                    Value = "2",
                    Text = "Prisklasse 2"
                }
            };

            List<SelectListItem> GenreSelectList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "1",
                    Text = "Sjanger 1"
                },
                new SelectListItem
                {
                    Value = "2",
                    Text = "Sjanger 2"
                },
                new SelectListItem
                {
                    Value = "3",
                    Text = "Sjanger 3"
                },
                new SelectListItem
                {
                    Value = "4",
                    Text = "Sjanger 4"
                }
            };

            List<GenreVM> Genres = new List<GenreVM>
            {
                new GenreVM
                {
                    Id = 1,
                    Name = "Sjanger 1"
                },
                new GenreVM
                {
                    Id = 2,
                    Name = "Sjanger 2"
                }
            };

            int[] genreIds = { 1, 2 };

            List<GenreVM> Genres2 = new List<GenreVM>
            {
                new GenreVM
                {
                    Id = 3,
                    Name = "Sjanger 3"
                },
                new GenreVM
                {
                    Id = 4,
                    Name = "Sjanger 4"
                }
            };

            int[] genreIds2 = { 3, 4 };

            AddFilmVM newFilm = new AddFilmVM()
            {
                FilmId = 1,
                Title = "FilmTittel",
                Description = "Beskrivelse av filmen",
                ImgURL = "www.google.com/bilde.jpg",
                CurrentGenres = Genres2,
                Active = true,
                FilmPriceClassId = 1,
                PriceId = 1,
                PriceSelectList = PriceClassSelectList,
                GenreIDs = genreIds2,
                GenreSelectList = GenreSelectList
            };
            newFilm.JsonSerialize = JsonConvert.SerializeObject(newFilm);

            List<int> ids = new List<int>();
            for (int i = 0; i < Genres.Count; i++)
            {
                ids.Add(Genres[i].Id);
            }
            newFilm.CurrFilmGenreIds = ids;

            var size = 10;
            for (var i = 0; i < size; i++)
            {
                addFilmVMListe.Add(newFilm);
            }

            return addFilmVMListe;
        }

        //-------------------USER

        public List<UserVM> ExpectedUserList()
        {
            var expected = new List<UserVM>();

            var user = new UserVM()
            {
                Active = false,
                Id = 3,
                FirstName = "Jan",
                SurName = "Teigen",
                Address = "Nøtterøys veien 69",
                Postal = "Tønsberg",
                PostalNr = "3162",
                CreatedDate = "21.10.2018 14:49:31",
                PassWord = "MilEtterMil",
                PassWordRepeat = "MilEtterMil",
                Email = "teigen@gmail.com",
                PhoneNr = "81549300",
            };

            var user2 = new UserVM()
            {
                Active = false,
                Id = 4,
                FirstName = "Petter",
                SurName = "Dass",
                Address = "Olav Ryes gate 90",
                Postal = "Tønsberg",
                PostalNr = "3162",
                CreatedDate = "23.10.2018 15:49:31",
                PassWord = "DusjForheng",
                PassWordRepeat = "DusjForheng",
                Email = "Petter@Dass.com",
                PhoneNr = "81122200",
            };

            expected.Add(user);
            expected.Add(user2);

            return expected;
        }

        public UserVM ExpectedUser = new UserVM()
        {
            Active = false,
            Id = 3,
            FirstName = "Jan",
            SurName = "Teigen",
            Address = "Nøtterøys veien 69",
            Postal = "Tønsberg",
            PostalNr = "3162",
            CreatedDate = "21.10.2018 14:49:31",
            PassWord = "MilEtterMil",
            PassWordRepeat = "MilEtterMil",
            Email = "teigen@gmail.com",
            PhoneNr = "81549300",
        };

        //-----------------------ORDERS
        public List<ExpandedOrderVM> ExpectedAllOrdersResult()
        {
            var expectedList = new List<ExpandedOrderVM>();
            var expandedOrder = new ExpandedOrderVM()
            {
                FirstName = "Ola",
                SurName = "Nordmann",
                Email = "ola@nordmann.no",
            };
            var expandedOrder2 = new ExpandedOrderVM()
            {
                FirstName = "Tore",
                SurName = "Sagen",
                Email = "tore@sagen.no",
            };
            OrderVM order = new OrderVM()
            {
                OrderNr = 0,
                Date =
                    new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 10, 40, 00).ToString()
            };

            OrderVM order2 = new OrderVM()
            {
                OrderNr = 1,
                Date =
                    new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 10, 20, 00).ToString()
            };
            OrdreLinjeVM orderline = new OrdreLinjeVM()
            {
                Price = 49,
                Title = "Mad Max"
            };
            OrdreLinjeVM orderline2 = new OrdreLinjeVM()
            {
                Price = 99,
                Title = "Tomb Raider"
            };
            OrdreLinjeVM orderline3 = new OrdreLinjeVM()
            {
                Price = 129,
                Title = "Kontiki"
            };
            List<OrdreLinjeVM> orderlines = new List<OrdreLinjeVM>();
            List<OrdreLinjeVM> orderlines2 = new List<OrdreLinjeVM>();

            orderlines.Add(orderline);
            orderlines.Add(orderline3);

            orderlines2.Add(orderline);
            orderlines2.Add(orderline2);
            orderlines2.Add(orderline3);

            expandedOrder.Order = order;
            expandedOrder.Order.OrderLines = orderlines;
            expandedOrder.Order.TotalPrice = 49 + 129;

            expandedOrder2.Order = order2;
            expandedOrder2.Order.OrderLines = orderlines2;
            expandedOrder2.Order.TotalPrice = 49 + 99 + 129;

            expectedList.Add(expandedOrder);
            expectedList.Add(expandedOrder2);

            return expectedList;
        }

        public List<ExpandedOrderVM> ExpectedOrderSearchList()
        {
            var expectedList = new List<ExpandedOrderVM>();
            var expandedOrder = new ExpandedOrderVM()
            {
                FirstName = "Ola",
                SurName = "Nordmann",
                Email = "ola@nordmann.no",
            };

            OrderVM order = new OrderVM()
            {
                OrderNr = 0,
                Date =
                    new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 10, 40, 00).ToString()
            };

            OrdreLinjeVM orderline = new OrdreLinjeVM()
            {
                Price = 49,
                Title = "Mad Max"
            };
            OrdreLinjeVM orderline2 = new OrdreLinjeVM()
            {
                Price = 129,
                Title = "Kontiki"
            };
            List<OrdreLinjeVM> orderlines = new List<OrdreLinjeVM>
            {
                orderline,
                orderline2
            };

            expandedOrder.Order = order;
            expandedOrder.Order.OrderLines = orderlines;
            expandedOrder.Order.TotalPrice = 49 + 129;

            expectedList.Add(expandedOrder);

            return expectedList;
        }

        public List<ExpandedOrderVM> ExpectedRefundOrderResult()
        {
            var expectedList = new List<ExpandedOrderVM>();

            var expandedOrder2 = new ExpandedOrderVM()
            {
                FirstName = "Tore",
                SurName = "Sagen",
                Email = "tore@sagen.no",
            };
            OrderVM order2 = new OrderVM()
            {
                OrderNr = 1,
                Date =
                    new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 10, 20, 00).ToString()
            };
            OrdreLinjeVM orderline = new OrdreLinjeVM()
            {
                Price = 49,
                Title = "Mad Max"
            };
            OrdreLinjeVM orderline2 = new OrdreLinjeVM()
            {
                Price = 99,
                Title = "Tomb Raider"
            };
            OrdreLinjeVM orderline3 = new OrdreLinjeVM()
            {
                Price = 129,
                Title = "Kontiki"
            };
            List<OrdreLinjeVM> orderlines2 = new List<OrdreLinjeVM>
            {
                orderline,
                orderline2,
                orderline3
            };

            expandedOrder2.Order = order2;
            expandedOrder2.Order.OrderLines = orderlines2;
            expandedOrder2.Order.TotalPrice = 49 + 99 + 129;

            expectedList.Add(expandedOrder2);

            return expectedList;
        }

        public List<ExpandedOrderVM> ExpectedRefundFilmResult()
        {
            var expectedList = new List<ExpandedOrderVM>();
            var expandedOrder = new ExpandedOrderVM()
            {
                FirstName = "Ola",
                SurName = "Nordmann",
                Email = "ola@nordmann.no",
            };
            var expandedOrder2 = new ExpandedOrderVM()
            {
                FirstName = "Tore",
                SurName = "Sagen",
                Email = "tore@sagen.no",
            };
            OrderVM order = new OrderVM()
            {
                OrderNr = 0,
                Date =
                    new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 10, 40, 00).ToString()
            };

            OrderVM order2 = new OrderVM()
            {
                OrderNr = 1,
                Date =
                    new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 10, 20, 00).ToString()
            };
            OrdreLinjeVM orderline = new OrdreLinjeVM()
            {
                Price = 49,
                Title = "Mad Max"
            };
            OrdreLinjeVM orderline2 = new OrdreLinjeVM()
            {
                Price = 99,
                Title = "Tomb Raider"
            };
            OrdreLinjeVM orderline3 = new OrdreLinjeVM()
            {
                Price = 129,
                Title = "Kontiki"
            };
            List<OrdreLinjeVM> orderlines = new List<OrdreLinjeVM>();
            List<OrdreLinjeVM> orderlines2 = new List<OrdreLinjeVM>();

            orderlines.Add(orderline3);

            orderlines2.Add(orderline);
            orderlines2.Add(orderline2);
            orderlines2.Add(orderline3);

            expandedOrder.Order = order;
            expandedOrder.Order.OrderLines = orderlines;
            expandedOrder.Order.TotalPrice = 129;

            expandedOrder2.Order = order2;
            expandedOrder2.Order.OrderLines = orderlines2;
            expandedOrder2.Order.TotalPrice = 49 + 99 + 129;

            expectedList.Add(expandedOrder);
            expectedList.Add(expandedOrder2);

            return expectedList;
        }

        //--------------------ERRORS
        public List<ErrorLogVM> ExpectedErrorList = new List<ErrorLogVM>()
        {
            new ErrorLogVM()
            {
                Id = 1,
                Message = "Test1",
                Parameter = "TestParameter1",
                StackTrace = "TestSTack",
                Time = "20.10.2018 01:01:01"
            },
            new ErrorLogVM()
            {
                Id = 2,
                Message = "Test2",
                Parameter = "TestParameter2",
                StackTrace = "TestSTack",
                Time = "20.10.2018 01:01:02"
            },
            new ErrorLogVM()
            {
                Id = 3,
                Message = "Test3",
                Parameter = "TestParameter3",
                StackTrace = "TestSTack",
                Time = "20.10.2018 01:01:03"
            }
        };

        //-----------------------------CHANGELOG

        public List<ChangeLogVM> ExpectedChangeList()
        {
            return new List<ChangeLogVM>()
            {
                new ChangeLogVM()
                {
                   DateChanged = "20.10.2018 01:01:02",
                   EntityName = "EntityName1",
                   NewValue ="NewVal1",
                   OldValue ="OldVal",
                   PrimaryKeyValue = "PrimaryKeyVal1",
                   PropertyName = "PropertyName1"
                }, new ChangeLogVM()
                {
                   DateChanged = "21.10.2018 01:01:05",
                   EntityName = "EntityName1",
                   NewValue ="NewVal1",
                   OldValue ="OldVal",
                   PrimaryKeyValue = "PrimaryKeyVal1",
                   PropertyName = "PropertyName1"
                }, new ChangeLogVM()
                {
                   DateChanged = "22.10.2018 01:01:10",
                   EntityName = "EntityName1",
                   NewValue ="NewVal1",
                   OldValue ="OldVal",
                   PrimaryKeyValue = "PrimaryKeyVal1",
                   PropertyName = "PropertyName1"
                }
            };
        }

        //---------------PRICECLASSES

        public List<PriceClassVM> ExpectedPriceClassList()
        {
            return new List<PriceClassVM>()
            {
                new PriceClassVM(){
                Id = 1,
                Price = 50
                },
                 new PriceClassVM(){
                Id = 2,
                Price = 100
                },
                new PriceClassVM(){
                Id = 3,
                Price = 150
                }
            };
        }

        #endregion Expected Helpers
    }
}