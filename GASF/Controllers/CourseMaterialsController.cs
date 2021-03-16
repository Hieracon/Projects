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
    public class CourseMaterialsController : Controller
    {
        private readonly CourseMaterialService courseMaterialService;

        public CourseMaterialsController(CourseMaterialService courseMaterialService)
        {
            this.courseMaterialService = courseMaterialService;
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // GET: CourseMaterials
        public async Task<IActionResult> Index()
        {
            var courseMaterial = courseMaterialService.GetAllCourseMaterials();
            return View(courseMaterial);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // GET: CourseMaterials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseMaterial = courseMaterialService.GetDetailsById(id);

            if (courseMaterial == null)
            {
                return NotFound();
            }

            return View(courseMaterial);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // GET: CourseMaterials/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(courseMaterialService.GetAllCourses(), "CourseId", "Name");
            return View();
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // POST: CourseMaterials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseMaterialId,CourseId,LinkCourseMaterial,LinkSeminarMaterial,LinkLaboratoryMaterial")] CourseMaterial courseMaterial)
        {
            if (ModelState.IsValid)
            {
                courseMaterialService.Create(courseMaterial);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(courseMaterialService.GetAllCourses(), "CourseId", "Name");
            return View(courseMaterial);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // GET: CourseMaterials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseMaterial = courseMaterialService.GetDetailsById(id);

            if (courseMaterial == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(courseMaterialService.GetAllCourses(), "CourseId", "Name");
            return View(courseMaterial);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // POST: CourseMaterials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseMaterialId,CourseId,LinkCourseMaterial,LinkSeminarMaterial,LinkLaboratoryMaterial")] CourseMaterial courseMaterial)
        {
            if (id != courseMaterial.CourseMaterialId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    courseMaterialService.UpdateCourseMaterial(courseMaterial);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!courseMaterialService.CourseMaterialExists(courseMaterial.CourseMaterialId))
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
            ViewData["CourseId"] = new SelectList(courseMaterialService.GetAllCourses(), "CourseId", "Name");
            return View(courseMaterial);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // GET: CourseMaterials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseMaterial = courseMaterialService.GetDetailsById(id);

            if (courseMaterial == null)
            {
                return NotFound();
            }

            return View(courseMaterial);
        }

        [Authorize(Roles = "Secretary,Teacher")]
        // POST: CourseMaterials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            courseMaterialService.DeleteCourseMaterial(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
