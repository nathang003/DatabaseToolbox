using Microsoft.AspNet.Identity;
using ToolboxDataManager.Library.DataAccess;
using ToolboxDataManager.Library.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ToolboxDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        [HttpGet]
        public UserModel GetById()
        {

            Console.WriteLine("Entering ToolboxDataManager.Controllers.UserController.GetById");

            // Can't pass in user id or the user could guess at these.
            string userId = RequestContext.Principal.Identity.GetUserId();
            UserData data = new UserData();

            return data.GetUserById(userId).First();
        }
    }
}