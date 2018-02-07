using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using SalesFirst.Core.Data;
using SalesFirst.Core.Service;
using SaphirConges.Models;
using SaphirCongesCore.Data;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace SaphirConges.Controllers
{
    public class CompteController : Controller
    {
        private SaphirCongesDB db = new SaphirCongesDB();
        readonly EmployeeRepository employeRepo;
        readonly EmployeeService employeService;

        public CompteController() :
            this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
            employeRepo = new EmployeeRepository(db);
            employeService = new EmployeeService(employeRepo);
        }

        public CompteController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
            // Pour pouvoir avoir des caracteres alphanumerique
            UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager)
            {
                AllowOnlyAlphanumericUserNames = false,
            };
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        //GET: /Compte/Login
        [AllowAnonymous] 
        public ActionResult Login(string returnUrl)
        {
           ViewBag.Employe = new SelectList(employeService.GetAll(), "Username", "Username");
            return View();
        }

        //
        //POST :/Compte/Login
        [HttpPost]
        [AllowAnonymous] 
        [ValidateAntiForgeryToken] //Mesure de sécurité de base dans les applications MVC
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            ViewBag.Employe = new SelectList(employeService.GetAll(), "Username", "Username");
           
            
                var user = await UserManager.FindAsync(model.employeeUsername.Username, model.password);
                if (user != null)
                {
                    await SignInAsync(user, model.rememberMe);
                    return RedirectToLocal(returnUrl);
                }
            
            if(model.employeeUsername.Username == null)
                {
                    ModelState.AddModelError("", "Mot de passe ou nom d'utilisateur invalide.");
                }
            //Actualiser le formulaire car on a des erreurs	
            return View(model);
        }

        //
        //GET: /Compte/Register
        [AllowAnonymous] // Peut être a effacer
        public ActionResult Register()
        {
            ViewBag.Username = new SelectList(employeService.GetAll(), "Username", "Username");
            return View();
        }

        //
        //POST: /Compte/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            ViewBag.Username = new SelectList(employeService.GetAll(), "Username", "Username");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.userName };
                var res = await UserManager.CreateAsync(user, model.password);
                if (res.Succeeded)
                {
                    await SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(res);
                }
            }
            return View(model);
        }

        //
        //POST: /Compte/Dissocier
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Dissociate(string loginPro, string proKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginPro, proKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Gerer", new { Message = message });
        }

        //
        //GET: /Compte/Gerer
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                    message == ManageMessageId.ChangePasswordSuccess ? "Votre mot de passe a bien été changé." :
                    message == ManageMessageId.SetPasswordSuccess ? "Votre mot de passe a été correctement défini." :
                    message == ManageMessageId.Error ? "Une erreur est survenue." :
                    "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();

        }

        //POST: /Compte/Gerer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPass = HasPassword();
            ViewBag.HasLocalPassword = hasPass;
            ViewBag.ReturnUrl = Url.Action("Gerer");
            if (hasPass)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult res = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.oldPassword, model.newPassword);
                    if (res.Succeeded)
                    {
                        return RedirectToAction("Gerer", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(res);
                    }
                }
            }
            else //Quand l'utilisateur n'a pas de mot de passe
            {
                ModelState state = ModelState["oldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }
                if (ModelState.IsValid)
                {
                    IdentityResult res = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.newPassword);
                    if (res.Succeeded)
                    {
                        return RedirectToAction("Gerer", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(res);
                    }
                }
            }
            return View(model);
        }

        //POST: /Compte/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Utils
        private const string XsrfKey = "MyXsrfID";

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult res)
        {
            foreach (var error in res.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserID { get; set; }

            public ChallengeResult(string pro, string redirectUri) : this(pro, redirectUri, null)
            { }

            public ChallengeResult(string pro, string redirectUri, string userID)
            {
                LoginProvider = pro;
                RedirectUri = redirectUri;
                UserID = userID;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                var prop = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserID != null)
                {
                    prop.Dictionary[XsrfKey] = UserID;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(prop, LoginProvider);
            }
        }
        #endregion
    }

}