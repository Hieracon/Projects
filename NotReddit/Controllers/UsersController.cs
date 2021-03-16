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
    public class UsersController : Controller
    {
        private readonly UserService userService;
        private readonly UserManager<User> userManager;
        private readonly NotRedditContext context;

        public UsersController(UserService userService, UserManager<User> userManager, NotRedditContext context)
        {
            this.userManager = userManager;
            this.context = context;
            this.userService = userService;
        }

        [Authorize(Roles = "Admin")]
        // GET: Users
        public async Task<IActionResult> Index()
        {
            var user = userService.GetAllUsers();
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        // GET: Users/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = userService.GetDetailsById(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [Authorize(Roles = "Admin")]
        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["BadgeId"] = new SelectList(userService.GetAllBadges(), "BadgeId", "BadgeId");
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserName,Password,MailAddress,BadgeId")] User user)
        {
            if (ModelState.IsValid)
            {
                userService.Create(user);
                return RedirectToAction(nameof(Index));
            }
            ViewData["BadgeId"] = new SelectList(userService.GetAllBadges(), "BadgeId", "BadgeId");
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = userService.GetDetailsById(id);

            if (user == null)
            {
                return NotFound();
            }
            ViewData["BadgeId"] = new SelectList(userService.GetAllBadges(), "BadgeId", "BadgeId");
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserId,UserName,Password,MailAddress,BadgeId")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    userService.UpdateUser(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!userService.UserExists(user.Id))
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
            ViewData["BadgeId"] = new SelectList(userService.GetAllBadges(), "BadgeId", "BadgeId");
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = userService.GetDetailsById(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [Authorize(Roles = "Admin")]
        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            userService.DeleteUser(id);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,User")]
        public async Task<ViewResult> UserBadge()
        {
            DataTable table = new DataTable();

            table.Columns.Add("Badge");
            table.Columns.Add("Required Likes");

            var userId = userManager.GetUserId(User);
            var user = userService.GetDetailsById(userId);

            var badges = (from badge in context.Badges
                          where badge.BadgeId == user.BadgeId
                          select new
                          {
                              badge.BadgeName,
                              badge.RequiredNrLikes
                          }).ToList();

            foreach (var userBadge in badges)
            {
                table.Rows.Add(userBadge.BadgeName, userBadge.RequiredNrLikes);
            }

            ViewData["table"] = table;

            return View();
        }
    }
}
