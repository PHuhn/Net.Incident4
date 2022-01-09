using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatR;
//
using NSG.NetIncident4.Core.Infrastructure.Common;
using NSG.NetIncident4.Core.Infrastructure.Notification;
using NSG.NetIncident4.Core.UI.ViewModels;
//
namespace NSG.NetIncident4.Core.UI.Controllers
{
    public class HomeController : BaseController
    {
        private INotificationService _notificationService;
        ILogger<HomeController> _logger;
        IApplication _application;
        //
        /// <summary>
        /// Base constructors, so initialize Alerts list of alert-messages.
        /// </summary>
        /// <param name="notificationService"></param>
        /// <param name="application"></param>
        /// <param name="logger"></param>
        public HomeController(INotificationService notificationService, 
            IApplication application, ILogger<HomeController> logger, IMediator mediator) : base(mediator)
        {
            _notificationService = notificationService;
            _application = application;
            _logger = logger;
        }
        /*
        ** home or home/index
        */
        public IActionResult Index()
        {
            _logger.Log(LogLevel.Information, "In home page");
            return View();
        }
        /*
        ** home/Privacy
        ** display the privacy contract
        */
        public IActionResult Privacy()
        {
            ViewBag.ApplicationName = _application.GetApplicationName();
            ViewBag.ApplicationPhone = _application.GetApplicationPhoneNumber();
            return View();
        }
        /*
        ** home/Bootstrap
        ** display bootstrap feature
        */
        public IActionResult Bootstrap()
        {
            return View();
        }
        /*
        ** home/TestAlerts
        */
        public IActionResult TestAlerts()
        {
            var _about = new AboutViewModel();
            ViewBag.Version = _about.Version;
            ViewBag.Process =
                System.Diagnostics.Process.GetCurrentProcess();
            //
            // display sample messages
            //
            Error("Error message...");
            Warning("Warning message...");
            Success("Success message...");
            Information("Information, About Page...");
            return View();
        }
        /*
        ** home/About
        */
        public ActionResult About()
        {
            var _about = new AboutViewModel();
            //
            return View(_about);
        }
        /*
        ** home/Contact
        */
        public ActionResult Contact()
        {
            ViewBag.Title = "Contact Us";
            return View();
        }
        /*
        ** home/help
        */
        public ActionResult Help()
        {
            ViewBag.Title = "Home Page";
            return View();
        }
        /*
        ** home/TestUser
        */
        public ActionResult TestUser()
        {
            TestUserViewModel _model = new TestUserViewModel();
            if (User.Identity.IsAuthenticated)
            {
                _model = new TestUserViewModel()
                {
                    UserHttpContext = _application.GetUserAccount(),
                    UserClaimsPrincipal = Base_GetUserAccount()
                };
            }
            return View(_model);
        }
        /*
        ** home/Error
        ** System error handler
        */
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //
    }
}
