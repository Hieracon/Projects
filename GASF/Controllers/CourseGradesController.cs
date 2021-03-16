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
    public class CourseGradesController : Controller
    {
        private readonly CourseGradeService courseGradeService;

        public CourseGradesController(CourseGradeService courseGradeService)
        {
            this.courseGradeService = courseGradeService;
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // GET: CourseGrades
        public async Task<IActionResult> Index()
        {
            var courseGrade = courseGradeService.GetAllCourseGrades();
            return View(courseGrade);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // GET: CourseGrades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseGrade = courseGradeService.GetDetailsById(id);

            if (courseGrade == null)
            {
                return NotFound();
            }

            return View(courseGrade);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // GET: CourseGrades/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(courseGradeService.GetAllCourses(), "CourseId", "Name");
            ViewData["StudentId"] = new SelectList(courseGradeService.GetAllStudents(), "StudentId", "FirstName");
            return View();
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // POST: CourseGrades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseGradeId,StudentId,CourseId,ExamGrade,LabGrade,BonusPoints,IsGraduated")] CourseGrade courseGrade)
        {
            if (ModelState.IsValid)
            {
                courseGradeService.Create(courseGrade);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(courseGradeService.GetAllCourses(), "CourseId", "Name");
            ViewData["StudentId"] = new SelectList(courseGradeService.GetAllStudents(), "StudentId", "FirstName");
            return View(courseGrade);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // GET: CourseGrades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseGrade = courseGradeService.GetDetailsById(id);

            if (courseGrade == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(courseGradeService.GetAllCourses(), "CourseId", "Name");
            ViewData["StudentId"] = new SelectList(courseGradeService.GetAllStudents(), "StudentId", "FirstName");
            return View(courseGrade);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // POST: CourseGrades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseGradeId,StudentId,CourseId,ExamGrade,LabGrade,BonusPoints,IsGraduated")] CourseGrade courseGrade)
        {
            if (id != courseGrade.CourseGradeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    courseGradeService.UpdateCourseGrade(courseGrade);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!courseGradeService.CourseGradeExists(courseGrade.CourseGradeId))
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
            ViewData["CourseId"] = new SelectList(courseGradeService.GetAllCourses(), "CourseId", "Name");
            ViewData["StudentId"] = new SelectList(courseGradeService.GetAllStudents(), "StudentId", "FirstName");
            return View(courseGrade);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // GET: CourseGrades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseGrade = courseGradeService.GetDetailsById(id);

            if (courseGrade == null)
            {
                return NotFound();
            }

            return View(courseGrade);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // POST: CourseGrades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            courseGradeService.DeleteCourseGrade(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
