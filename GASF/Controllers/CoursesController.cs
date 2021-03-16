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
using Microsoft.AspNetCore.Authorization;

namespace GASF.Controllers
{
    public class CoursesController : Controller
    {
        private readonly CourseService courseService;
        
        public CoursesController(CourseService courseService)
        {
            this.courseService = courseService;
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var course = courseService.GetAllCourses();
            return View(course);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = courseService.GetDetailsById(id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["TeacherId"] = new SelectList(courseService.GetAllTeachers(), "TeacherId", "FirstName");
            return View();
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,TeacherId,Name,NrCredits,CourseYear,Section,CourseType,Semester,GradingMethod,HasLaboratory,HasSeminar")] Course course)
        {
            if (ModelState.IsValid)
            {
                courseService.Create(course);
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeacherId"] = new SelectList(courseService.GetAllTeachers(), "TeacherId", "FirstName");
            return View(course);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = courseService.GetDetailsById(id);

            if (course == null)
            {
                return NotFound();
            }
            ViewData["TeacherId"] = new SelectList(courseService.GetAllTeachers(), "TeacherId", "FirstName");
            return View(course);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,TeacherId,Name,NrCredits,CourseYear,Section,CourseType,Semester,GradingMethod,HasLaboratory,HasSeminar")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    courseService.UpdateCourse(course);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!courseService.CourseExists(course.CourseId))
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
            ViewData["TeacherId"] = new SelectList(courseService.GetAllTeachers(), "TeacherId", "FirstName");
            return View(course);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = courseService.GetDetailsById(id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            courseService.DeleteCourse(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
