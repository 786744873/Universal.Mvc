using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Universal.Framework.Controllers.Admin;
using Universal.Framework.Security.Admin;

namespace Universal.Mvc.Areas.Admin.Controllers
{
    [Route("admin/main")]
    public class MainController : PublicAdminController
    {
        private IAdminAuthService _adminAuthService;
        private readonly ILogger<MainController> _logger;
        public MainController(IAdminAuthService adminAuthService, ILogger<MainController> logger)
        {
            _adminAuthService = adminAuthService;
            _logger = logger;
        }

        [Route("index",Name = "mainindex")]
        public IActionResult Index()
        {
            _logger.LogError("*****************************");
            return View();
        }

        [Route("signout")]
        public IActionResult SignOut()
        {
            _adminAuthService.SignOut();
            return RedirectToRoute("adminLogin");
        }

        [Route("Error404")]
        public IActionResult Error404()
        {
            return View("~/Areas/Admin/Views/Shard/Error.cshtml");
        }
    }
}