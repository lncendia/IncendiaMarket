using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Data.Interfaces;
using WebApplication.Data.Repository;
using WebApplication.ViewModels;

namespace WebApplication.Data.Controllers
{
    public class AccountController:Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly EmailService _emailService;
        private readonly IMessage _messageManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, EmailService emailService, IMessage messageManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _messageManager = messageManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            IdentityUser user = new IdentityUser {UserName = model.Username, Email = model.Email};
            if (!ModelState.IsValid) return View(model);
            var result = await _userManager.CreateAsync(user,model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("ConfirmEmail");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ConfirmEmail()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user.EmailConfirmed)
            {
                return BadRequest();
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action(
                "AcceptCode",
                "Account",
                new {code},
                protocol: HttpContext.Request.Scheme);
            _emailService.SendMessage(user.UserName, user.Email,
                $"Подтвердите регистрацию, перейдя по <a href=\"{callbackUrl}\">ссылке</a>.");
            ViewBag.Message = "Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме.";
            return View("PrintText");
        }

        [HttpGet]
        [AllowAnonymous]
        [Authorize]
        public async Task<IActionResult> AcceptCode(string code)
        {
            IdentityUser user = await _userManager.GetUserAsync(User);
            if (code == null)
            {
                return Unauthorized();
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if(result.Succeeded)return RedirectToAction("Index", "Home");
            return BadRequest();
        }
        [HttpGet]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AcceptChangeEmail(string email, string code)
        {
            IdentityUser user = await _userManager.GetUserAsync(User);
            if (code == null || email == null)
            {
                return Unauthorized();
            }

            var result = await _userManager.ChangeEmailAsync(user, email, code);
            if(result.Succeeded)return RedirectToAction("Index", "Home");
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("",error.Description);
            }

            return View("ChangeEmail",new ChangeEmailViewModel(){Email = user.Email});
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var result =
                await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            return View(model);
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid) return View(model);
            var result = await _userManager.ChangePasswordAsync( user, model.OldPassword, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangeEmail()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(new ChangeEmailViewModel(){Email = user.Email});
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            IdentityUser user = await _userManager.GetUserAsync(User);
            var code = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
            var callbackUrl = Url.Action(
                "AcceptChangeEmail",
                "Account",
                new {email=model.Email, code},
                protocol: HttpContext.Request.Scheme);
            _emailService.SendMessage(user.UserName,model.Email,
                $"Подтвердите смену почты, перейдя по <a href=\"{callbackUrl}\">ссылке</a>.");
            ViewBag.Message = "Для завершения проверьте электронную почту и перейдите по ссылке, указанной в письме.";
            return View("PrintText");

        }
        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("","Пользователь не найден!");
                return View();
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action(
                "NewPassword",
                "Account",
                new {email=model.Email, code},
                protocol: HttpContext.Request.Scheme);
            _emailService.SendMessage(user.UserName,model.Email,
                $"Подтвердите смену пароля, перейдя по <a href=\"{callbackUrl}\">ссылке</a>.");
            ViewBag.Message = "Для восстановления пароля проверьте электронную почту и перейдите по ссылке, указанной в письме.";
            return View("PrintText");
        }
        [HttpGet]
        public IActionResult NewPassword(string email, string code)
        {
            if (email is null && code is null) return NotFound();
            return View(new EnterNewPasswordViewModel {Email = email, Code = code});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewPassword(EnterNewPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return NotFound();
            var reset = _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (reset.Result.Succeeded)
            {
                var callbackUrl = Url.Action(
                    "Login",
                    "Account");
                ViewBag.Message =
                    $"Пароль успешно изменен. Для входа в аккаунт перейдите <a href = \"{callbackUrl}\">по ссылке</a>.";
                return View("PrintText");
            }
            foreach (var error in reset.Result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            ViewBag.Message = "Ошибка";
            return View("PrintText");
        }
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            return View(await _userManager.GetUserAsync(User));
        }
        // [Authorize]
        // [HttpGet]
        // public async Task<IActionResult> GetDialogs()
        // {
        //     var user = await _userManager.GetUserAsync(User);
        //
        //     return Ok();
        // }
        // [HttpPost]
        // public async Task<IActionResult> GetDialogs(int page, string sort)
        // {
        //     return Ok();
        // }
        // public async Task<IActionResult> GetActiveAdvertisements()
        // {
        //     return Ok();
        // }
        // public async Task<IActionResult> GetArchiveAdvertisements()
        // {
        //     return Ok();
        // }
        // public async Task<IActionResult> GetFavoriteAdvertisements()
        // {
        //     return Ok();
        // }
        //
        //
        //
        // public async Task<IActionResult> SendMessage(int id, string text)
        // {
        //     return Ok();
        // }
        // [Authorize]
        // public async Task<IActionResult> DeleteMessage()
        // {
        //     return Ok();
        // }
    }
}