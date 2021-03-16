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
    public class CourseAttendancesController : Controller
    {
        private readonly CourseAttendanceService courseAttendanceService;

        public CourseAttendancesController(CourseAttendanceService courseAttendanceService)
        {
            this.courseAttendanceService = courseAttendanceService;
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // GET: CourseAttendances
        public async Task<IActionResult> Index()
        {
            var courseAttendance = courseAttendanceService.GetAllCourseAttendances();
            return View(courseAttendance);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // GET: CourseAttendances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseAttendance = courseAttendanceService.GetDetailsById(id);

            if (courseAttendance == null)
            {
                return NotFound();
            }

            return View(courseAttendance);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // GET: CourseAttendances/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(courseAttendanceService.GetAllCourses(), "CourseId", "Name");
            ViewData["StudentId"] = new SelectList(courseAttendanceService.GetAllStudents(), "StudentId", "FirstName");
            return View();
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // POST: CourseAttendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseAttendanceId,StudentId,CourseId,NrCourseAttendances,NrLaboratoryAttendances")] CourseAttendance courseAttendance)
        {
            if (ModelState.IsValid)
            {
                courseAttendanceService.Create(courseAttendance);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(courseAttendanceService.GetAllCourses(), "CourseId", "Name");
            ViewData["StudentId"] = new SelectList(courseAttendanceService.GetAllStudents(), "StudentId", "FirstName");
            return View(courseAttendance);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // GET: CourseAttendances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseAttendance = courseAttendanceService.GetDetailsById(id);

            if (courseAttendance == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(courseAttendanceService.GetAllCourses(), "CourseId", "Name");
            ViewData["StudentId"] = new SelectList(courseAttendanceService.GetAllStudents(), "StudentId", "FirstName");
            return View(courseAttendance);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // POST: CourseAttendances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseAttendanceId,StudentId,CourseId,NrCourseAttendances,NrLaboratoryAttendances")] CourseAttendance courseAttendance)
        {
            if (id != courseAttendance.CourseAttendanceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    courseAttendanceService.UpdateCourseAttendance(courseAttendance);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!courseAttendanceService.CourseAttendanceExists(courseAttendance.CourseAttendanceId))
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
            ViewData["CourseId"] = new SelectList(courseAttendanceService.GetAllCourses(), "CourseId", "Name");
            ViewData["StudentId"] = new SelectList(courseAttendanceService.GetAllStudents(), "StudentId", "FirstName");
            return View(courseAttendance);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // GET: CourseAttendances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseAttendance = courseAttendanceService.GetDetailsById(id);

            if (courseAttendance == null)
            {
                return NotFound();
            }

            return View(courseAttendance);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // POST: CourseAttendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            courseAttendanceService.DeleteCourseAttendance(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
