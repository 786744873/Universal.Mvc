using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Universal.Core;
using Universal.Entities;
using Universal.Mvc.Models;
using Universal.Services;

namespace Universal.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private EFDbContext _dbContext;
        private IRepository<SysUser> _repository;
        private ICategoryService _categoryService;
        public HomeController(EFDbContext dbContext, IRepository<SysUser> repository, ICategoryService categoryService)
        {
            _dbContext = dbContext;
            _repository = repository;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var list = _dbContext.SysUsers.ToList();
            var model = _repository.Table.ToList();
            var categoryList = _categoryService.GetAll();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
