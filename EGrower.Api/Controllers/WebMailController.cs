using System;
using System.Linq;
using System.Threading.Tasks;
using EGrower.Infrastructure.Aggregates.Interfaces;
using EGrower.Infrastructure.Commands.EmailAccount;
using EGrower.Infrastructure.Commands.SendedEmail;
using EGrower.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EGrower.Api.Controllers {

    [Authorize]
    public class WebMailController : ApiUserController {
        private readonly IEmailAccountService _emailAccountService;
        private readonly IUserService _userService;
        private readonly ISendedEmailMessageService _sendedEmailMessageService;
        private readonly IUserAggregate _userAggregate;
        private readonly IEmailMessageService _emailMessageService;

        public WebMailController (IEmailAccountService emailAccountService,
            IUserService userService,
            IEmailMessageService emailMessageService,
            ISendedEmailMessageService sendedEmailMessageService,
            IUserAggregate userAggregate) {
            _emailAccountService = emailAccountService;
            _userService = userService;
            _emailMessageService = emailMessageService;
            _sendedEmailMessageService = sendedEmailMessageService;
            _userAggregate = userAggregate;
        }

        [HttpPost ("emailaccounts")]
        public async Task<IActionResult> AddEmailAccount ([FromBody] CreateEmailAccount command) {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            if (await _emailAccountService.ExistsByEmailAndUserIdAsync (UserId, command.Email)) {
                ModelState.AddModelError ("Email", "Email address is already taken.");
                return BadRequest (ModelState);
            }
            try {
                await _emailAccountService.CreateAsync (UserId, command.Email, command.Password, command.ImapHost, command.ImapPort, command.SmtpHost, command.SmtpPort);
                return StatusCode (201);

            } catch (Exception e) {
                return BadRequest (e.Message);
            }
        }

        [HttpGet ("emailaccounts")]
        public async Task<IActionResult> GetUserEmailAccounts () {
            try {
                var userEmailAccounts = await _emailAccountService.GetAllWIthUserByUserIdAsync (UserId);
                if (userEmailAccounts.Count () > 0)
                    return Json (userEmailAccounts);
                return Ok ("Lack of email accounts");
            } catch (Exception e) {
                return BadRequest (e.Message);
            }
        }

        [HttpDelete ("emailaccounts/{emailAccountId}")]
        public async Task<IActionResult> DeleteUserEmailAccount (int emailAccountId) {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            try {
                await _emailAccountService.DeleteAsync (UserId, emailAccountId);
                return Ok ();
            } catch (Exception e) {
                return BadRequest (e.Message);
            }
        }

        [HttpGet ("emailscontacts")]
        public async Task<IActionResult> GetEmailsContacts () {
            try {
                var emailsContacts = await _userAggregate.GetEmailAccountsContactsAsync (UserId);
                return Json (emailsContacts);
            } catch (Exception e) {
                return BadRequest (e.Message);
            }
        }

        [HttpGet ("receivedemails")]
        public async Task<IActionResult> GetUndeletedEmails () {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            try {
                var emailMessages = await _emailMessageService.GetAllUnDeletedForUserAsync (UserId);
                return Json (emailMessages);
            } catch (Exception e) {
                return BadRequest (e.Message);
            }
        }

        [HttpGet ("receivedemails/{emailId}")]
        public async Task<IActionResult> GetEmail (int emailId) {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            try {
                var emailMessage = await _emailMessageService.GetUndeletedForUserAsync (UserId, emailId);
                return Json (emailMessage);
            } catch (Exception e) {
                return BadRequest (e.Message);
            }
        }

        [HttpGet ("receivedemails/attachments/{emailId}")]
        public async Task<IActionResult> DownloadAttachments (int emailId) {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            try {
                var attachmentsStream = await _emailMessageService.GetUserAttachmentsAsync (UserId, emailId);
                return File (attachmentsStream.ToArray (), "application/zip", "Attachments.zip");
            } catch (Exception e) {
                return BadRequest (e.Message);
            }
        }

        [HttpPut ("receivedemails/replyto/{emailMessageId}")]
        public async Task<IActionResult> ReplyToAsync (int emailMessageId, [FromForm] ReplyToEmail command) {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            try {
                await _emailMessageService.ReplyToAsync (emailMessageId, UserId, command.TextHTMLBody, command.ReplyToAll, command.Attachments, command.Cc, command.Bcc);
                return NoContent ();
            } catch (Exception e) {
                return BadRequest (e.Message);
            }
        }

        [HttpPut ("receivedemails/delete/{emailMessageId}")]
        public async Task<IActionResult> DeleteEmailMessage (int emailMessageId) {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            try {
                await _emailMessageService.DeleteUserMessageAsync (emailMessageId, UserId);
                return NoContent ();
            } catch (Exception e) {
                return BadRequest (e.Message);
            }
        }

        [HttpPost ("sendedemails")]
        public async Task<IActionResult> SendEmailMessage ([FromForm] CreateSendedEmail command) {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            try {
                await _sendedEmailMessageService.SendAsync (UserId, command.From, command.To, command.Cc, command.Bcc, command.Subject, command.TextHTMLBody, command.Attachments);
                return StatusCode (201);
            } catch (Exception e) {
                return BadRequest (e.Message);
            }
        }

        [HttpGet ("sendedemails")]
        public async Task<IActionResult> GetAllSendedEmails () {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            try {
                var sendedEmails = await _sendedEmailMessageService.GetAllByUserIdAsync (UserId);
                return Json (sendedEmails);
            } catch (Exception e) {
                return BadRequest (e.Message);
            }
        }

        [HttpGet ("sendedemails/attachments/{emailId}")]
        public async Task<IActionResult> DownloadSendedAttachments (int emailId) {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            try {
                var attachmentsStream = await _sendedEmailMessageService.GetUserSendedAttachmentsAsync (UserId, emailId);
                return File (attachmentsStream.ToArray (), "application/zip", "Attachments.zip");
            } catch (Exception e) {
                return BadRequest (e.Message);
            }
        }

        [HttpPut ("emailmessages/markasread/{emailId}")]
        public async Task<IActionResult> MarEmailMessageAsRead (int emailId) {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            try {
                await _emailMessageService.MarkAsReadAsync (UserId, emailId);
                return NoContent ();
            } catch (Exception e) {
                return BadRequest (e.Message);
            }
        }

        [HttpPut ("emailmessages/markasunread/{emailId}")]
        public async Task<IActionResult> MarEmailMessageAsUnread (int emailId) {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            try {
                await _emailMessageService.MarkAsUnreadAsync (UserId, emailId);
                return NoContent ();
            } catch (Exception e) {
                return BadRequest (e.Message);
            }
        }

        [HttpGet ("trash")]
        public async Task<IActionResult> GetEmailMessagesForTrash () {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            try {
                var deletedEmailMessages = await _emailMessageService.GetAllDeletedForUserAsync (UserId);
                return Json (deletedEmailMessages);
            } catch (Exception e) {
                return BadRequest (e.Message);
            }
        }

        [HttpGet ("trash/{emailId}")]
        public async Task<IActionResult> GetDeletedEmail (int emailId) {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            try {
                var emailMessage = await _emailMessageService.GetDeletedForUserAsync (UserId, emailId);
                return Json (emailMessage);
            } catch (Exception e) {
                return BadRequest (e.Message);
            }
        }

        [HttpPut ("trash/restore/{emailMessageId}")]
        public async Task<IActionResult> RestoreEmailMessage (int emailMessageId) {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            try {
                await _emailMessageService.RestoreMessageFromTrashAsync (emailMessageId, UserId);
                return Ok ();
            } catch (Exception e) {
                return BadRequest (e.Message);
            }
        }

        [HttpDelete ("trash/delete/{emailMessageId}")]
        public async Task<IActionResult> DeleteEmailMessageFromTrash (int emailMessageId) {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            try {
                await _emailMessageService.RemoveDeletedUserMessageAsync (emailMessageId, UserId);
                return Ok ();
            } catch (Exception e) {
                return BadRequest (e.Message);
            }
        }

        [HttpPut ("newemails")]
        public async Task<IActionResult> GetNewEmails () {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            try {
                await _userAggregate.GetNewEmailsByUserIdAsync (UserId);
                return StatusCode (201);
            } catch (Exception e) {
                return BadRequest (e.Message);
            }
        }

        // [HttpPost ("newEmails/users")]
        // public async Task<IActionResult> GetNewEmailsForAllUsers () {
        //     if (!ModelState.IsValid)
        //         return BadRequest (ModelState);
        //     try {
        //         await E
        //         return Ok (201);
        //     } catch (Exception e) {
        //         return BadRequest (e.Message);
        //     }
        // }

        #region Extensions
        // [HttpGet ("getEmails/{accountId}")]
        // public async Task<IActionResult> GetEmailsForEmailAccount (int accountId) {
        //     if (!ModelState.IsValid)
        //         return BadRequest (ModelState);
        //     if (!await _userService.UserExistAsync (UserId))
        //         return Unauthorized ();
        //     try {
        //         var emailMessages = await _emailMessageService.GetAllForEmailAccountAsync (accountId);
        //         return Json (emailMessages);
        //     } catch (Exception e) {
        //         return BadRequest (e.Message);
        //     }
        // }

        // [HttpGet ("getEmail/{emailAccountId}/{emailId}")]
        // public async Task<IActionResult> GetEmailAccountEmail (int emailAccountId, int emailId) {
        //     if (!ModelState.IsValid)
        //         return BadRequest (ModelState);
        //     if (!await _userService.UserExistAsync (UserId))
        //         return Unauthorized ();
        //     try {
        //         var emailMessage = await _emailMessageService.GetForEmailAccountAsync (emailAccountId, emailId);
        //         return Json (emailMessage);
        //     } catch (Exception e) {
        //         return BadRequest (e.Message);
        //     }
        // }

        // [HttpGet ("GetEmailAttachment/{emailId}")]
        // public async Task<IActionResult> GetEmailAttachment (int emailId) {
        //     if (!ModelState.IsValid)
        //         return BadRequest (ModelState);
        //     if (!await _userService.UserExistAsync (UserId))
        //         return Unauthorized ();
        //     try {
        //         var emailMessage = await _emailMessageService.GetAsync (UserId, emailId);
        //         return Json (emailMessage);
        //     } catch (Exception e) {
        //         return BadRequest (e.Message);
        //     }
        // }

        // [HttpGet ("downloadAttachment/{emailAccountId}/{emailId}")]
        // public async Task<IActionResult> DwnloadAttachments (int emailAccountId, int emailId) {
        //     if (!ModelState.IsValid)
        //         return BadRequest (ModelState);
        //     if (!await _userService.UserExistAsync (UserId))
        //         return Unauthorized ();
        //     try {
        //         var attachmentsStream = await _emailMessageService.GetAttachmentsForEmailAccountAsync (emailAccountId, emailId);
        //         if (attachmentsStream != null)
        //             return File (attachmentsStream.ToArray (), "application/zip", "Attachments.zip");
        //         else
        //             BadRequest ("Lack of attachments to download");
        //     } catch (Exception e) {
        //         return BadRequest (e.Message);
        //     }
        //     return BadRequest ();
        // }

        //ok

        // [HttpPost ("sendEmail")]
        // public async Task<IActionResult> SendEmail ([FromForm] CreateSendedEmail command) {
        //     if (!ModelState.IsValid)
        //         return BadRequest (ModelState);
        //     if (!await _userService.UserExistAsync (UserId))
        //         return Unauthorized ();
        //     try {
        //         var emailMessages = await _emailMessageService.GetAllForUserAsync (UserId);
        //         return Json (emailMessages);
        //     } catch (Exception e) {
        //         return BadRequest (e.Message);
        //     }
        // }
        #endregion
    }
}