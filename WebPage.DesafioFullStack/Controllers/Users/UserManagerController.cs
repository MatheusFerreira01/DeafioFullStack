using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Models.DesafioFullStack;
using System.Net.Http.Headers;
using System.Net.Http;
using WebPage.DesafioFullStack.Integration;
using WebPage.DesafioFullStack.Models;

namespace WebPage.DesafioFullStack.Controllers.Users
{
    public class UserManagerController : Controller
    {

        public IActionResult UserManagement()
        {
            UserManagementPageModel pageModel = new UserManagementPageModel();

            List<User> users = UserManagerIntegration.GetUsers();

            pageModel.ListSelectUsers = users;

            return View(pageModel);
        }
        [HttpPost]
        public IActionResult Add([FromBody] UserManagementPageModel pageModel)
        {
            try
            {
                var isNewUser = ValidateExistence(pageModel);

                if (isNewUser)
                {
                    UserManagerIntegration.Add(pageModel);

                    return Json(new { success = true, message = "O usuario foi cadastrado com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Esse usuario já existe" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro: " + ex.Message });
            }
        }
        [HttpPost]
        public IActionResult Edit([FromBody] UserManagementPageModel pageModel)
        {
            try
            {
                if (pageModel.Username == "admin")
                {
                    return Json(new { success = false, message = "Este usuario não pode ser manipulado" });
                }
                pageModel.Username = pageModel.SelectedUser;

                var isNewUser = ValidateExistence(pageModel);

                if (!isNewUser)
                {
                    UserManagerIntegration.Add(pageModel);

                    return Json(new { success = true, message = "O usuario foi alterado com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Esse usuario já existe" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro: " + ex.Message });
            }
        }
        [HttpPost]
        public IActionResult Remove([FromBody] UserManagementPageModel pageModel)
        {
            try
            {
                if (pageModel.Username == "admin")
                {
                    return Json(new { success = false, message = "Este usuario não pode ser manipulado" });
                }
                pageModel.Username = pageModel.SelectedUser;

                var isNewUser = ValidateExistence(pageModel);

                if (!isNewUser)
                {
                    UserManagerIntegration.Remove(pageModel);

                    return Json(new { success = true, message = "O usuario foi removido com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Esse usuario já existe" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro: " + ex.Message });
            }
        }


        public bool ValidateExistence(UserManagementPageModel pageModel)
        {
            bool isConfirmed = true;

            List<User> existentUser = UserManagerIntegration.GetUsers();

            if (existentUser.Where(x => x.Username == pageModel.Username).Any())
            {
                isConfirmed = false;
            }

            return isConfirmed;
        }
    }
}
