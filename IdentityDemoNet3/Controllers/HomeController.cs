using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IdentityDemoNet3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using IdentityDemoNet3.ViewModels;

namespace IdentityDemoNet3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> signInManager;

        public HomeController(ILogger<HomeController> logger, UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult AcercaDe()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Registro()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(RegistroViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Descripcion);

                if (user == null)
                {

                    user = new Usuario
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = model.Descripcion
                    };

                    var result = await _userManager.CreateAsync(user, model.Contraseña);

                    if (result.Succeeded)
                    {
                        return View("Exito");
                    }
                    else
                    {

                        var errorVM = new ErrorViewModel
                        {
                            Errores = result.Errors,
                            RequestId = "1"
                        };
                        return View("Error", errorVM);

                    }



                }


            }
            return View();
        }

        [HttpGet]
        public IActionResult IniciarSesion()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> IniciarSesion(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {


                var signInResult = await signInManager.PasswordSignInAsync(model.Descripcion, model.Contraseña, false, false);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                //var user = await _userManager.FindByNameAsync(model.Descripcion);
                //if (user != null && await _userManager.CheckPasswordAsync(user,model.Contraseña)) {

                //    var identity = new ClaimsIdentity("cookies"); //This is the authority that issued this identity
                //    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier,user.Id));
                //    identity.AddClaim(new Claim(ClaimTypes.Name,user.UserName));

                //    //Create a ClaimsPrincipal that represents the user
                //    await HttpContext.SignInAsync("cookies",new ClaimsPrincipal(identity));
                //  return   RedirectToAction("Index");

                //}
            }
            ModelState.AddModelError("", "Usuario o contraseña invalidos");

            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM vm)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(vm.Correo);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetUrl = Url.Action("ResetPassword", "Home",
                        new { token = token, correo = user.Email }, Request.Scheme);
                    System.IO.File.WriteAllText("resetLink.txt", resetUrl);
                }
                else
                {
                    //email user and inform them that they do not have an account
                }
                return View("Exito");
            }
            return View();

        }


        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            return View(new ResetPasswordVM { Token = token, Correo = email });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM vm)
        {

            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByEmailAsync(vm.Correo);
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, vm.Token, vm.Contraseña);
                    if (!result.Succeeded) {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("",error.Description);
                        }
                        return View();
                    }
                    return View("Exito");
                }
                ModelState.AddModelError("", "Solicitud Invalida");
            }
            return View();
        }

    }
}
