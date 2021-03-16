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
    public class CommentsController : Controller
    {
        //private readonly NotRedditContext _context;
        private readonly CommentService commentService;

        public CommentsController(CommentService commentService)
        {
            this.commentService = commentService;
        }

        [Authorize(Roles = "Admin")]
        // GET: Comments
        public async Task<IActionResult> Index()
        {
            //var notRedditContext = _context.Comments.Include(c => c.Admin).Include(c => c.Post).Include(c => c.User);
            //return View(await notRedditContext.ToListAsync());
            var comment = commentService.GetAllComments();
            return View(comment);
        }

        [Authorize(Roles = "Admin")]
        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = commentService.GetDetailsById(id);

            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        [Authorize(Roles = "Admin")]
        // GET: Comments/Create
        public IActionResult Create()
        {
            ViewData["PostId"] = new SelectList(commentService.GetAllPosts(), "PostId", "PostId");
            ViewData["UserId"] = new SelectList(commentService.GetAllUsers(), "Id", "UserName");
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId,CommentContent,CommentLike,PostId,UserId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.CommentLike = 0;
                commentService.Create(comment);
                return RedirectToAction(nameof(Index));
            }
            ViewData["PostId"] = new SelectList(commentService.GetAllPosts(), "PostId", "PostId");
            ViewData["UserId"] = new SelectList(commentService.GetAllUsers(), "Id", "UserName");
            return View(comment);
        }

        [Authorize(Roles = "Admin")]
        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var comment = await _context.Comments.FindAsync(id);
            var comment = commentService.GetDetailsById(id);

            if (comment == null)
            {
                return NotFound();
            }
            ViewData["PostId"] = new SelectList(commentService.GetAllPosts(), "PostId", "PostId");
            ViewData["UserId"] = new SelectList(commentService.GetAllUsers(), "Id", "UserName");
            return View(comment);
        }

        [Authorize(Roles = "Admin")]
        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommentId,CommentContent,CommentLike,PostId,UserId")] Comment comment)
        {
            if (id != comment.CommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    commentService.UpdateComment(comment);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!commentService.CommentExists(comment.CommentId))
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
            ViewData["PostId"] = new SelectList(commentService.GetAllPosts(), "PostId", "PostId");
            ViewData["UserId"] = new SelectList(commentService.GetAllUsers(), "Id", "UserName");
            return View(comment);
        }

        [Authorize(Roles = "Admin")]
        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = commentService.GetDetailsById(id);

            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        [Authorize(Roles = "Admin")]
        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            commentService.DeleteComment(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
