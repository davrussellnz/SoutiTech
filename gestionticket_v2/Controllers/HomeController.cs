using gestionticket_v2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using gestionticket_v2.Data;
using gestionticket_v2.Services;


namespace gestionticket_v2.Controllers
{
    public class HomeController : Controller
    {
/*
        private readonly ChatService _chatService;

        public HomeController(ChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] ChatService input)
        {
            var output = await _chatService.GetResponse();
            return Json(new { output = output });
        }
*/
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly gestionticket_v2Context _context;

        public HomeController(ILogger<HomeController> logger, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, gestionticket_v2Context context)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }


        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Contains("Client"))
                    {
                        return RedirectToAction("ClientTickets", "Tickets"); // replace with your action and controller names
                    }
                    else if (roles.Contains("MembreSupportTechnique"))
                    {
                        return RedirectToAction("TechnicianTickets", "Tickets"); // replace with your action and controller names
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Nom = model.Nom, Prenom = model.Prenom };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assign role to user and create corresponding entity
                    if (model.UserType == "Client")
                    {
                        await _userManager.AddToRoleAsync(user, Roles.Client);

                        var client = new Client { Id = user.Id, /* other properties */ };
                        _context.Client.Add(client);
                    }
                    else if (model.UserType == "MembreSupportTechnique")
                    {
                        await _userManager.AddToRoleAsync(user, Roles.MembreSupportTechnique);

                        var technician = new MembreSupportTechnique { Id = user.Id, /* other properties */ };
                        _context.MembreSupportTechnique.Add(technician);
                    }

                    await _context.SaveChangesAsync();

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }




        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var viewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }

    }
}