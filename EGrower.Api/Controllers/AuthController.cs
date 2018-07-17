using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EGrower.Core.Domains;
using EGrower.Infrastructure.Commands.SendedEmail;
using EGrower.Infrastructure.Commands.User;
using EGrower.Infrastructure.Extension.FileModel;
using EGrower.Infrastructure.Extension.JWT;
using EGrower.Infrastructure.Extension.Zip;
using EGrower.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EGrower.Api.Controllers {
    public class AuthController : ApiUserController {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IJWTSettings _jwtSettings;

        public AuthController (IAuthService authService,
            IUserService userService,
            IJWTSettings jwtSettings) {
            _jwtSettings = jwtSettings;
            _authService = authService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index () {
            await Task.CompletedTask;
            return Json ("Aplikacja dziala");
        }

        [HttpPost ("login")]
        public async Task<IActionResult> Login ([FromBody] SignInUser command) {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            var user = await _authService.LoginAsync (command.Email, command.Password);
            if (user == null)
                return Unauthorized ();
            var token = new TokenDTO {
                Token = await GenerateToken (user, _jwtSettings)
            };
            return Ok (token);
        }
        private async Task<string> GenerateToken (User user, IJWTSettings jwtSettings) {
            var tokenHandler = new JwtSecurityTokenHandler ();
            var key = Encoding.ASCII.GetBytes (jwtSettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity (new Claim[] {
                new Claim (ClaimTypes.NameIdentifier, user.Id.ToString ()),
                new Claim (ClaimTypes.Name, user.Email),
                new Claim (ClaimTypes.Role, user.Role)
                }),
                Issuer = "",
                Expires = DateTime.Now.AddDays (jwtSettings.ExpiryDays),
                SigningCredentials = new SigningCredentials (new SymmetricSecurityKey (key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken (tokenDescriptor);
            return await Task.FromResult (tokenHandler.WriteToken (token));
        }

        [HttpPost ("register")]
        public async Task<IActionResult> Register ([FromBody] CreateUser command) {
            command.Email = command.Email.ToLower ();
            if (await _userService.UserExistByEmailAsync (command.Email))
                ModelState.AddModelError ("Email", "Email is already taken.");
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            try {
                await _authService.RegisterAsync (command.Email, command.Password, command.Name, command.Surname, command.Country);
                return StatusCode (201);
            } catch (Exception e) {
                return BadRequest (e.Message);
            }
        }

        [HttpPost ("activation")]
        public async Task<IActionResult> Activation ([FromBody] Activate command) {
            if (!ModelState.IsValid) {
                return BadRequest (new { errorMessage = "Value of activation key is invalid" });
            }
            try {
                await _userService.ActivateAsync (command.ActivationKey);
                return Ok (new { message = "Account was activated" });
            } catch (Exception e) {
                return NotFound (new { message = e.Message });
            }
        }

        [Authorize]
        [HttpPost ("changePassword")]
        public async Task<IActionResult> ChangePassword ([FromBody] ChangePassword command) {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            var user = await _authService.LoginAsync (UserEmail, command.OldPassword);
            if (user == null)
                return Unauthorized ();
            try {
                await _userService.UpdatePasswordAsync (user, command.NewPassword);
                return Ok (new { message = "Password was changed" });
            } catch (Exception e) {
                return BadRequest (new { errorMessage = e.Message });
            }
        }

        [HttpPost ("restorePassword")]
        public async Task<IActionResult> RestorePassword ([FromBody] RestorePassword command) {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            var user = await _userService.GetActiveByEmailAsync (command.Email);
            if (user == null)
                return Ok (new { message = "The message of password restoring has been sent to given email address" });
            try {
                await _userService.RestorePasswordAsync (user);
                return Ok (new { message = "The message of password restoring has been sent to given email address" });
            } catch (Exception e) {
                return BadRequest (new { errorMessage = e.Message });
            }
        }

        [HttpPut ("restorePassword")]
        public async Task<IActionResult> ChangePasswordByRestoringPassword ([FromBody] ChangePasswordByRestoringPassword command) {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            var user = await _userService.GetActiveByEmailAsync (command.Email);
            if (user == null)
                return Unauthorized ();
            try {
                await _userService.ChangePasswordByRestoringPassword (user.Email, command.Token, command.NewPassword);
                return Ok (new { message = "The password was changed" });
            } catch (Exception e) {
                return BadRequest (new { errorMessage = e.Message });
            }
        }

        private async Task<IActionResult> Upload ([FromForm] CreateSendedEmail command) {
            List<FileModel> fileModels = new List<FileModel> ();
            foreach (var file in command.Attachments) {
                if (file.Length > 0) {
                    using (var ms = new MemoryStream ()) {
                        var fileModel = new FileModel ();
                        file.CopyTo (ms);
                        fileModel.Store = ms.ToArray ();
                        fileModel.ContentType = file.ContentType;
                        fileModel.FileName = file.FileName;
                        fileModels.Add (fileModel);
                    }
                }
            }
            await Task.CompletedTask;
            var returnFile = fileModels.ElementAt (1);
            // var msa = await Zip.GetZipStream (fileModels);
            // return File (msa.ToArray (), "application/zip", "Attachments.zip");
            return Ok ();
        }

    }
}