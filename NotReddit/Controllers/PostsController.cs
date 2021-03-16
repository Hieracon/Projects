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
    public class PostsController : Controller
    {
        private readonly PostService postService;
        private readonly CommentService commentService;
        private readonly NotRedditContext context;
        private readonly UserManager<User> userManager;

        public PostsController(PostService postService, NotRedditContext context,
                               CommentService commentService, UserManager<User> userManager)
        {
            this.postService = postService;
            this.context = context;
            this.commentService = commentService;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var post = postService.GetAllPosts();
            return View(post);
        }

        [Authorize(Roles = "Admin")]
        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = postService.GetDetailsById(id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [Authorize(Roles = "Admin")]
        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["SubsectionId"] = new SelectList(postService.GetAllSubsections(), "SubsectionId", "SubsectionName");
            ViewData["UserId"] = new SelectList(postService.GetAllUsers(), "Id", "UserName");
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Title,PostContent,PostLink,PostLike,UserId,SubsectionId")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.PostLike = 0;
                postService.Create(post);
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubsectionId"] = new SelectList(postService.GetAllSubsections(), "SubsectionId", "SubsectionName");
            ViewData["UserId"] = new SelectList(postService.GetAllUsers(), "Id", "UserName");
            return View(post);
        }

        [Authorize(Roles = "Admin")]
        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = postService.GetDetailsById(id);

            if (post == null)
            {
                return NotFound();
            }
            ViewData["SubsectionId"] = new SelectList(postService.GetAllSubsections(), "SubsectionId", "SubsectionName");
            ViewData["UserId"] = new SelectList(postService.GetAllUsers(), "Id", "UserName");
            return View(post);
        }

        [Authorize(Roles = "Admin")]
        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,PostContent,PostLink,PostLike,UserId,SubsectionId")] Post post)
        {
            if (id != post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    postService.UpdatePost(post);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!postService.PostExists(post.PostId))
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
            ViewData["SubsectionId"] = new SelectList(postService.GetAllSubsections(), "SubsectionId", "SubsectionName");
            ViewData["UserId"] = new SelectList(postService.GetAllUsers(), "Id", "UserName");
            return View(post);
        }

        [Authorize(Roles = "Admin")]
        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = postService.GetDetailsById(id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [Authorize(Roles = "Admin")]
        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            postService.DeletePost(id);
            return RedirectToAction(nameof(Index));
        }

        public ActionResult PostComments(string author, int postId)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Post Id");
            table.Columns.Add("Post Title");
            table.Columns.Add("Author");
            table.Columns.Add("Post Content");
            table.Columns.Add("User");
            table.Columns.Add("Comment");
            table.Columns.Add("Comment Likes");
            table.Columns.Add("Comment Id");

            var content = (from post in context.Posts
                           from comment in context.Comments
                           from user in context.Users
                           where post.PostId == postId &&
                                 post.PostId == comment.PostId &&
                                 user.Id == comment.UserId
                           select new
                           {
                               post.Title,
                               post.PostContent,
                               user.UserName,
                               comment.CommentContent,
                               comment.CommentId
                           }).ToList();

            foreach (var postComments in content)
            {
                var comment = commentService.GetDetailsById(postComments.CommentId);
                table.Rows.Add(postId, postComments.Title, author, postComments.PostContent,
                                postComments.UserName,postComments.CommentContent, comment.CommentLike, postComments.CommentId);
            }

            if (table.Rows.Count <= 0)
            {
                return RedirectToAction("CreatePostComment", new { postId = postId });
            }

            var postLikes = postService.GetDetailsById(postId);

            ViewData["table"] = table;
            ViewData["likes"] = postLikes.PostLike.ToString();

            return View();
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult PostLike(string author, int postId)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Post Id");
            table.Columns.Add("Post Title");
            table.Columns.Add("Author");
            table.Columns.Add("Post Content");
            table.Columns.Add("User");
            table.Columns.Add("Comment");
            table.Columns.Add("Comment Likes");
            table.Columns.Add("Comment Id");

            var content = (from post in context.Posts
                           from comment in context.Comments
                           from user in context.Users
                           where post.PostId == postId &&
                                 post.PostId == comment.PostId &&
                                 user.Id == comment.UserId
                           select new
                           {
                               post.Title,
                               post.PostContent,
                               user.UserName,
                               comment.CommentContent,
                               comment.CommentId
                           }).ToList();

            var likedPost = postService.GetDetailsById(postId);

            likedPost.PostLike++;

            postService.UpdatePost(likedPost);

            foreach (var postComments in content)
            {
                var comment = commentService.GetDetailsById(postComments.CommentId);
                table.Rows.Add(postId, postComments.Title, author, postComments.PostContent,
                                postComments.UserName, postComments.CommentContent, comment.CommentLike, postComments.CommentId);
            }

            ViewData["table"] = table;
            ViewData["likes"] = likedPost.PostLike.ToString();

            return View("PostComments");
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult CommentLike(string author, int postId, int commentId)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Post Id");
            table.Columns.Add("Post Title");
            table.Columns.Add("Author");
            table.Columns.Add("Post Content");
            table.Columns.Add("User");
            table.Columns.Add("Comment");
            table.Columns.Add("Comment Likes");
            table.Columns.Add("Comment Id");

            var content = (from post in context.Posts
                           from comment in context.Comments
                           from user in context.Users
                           where post.PostId == postId &&
                                 post.PostId == comment.PostId &&
                                 user.Id == comment.UserId
                           select new
                           {
                               post.Title,
                               post.PostContent,
                               user.UserName,
                               comment.CommentContent,
                               comment.CommentId
                           }).ToList();

            foreach (var postComments in content)
            {
                var comment = commentService.GetDetailsById(postComments.CommentId);
                if (comment.CommentId == commentId)
                {
                    comment.CommentLike++;
                    commentService.UpdateComment(comment);
                }

                table.Rows.Add(postId, postComments.Title, author, postComments.PostContent,
                                postComments.UserName, postComments.CommentContent, comment.CommentLike, postComments.CommentId);
            }

            var postLikes = postService.GetDetailsById(postId);

            ViewData["table"] = table;
            ViewData["likes"] = postLikes.PostLike.ToString();

            return View("PostComments");
        }

        [Authorize(Roles = "Admin,User")]
        public IActionResult CreatePostComment(int postId)
        {
            ViewData["PostId"] = new SelectList(commentService.GetAllPosts(), "PostId", "PostId");
            ViewData["UserId"] = new SelectList(commentService.GetAllUsers(), "Id", "UserName");

            ViewData["postId"] = postId.ToString();

            return View();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePostComment(int postId, [Bind("CommentId,CommentContent,PostId,UserId")] Comment comment)
        {
            var userId = userManager.GetUserId(User);

            var postAuthor = (from post in context.Posts
                              from user in context.Users
                              where post.PostId == postId &&
                                    post.UserId == user.Id
                              select user.UserName).ToList();

            comment.UserId = userId;
            comment.PostId = postId;

            if (ModelState.IsValid)
            {
                commentService.Create(comment);
                return RedirectToAction("PostComments", new {author = postAuthor.ElementAt(0), postId = postId });
            }

            ViewData["PostId"] = new SelectList(commentService.GetAllPosts(), "PostId", "PostId");
            ViewData["UserId"] = new SelectList(commentService.GetAllUsers(), "Id", "UserName");
            return View(comment);
            //return View("PostComments");
        }
    }
}
