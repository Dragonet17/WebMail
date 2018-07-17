using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace EGrower.Api.Controllers {
    [Route ("api/[controller]")]
    public class ApiUserController : Controller {
        protected int UserId => User?.Identity?.IsAuthenticated == true ?
            int.Parse (User.Claims.ToList ().First ().Value) :
            0;
        protected string UserEmail => User?.Identity?.IsAuthenticated == true ?
            (User.Claims.ToList ().ElementAt (1).Value) :
            "";
    }
}