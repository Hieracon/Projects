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
    public class BadgesController : Controller
    {
        //private readonly NotRedditContext _context;
        private readonly BadgeService badgeService;

        public BadgesController(BadgeService badgeService)
        {
            this.badgeService = badgeService;
        }

        [Authorize(Roles = "Admin,User")]
        // GET: Badges
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Badges.ToListAsync());
            var badge = badgeService.GetAllBadges();
            return View(badge);
        }

        [Authorize(Roles = "Admin")]
        // GET: Badges/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var badge = await _context.Badges
            //    .FirstOrDefaultAsync(m => m.BadgeId == id);
            var badge = badgeService.GetDetailsById(id);

            if (badge == null)
            {
                return NotFound();
            }

            return View(badge);
        }

        [Authorize(Roles = "Admin")]
        // GET: Badges/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Badges/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BadgeId,BadgeName,RequiredNrLikes")] Badge badge)
        {
            if (ModelState.IsValid)
            {
                badgeService.Create(badge);
                return RedirectToAction(nameof(Index));
            }
            return View(badge);
        }

        [Authorize(Roles = "Admin")]
        // GET: Badges/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var badge = await _context.Badges.FindAsync(id);
            var badge = badgeService.GetDetailsById(id);

            if (badge == null)
            {
                return NotFound();
            }
            return View(badge);
        }

        // POST: Badges/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BadgeId,BadgeName,RequiredNrLikes")] Badge badge)
        {
            if (id != badge.BadgeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    badgeService.UpdateBadge(badge);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!badgeService.BadgeExists(badge.BadgeId))
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
            return View(badge);
        }

        // GET: Badges/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var badge = await _context.Badges
            //    .FirstOrDefaultAsync(m => m.BadgeId == id);
            var badge = badgeService.GetDetailsById(id);

            if (badge == null)
            {
                return NotFound();
            }

            return View(badge);
        }

        // POST: Badges/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            badgeService.DeleteBadge(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
