using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotReddit.Data;
using NotReddit.Models;
using NotReddit.Services.ImplementationServices;
using NotReddit.Services.Interfaces;

namespace NotReddit.Controllers
{
    public class LikesController : Controller
    {
        //private readonly NotRedditContext _context;
        private readonly LikeService likeService;

        public LikesController(LikeService likeService)
        {
            this.likeService = likeService;
        }

        [Authorize(Roles = "Admin")]
        // GET: Likes
        public async Task<IActionResult> Index()
        {
            var like = likeService.GetAllLikes();
            return View(like);
        }

        [Authorize(Roles = "Admin")]
        // GET: Likes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var like = likeService.GetDetailsById(id);

            if (like == null)
            {
                return NotFound();
            }

            return View(like);
        }

        [Authorize(Roles = "Admin")]
        // GET: Likes/Create
        public IActionResult Create()
        {
            ViewData["CommentId"] = new SelectList(likeService.GetAllComments(), "CommentId", "CommentId");
            ViewData["PostId"] = new SelectList(likeService.GetAllPosts(), "PostId", "PostId");
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: Likes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LikeId,NrLikes,PostId,CommentId")] Like like)
        {
            if (ModelState.IsValid)
            {
                likeService.Create(like);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CommentId"] = new SelectList(likeService.GetAllComments(), "CommentId", "CommentId");
            ViewData["PostId"] = new SelectList(likeService.GetAllPosts(), "PostId", "PostId");
            return View(like);
        }

        [Authorize(Roles = "Admin")]
        // GET: Likes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var like = likeService.GetDetailsById(id);

            if (like == null)
            {
                return NotFound();
            }
            ViewData["CommentId"] = new SelectList(likeService.GetAllComments(), "CommentId", "CommentId");
            ViewData["PostId"] = new SelectList(likeService.GetAllPosts(), "PostId", "PostId");
            return View(like);
        }

        [Authorize(Roles = "Admin")]
        // POST: Likes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LikeId,NrLikes,PostId,CommentId")] Like like)
        {
            if (id != like.LikeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    likeService.UpdateLike(like);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!likeService.LikeExists(like.LikeId))
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
            ViewData["CommentId"] = new SelectList(likeService.GetAllComments(), "CommentId", "CommentId");
            ViewData["PostId"] = new SelectList(likeService.GetAllPosts(), "PostId", "PostId");
            return View(like);
        }

        [Authorize(Roles = "Admin")]
        // GET: Likes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var like = likeService.GetDetailsById(id);

            if (like == null)
            {
                return NotFound();
            }

            return View(like);
        }

        [Authorize(Roles = "Admin")]
        // POST: Likes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            likeService.DeleteLike(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
