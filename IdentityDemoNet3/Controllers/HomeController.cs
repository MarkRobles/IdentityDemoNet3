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

namespace IdentityDemoNet3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
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
        public IActionResult AcercaDe() {
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

                    user = new IdentityUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = model.Descripcion
                    };

                    var result = await _userManager.CreateAsync(user, model.Contraseña);

                    if (result.Succeeded)
                    {
                        return View("Exito");
                    }
                    else {
                       
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
        public IActionResult IniciarSesion() {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> IniciarSesion(LoginViewModel model)
        {
            if (ModelState.IsValid) {

                var user = await _userManager.FindByNameAsync(model.Descripcion);
                if (user != null && await _userManager.CheckPasswordAsync(user,model.Contraseña)) {
                  
                    var identity = new ClaimsIdentity("cookies"); //This is the authority that issued this identity
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier,user.Id));
                    identity.AddClaim(new Claim(ClaimTypes.Name,user.UserName));

                    //Create a ClaimsPrincipal that represents the user
                    await HttpContext.SignInAsync("cookies",new ClaimsPrincipal(identity));
                  return   RedirectToAction("Index");

                }
            }
            ModelState.AddModelError("","Usuario o contraseña invalidos");

            return View();
        }

    }
}
