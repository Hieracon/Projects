using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GASF.Data;
using GASF.Models;
using GASF.Services.ImplementationServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace GASF.Controllers
{
    public class TeachersController : Controller
    {
        private readonly TeacherService teacherService;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public TeachersController(TeacherService teacherService,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.teacherService = teacherService;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Secretary")]
        // GET: Teachers
        public async Task<IActionResult> Index()
        {
            var teachers = teacherService.GetAllTeachers();
            return View(teachers);
        }

        [Authorize(Roles = "Secretary")]
        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = teacherService.GetDetailsById(id);

            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        [Authorize(Roles = "Secretary")]
        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeacherId,CNP,FirstName,LastName,MailAddress,Password,Address,Birthday")] Teacher teacher)
        {
            var user = new IdentityUser { UserName = teacher.MailAddress, Email = teacher.MailAddress };
            var result = await userManager.CreateAsync(user, teacher.Password);

            if (ModelState.IsValid)
            {
                await userManager.AddToRoleAsync(user, "Teacher");
                teacherService.Create(teacher);
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        [Authorize(Roles = "Secretary")]
        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = teacherService.GetDetailsById(id);

            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        [Authorize(Roles = "Secretary")]
        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TeacherId,CNP,FirstName,LastName,MailAddress,Password,Address,Birthday")] Teacher teacher)
        {
            if (id != teacher.TeacherId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    teacherService.UpdateTeacher(teacher);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!teacherService.TeacherExists(teacher.TeacherId))
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
            return View(teacher);
        }

        [Authorize(Roles = "Secretary")]
        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = teacherService.GetDetailsById(id);

            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        [Authorize(Roles = "Secretary")]
        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            teacherService.DeleteTeacher(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
