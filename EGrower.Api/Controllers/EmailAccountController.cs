using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EGrower.Infrastructure.Commands.EmailAccount;
using EGrower.Infrastructure.Commands.SendedEmail;
using EGrower.Infrastructure.Repositories.Interfaces;
using EGrower.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EGrower.Api.Controllers {
    public class EmailAccountController : ApiUserController {
        private readonly IEmailAccountService _emailAccountService;
        private readonly IEmailAccountIMapService _emailAccountIMapService;
        private readonly IUserRepository _userRepository;

        public EmailAccountController (IEmailAccountService emailAccountService,
            IEmailAccountIMapService emailAccountIMapService,
            IUserRepository userRepository) {
            _userRepository = userRepository;
            _emailAccountService = emailAccountService;
            _emailAccountIMapService = emailAccountIMapService;
        }

        [HttpGet]
        public async Task<IActionResult> Get (string email) {
            var emailAccount = await _emailAccountIMapService.BrowseAsync (email);
            return Json (emailAccount);
        }

        [Authorize]
        [HttpPost ("send")]
        public async Task<IActionResult> Send ([FromBody] CreateSendedEmail command) {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            var user = await _userRepository.GetWithEmailAccountsAsync (UserId);
            if (user == null)
                return Unauthorized ();
            try {
                // await _emailAccountService.CreateAsync (user, command.Email, command.Password, command.ImapHost, command.ImapPort, command.SmtpHost, command.SmtpPort);
            } catch (Exception e) {
                return BadRequest (e.Message);
            }
            return StatusCode (201);
        }
    }
}