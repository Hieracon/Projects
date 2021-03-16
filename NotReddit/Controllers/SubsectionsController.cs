using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotReddit.Data;
using NotReddit.Models;
using NotReddit.Services.ImplementationServices;
using NotReddit.Services.Interfaces;

namespace NotReddit.Controllers
{
    public class SubsectionsController : Controller
    {
        private readonly SubsectionService subsectionService;
        private readonly PostService postService;
        private readonly NotRedditContext context;
        private readonly UserManager<User> userManager;

        public SubsectionsController(SubsectionService subsectionService, NotRedditContext context, PostService postService, UserManager<User> userManager)
        {
            this.subsectionService = subsectionService;
            this.postService = postService;
            this.context = context;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        // GET: Subsections
        public async Task<IActionResult> Index()
        {
            var subsection = subsectionService.GetAllSubsections();
            return View(subsection);
        }

        [Authorize(Roles = "Admin")]
        // GET: Subsections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subsection = subsectionService.GetDetailsById(id);

            if (subsection == null)
            {
                return NotFound();
            }

            return View(subsection);
        }

        [Authorize(Roles = "Admin,User")]
        // GET: Subsections/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin,User")]
        // POST: Subsections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubsectionId,SubsectionName")] Subsection subsection)
        {
            if (ModelState.IsValid)
            {
                subsectionService.Create(subsection);
                return RedirectToAction(nameof(Index));
            }
            return View(subsection);
        }

        [Authorize(Roles = "Admin")]
        // GET: Subsections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subsection = subsectionService.GetDetailsById(id);

            if (subsection == null)
            {
                return NotFound();
            }
            return View(subsection);
        }

        [Authorize(Roles = "Admin")]
        // POST: Subsections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubsectionId,SubsectionName")] Subsection subsection)
        {
            if (id != subsection.SubsectionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    subsectionService.UpdateSubsection(subsection);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!subsectionService.SubsectionExists(subsection.SubsectionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(subsection);
        }

        [Authorize(Roles = "Admin")]
        // GET: Subsections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subsection = subsectionService.GetDetailsById(id);

            if (subsection == null)
            {
                return NotFound();
            }

            return View(subsection);
        }

        [Authorize(Roles = "Admin")]
        // POST: Subsections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            subsectionService.DeleteSubsection(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ViewResult> SubsectionContent(string name)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Subsection Id");
            table.Columns.Add("Subsection Name");
            table.Columns.Add("Post Id");
            table.Columns.Add("Post Title");
            table.Columns.Add("User");

            var content = (from subsection in context.Subsections
                           from post in context.Posts
                           from user in context.Users
                           where subsection.SubsectionName == name &&
                                 subsection.SubsectionId == post.SubsectionId &&
                                 user.Id == post.UserId
                           select new
                           {
                               subsection.SubsectionId,
                               subsection.SubsectionName,
                               post.PostId,
                               post.Title,
                               user.UserName
                           }).ToList();

            foreach (var subContent in content)
            {
                table.Rows.Add(subContent.SubsectionId, subContent.SubsectionName, subContent.PostId, subContent.Title, subContent.UserName);
            }

            ViewData["table"] = table;

            return View();
        }

        [Authorize(Roles = "Admin,User")]
        public IActionResult CreatePost(int subsectionId, string subsectionName)
        {
            ViewData["SubsectionId"] = new SelectList(postService.GetAllSubsections(), "SubsectionId", "SubsectionName");
            ViewData["UserId"] = new SelectList(postService.GetAllUsers(), "Id", "UserName");

            ViewData["subsectionId"] = subsectionId.ToString();
            ViewData["subsectionName"] = subsectionName;

            return View();
        }

        [Authorize(Roles = "Admin,User")]
        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(int subsectionId, string subsectionName, [Bind("PostId,Title,PostContent,PostLink,PostLike,UserId,SubsectionId")] Post post)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Subsection Id");
            table.Columns.Add("Subsection Name");
            table.Columns.Add("Post Id");
            table.Columns.Add("Post Title");
            table.Columns.Add("User");

            var content = (from subsection in context.Subsections
                           from posts in context.Posts
                           from user in context.Users
                           where subsection.SubsectionName == subsectionName &&
                                 subsection.SubsectionId == posts.SubsectionId &&
                                 user.Id == posts.UserId
                           select new
                           {
                               subsection.SubsectionId,
                               subsection.SubsectionName,
                               posts.PostId,
                               posts.Title,
                               user.UserName
                           }).ToList();

            foreach (var subContent in content)
            {
                table.Rows.Add(subContent.SubsectionId, subContent.SubsectionName, subContent.PostId, subContent.Title, subContent.UserName);
            }

            ViewData["table"] = table;

            var userId = userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                post.UserId = userId;
                post.SubsectionId = subsectionId;
                post.PostLike = 0;
                postService.Create(post);
                return RedirectToAction("SubsectionContent", new { name = subsectionName });
            }

            ViewData["SubsectionId"] = new SelectList(postService.GetAllSubsections(), "SubsectionId", "SubsectionName");
            ViewData["UserId"] = new SelectList(postService.GetAllUsers(), "Id", "UserName");
            return View(post);
        }
    }
}
