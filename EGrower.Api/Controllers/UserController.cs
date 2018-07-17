using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EGrower.Infrastructure.Commands.EmailAccount;
using EGrower.Infrastructure.Commands.SendedEmail;
using EGrower.Infrastructure.Repositories.Interfaces;
using EGrower.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EGrower.Api.Controllers {
    [Authorize]
    public class UserController : ApiUserController {
        // private readonly IUserService _userService;

        /*  public UserController (IUserService userService) {
             _userService = userService;

         } */

        // [HttpGet ("profile")]
        // public async Task<IActionResult> AddEmailAccount () {
        //     if (!await _userService.UserExistAsync (UserId))
        //         return Unauthorized ();
        //     try {
        //         var activeUserWithEmailAccount = await _userService.GetActiveWithEmailAccountsAsync (UserId);
        //         return Json (activeUserWithEmailAccount);

        //     } catch (Exception e) {
        //         return BadRequest (e.Message);
        //     }
        // }
    }
}