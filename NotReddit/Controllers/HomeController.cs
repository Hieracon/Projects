using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotReddit.Data;
using NotReddit.Models;

namespace NotReddit.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserManager<User> userManager;
        private readonly NotRedditContext context;

        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, NotRedditContext context)
        {
            _logger = logger;
            this.userManager = userManager;
            this.context = context;
        }

        public IActionResult Index()
        {
            DataTable table = new DataTable();

            table.Columns.Add("Subsection Name");

            var content = (from subsection in context.Subsections
                           from post in context.Posts
                           where subsection.SubsectionId == post.SubsectionId
                           select new
                           {
                               subsection.SubsectionName
                           }).Distinct().ToList();

            foreach (var subsections in content)
            {
                table.Rows.Add(subsections.SubsectionName);
            }

            ViewData["table"] = table;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<FileContentResult> Imagine()
        {
            var user = await userManager.GetUserAsync(User);
            if (user.Imagine != null)
                return new FileContentResult(user.Imagine, "image/jpeg");
            else
                return new FileContentResult(new byte[0], "image/jpeg");
        }

    }
}
