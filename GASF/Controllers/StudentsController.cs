using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GASF.Data;
using GASF.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;
using GASF.Services;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using GASF.Services.ImplementationServices;
using Microsoft.AspNetCore.Authorization;

namespace GASF.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentService studentService;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly GASFContext context;

        public StudentsController(StudentService studentService,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            GASFContext context)
        {
            this.studentService = studentService;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.context = context;
        }

        [Authorize(Roles = "Secretary")]
        // GET: Students
        public async Task<IActionResult> Index()
        {
            var student = studentService.GetAllStudents();
            return View(student);
        }

        [Authorize(Roles = "Secretary")]
        // GET: Students/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = studentService.GetDetailsById(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [Authorize(Roles = "Secretary")]
        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Secretary")]
        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,CNP,FirstName,LastName,MailAddress,Password,DadInitials,Address,Birthday,Credits,CurrentYear,Section")] Student student)
        {
            var user = new IdentityUser { UserName = student.MailAddress, Email = student.MailAddress };

            var result = await userManager.CreateAsync(user, student.Password);
            var userId = user.Id;
            student.StudentId = user.Id;

            if (ModelState.IsValid)
            {
                await userManager.AddToRoleAsync(user, "Student");
                studentService.Create(student);
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        [Authorize(Roles = "Secretary")]
        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = studentService.GetDetailsById(id);

            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [Authorize(Roles = "Secretary")]
        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,
            [Bind("StudentId,CNP,FirstName,LastName,MailAddress,Password,DadInitials,Address,Birthday,Credits,CurrentYear,Section")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    studentService.UpdateStudent(student);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!studentService.StudentExists(student.StudentId))
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
            return View(student);
        }

        [Authorize(Roles = "Secretary")]
        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = studentService.GetDetailsById(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [Authorize(Roles = "Secretary")]
        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            studentService.DeleteStudent(id);
            return RedirectToAction(nameof(Index));
        }

        public void ExportPersonalDataPDF(DataTable dataTable, String savePath, string title)
        {
            System.IO.FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None);
            iTextSharp.text.Document document = new iTextSharp.text.Document();
            document.SetPageSize(iTextSharp.text.PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            //Report Header
            BaseFont bfntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Paragraph prgHeading = new Paragraph();
            prgHeading.Alignment = Element.ALIGN_CENTER;
            prgHeading.Add(new Chunk(title.ToUpper()));
            document.Add(prgHeading);

            //Author
            Paragraph prgAuthor = new Paragraph();
            BaseFont btnAuthor = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            prgAuthor.Alignment = Element.ALIGN_RIGHT;
            prgAuthor.Add(new Chunk("Author : GASF"));
            prgAuthor.Add(new Chunk("\nRun Date : " + DateTime.Now.ToShortDateString()));
            document.Add(prgAuthor);

            //Add a line seperation
            Paragraph p = new Paragraph();
            document.Add(p);

            //Add line break
            document.Add(new Chunk("\n"));

            //Write the table
            PdfPTable table = new PdfPTable(dataTable.Columns.Count);
            //Table header
            BaseFont btnColumnHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                document.Add(new Chunk(dataTable.Columns[i].ColumnName + " : " + dataTable.Rows[0][i].ToString()));
                document.Add(new Chunk("\n"));
            }

            document.Add(table);
            document.Close();
            writer.Close();
            fs.Close();
        }

        public void ExportSchoolSituationPDF(DataTable dataTable, String savePath, string title)
        {
            System.IO.FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None);
            iTextSharp.text.Document document = new iTextSharp.text.Document();
            document.SetPageSize(iTextSharp.text.PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            //Report Header
            BaseFont bfntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Paragraph prgHeading = new Paragraph();
            prgHeading.Alignment = Element.ALIGN_CENTER;
            prgHeading.Add(new Chunk(title.ToUpper() + " : " + dataTable.Rows[0][0].ToString()));
            document.Add(prgHeading);

            //Author
            Paragraph prgAuthor = new Paragraph();
            BaseFont btnAuthor = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            prgAuthor.Alignment = Element.ALIGN_RIGHT;
            prgAuthor.Add(new Chunk("Author : GASF"));
            prgAuthor.Add(new Chunk("\nRun Date : " + DateTime.Now.ToShortDateString()));
            document.Add(prgAuthor);

            //Add a line seperation
            Paragraph p = new Paragraph();
            document.Add(p);

            //Add line break
            document.Add(new Chunk("\n"));

            //Write the table
            PdfPTable table = new PdfPTable(dataTable.Columns.Count - 1);
            //Table header
            BaseFont btnColumnHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            for (int i = 1; i < dataTable.Columns.Count; i++)
            {
                PdfPCell cell = new PdfPCell();
                cell.AddElement(new Chunk(dataTable.Columns[i].ColumnName.ToUpper()));
                table.AddCell(cell);
            }
            //table Data
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                for (int j = 1; j < dataTable.Columns.Count; j++)
                {
                    table.AddCell(dataTable.Rows[i][j].ToString());
                }
            }

            document.Add(table);
            document.Close();
            writer.Close();
            fs.Close();
        }

        public void ExportInvoicePDF(DataTable dataTable, String savePath, string title)
        {
            System.IO.FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None);
            iTextSharp.text.Document document = new iTextSharp.text.Document();
            document.SetPageSize(iTextSharp.text.PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            //Report Header
            BaseFont bfntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Paragraph prgHeading = new Paragraph();
            prgHeading.Alignment = Element.ALIGN_CENTER;
            prgHeading.Add(new Chunk(title.ToUpper()));
            document.Add(prgHeading);

            //Author
            Paragraph prgData = new Paragraph();
            BaseFont btnData = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            prgData.Alignment = Element.ALIGN_RIGHT;
            prgData.Add(new Chunk("Series : 167141"));
            prgData.Add(new Chunk("\nNumber : 000564363"));
            prgData.Add(new Chunk("\nDate : " + DateTime.Now.ToShortDateString()));
            document.Add(prgData);

            //Add a line seperation
            Paragraph p = new Paragraph();
            document.Add(p);

            //Add line break
            document.Add(new Chunk("\n"));
            document.Add(new Chunk("A.C.E. Craiova.\n"));
            document.Add(new Chunk("Address : Str. A. I. Cuza Nr.13.\n"));
            document.Add(new Chunk("City : Craiova, Dolj\n"));
            document.Add(new Chunk("Tel. : 0251788787.\n"));
            document.Add(new Chunk("\n"));
            document.Add(new Chunk("\n"));

            //Write the table
            PdfPTable table = new PdfPTable(dataTable.Columns.Count);
            //Table header
            BaseFont btnColumnHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                document.Add(new Chunk(dataTable.Columns[i].ColumnName + " : " + dataTable.Rows[0][i].ToString()));
                document.Add(new Chunk("\n"));
            }

            document.Add(new Chunk("\n"));
            document.Add(new Chunk("\n"));
            document.Add(new Chunk("Services : Failed Exam.\n"));
            document.Add(new Chunk("Price : 50 LIONS."));
            document.Add(new Chunk("\n"));

            document.Close();
            writer.Close();
            fs.Close();
        }

        public void ExportCourseMaterialsPDF(DataTable dataTable, String savePath, string title)
        {
            System.IO.FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None);
            iTextSharp.text.Document document = new iTextSharp.text.Document();
            document.SetPageSize(iTextSharp.text.PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            //Report Header
            BaseFont bfntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Paragraph prgHeading = new Paragraph();
            prgHeading.Alignment = Element.ALIGN_CENTER;
            prgHeading.Add(new Chunk(title.ToUpper() + " : " + dataTable.Rows[0][0].ToString()));
            document.Add(prgHeading);

            //Author
            Paragraph prgAuthor = new Paragraph();
            BaseFont btnAuthor = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            prgAuthor.Alignment = Element.ALIGN_RIGHT;
            prgAuthor.Add(new Chunk("Author : GASF"));
            prgAuthor.Add(new Chunk("\nRun Date : " + DateTime.Now.ToShortDateString()));
            document.Add(prgAuthor);

            //Add a line seperation
            Paragraph p = new Paragraph();
            document.Add(p);

            //Add line break
            document.Add(new Chunk("\n"));

            //Write the table
            PdfPTable table = new PdfPTable(dataTable.Columns.Count - 1);
            //Table header
            BaseFont btnColumnHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            for (int i = 1; i < dataTable.Columns.Count; i++)
            {
                PdfPCell cell = new PdfPCell();
                cell.AddElement(new Chunk(dataTable.Columns[i].ColumnName.ToUpper()));
                table.AddCell(cell);
            }
            //table Data
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                for (int j = 1; j < dataTable.Columns.Count; j++)
                {
                    table.AddCell(dataTable.Rows[i][j].ToString());
                }
            }

            document.Add(table);
            document.Close();
            writer.Close();
            fs.Close();
        }

        [Authorize(Roles = "Secretary,Student")]
        public async Task<ViewResult> PersonalData()
        {
            DataTable table = new DataTable();

            table.Columns.Add("CNP");
            table.Columns.Add("FirstName");
            table.Columns.Add("LastName");
            table.Columns.Add("MailAddress");
            table.Columns.Add("DadInitials");
            table.Columns.Add("Birthday");
            table.Columns.Add("Address");
            table.Columns.Add("Credits");
            table.Columns.Add("CurrentYear");
            table.Columns.Add("Section");

            var userId = userManager.GetUserId(User);
            var student = studentService.GetDetailsById(userId);

            table.Rows.Add(student.CNP, student.FirstName, student.LastName, student.MailAddress,
                student.DadInitials, student.Birthday, student.Address, student.Credits, student.CurrentYear, student.Section);

            ViewData["table"] = table;

            return View();
        }

        [Authorize(Roles = "Secretary,Student")]
        public async Task<ViewResult> PersonalDataPDF()
        {
            DataTable table = new DataTable();

            table.Columns.Add("CNP");
            table.Columns.Add("FirstName");
            table.Columns.Add("LastName");
            table.Columns.Add("MailAddress");
            table.Columns.Add("DadInitials");
            table.Columns.Add("Birthday");
            table.Columns.Add("Address");
            table.Columns.Add("Credits");
            table.Columns.Add("CurrentYear");
            table.Columns.Add("Section");

            var userId = userManager.GetUserId(User);
            var student = studentService.GetDetailsById(userId);

            table.Rows.Add(student.CNP, student.FirstName, student.LastName, student.MailAddress,
                student.DadInitials, student.Birthday, student.Address, student.Credits, student.CurrentYear, student.Section);

            ViewData["table"] = table;

            ExportPersonalDataPDF(table, @"PDF/PersonalData.pdf", "Personal Data");

            return View("PersonalData");
        }

        [Authorize(Roles = "Secretary,Student")]
        public async Task<ViewResult> SchoolSituation()
        {
            DataTable table = new DataTable();

            table.Columns.Add("Student");
            table.Columns.Add("Course");
            table.Columns.Add("Grade");
            table.Columns.Add("Course Attendances");
            table.Columns.Add("Laboratory Attendances");

            var userId = userManager.GetUserId(User);
            var student = studentService.GetDetailsById(userId);

            var badges = (from course in context.Courses
                          from attendance in context.CourseAttendances
                          from grade in context.CourseGrades
                          where student.StudentId == attendance.StudentId &&
                                student.StudentId == grade.StudentId &&
                                course.CourseId == attendance.CourseId &&
                                course.CourseId == grade.CourseId
                          select new { student.FirstName, course.Name, grade.ExamGrade, attendance.NrCourseAttendances, attendance.NrLaboratoryAttendances}).ToList();

            foreach (var schoolSituation in badges)
            {
                table.Rows.Add(schoolSituation.FirstName, schoolSituation.Name, schoolSituation.ExamGrade,
                                schoolSituation.NrCourseAttendances, schoolSituation.NrLaboratoryAttendances);
            }

            ViewData["table"] = table;

            return View();
        }

        [Authorize(Roles = "Secretary,Student")]
        public async Task<ViewResult> SchoolSituationPDF()
        {
            DataTable table = new DataTable();

            table.Columns.Add("Student");
            table.Columns.Add("Course");
            table.Columns.Add("Grade");
            table.Columns.Add("Course Attendances");
            table.Columns.Add("Laboratory Attendances");

            var userId = userManager.GetUserId(User);
            var student = studentService.GetDetailsById(userId);

            var badges = (from course in context.Courses
                          from attendance in context.CourseAttendances
                          from grade in context.CourseGrades
                          where student.StudentId == attendance.StudentId &&
                                student.StudentId == grade.StudentId &&
                                course.CourseId == attendance.CourseId &&
                                course.CourseId == grade.CourseId
                          select new { student.FirstName, course.Name, grade.ExamGrade, attendance.NrCourseAttendances, attendance.NrLaboratoryAttendances }).ToList();

            foreach (var schoolSituation in badges)
            {
                table.Rows.Add(schoolSituation.FirstName, schoolSituation.Name, schoolSituation.ExamGrade,
                                schoolSituation.NrCourseAttendances, schoolSituation.NrLaboratoryAttendances);
            }

            ViewData["table"] = table;

            ExportSchoolSituationPDF(table, @"PDF/SchoolSitutation.pdf", "School Situation");

            return View("SchoolSituation");
        }

        [Authorize(Roles = "Secretary,Student")]
        public ViewResult Invoice()
        {
            DataTable table = new DataTable();

            table.Columns.Add("Student");
            table.Columns.Add("CNP");
            table.Columns.Add("Address");

            var userId = userManager.GetUserId(User);
            var student = studentService.GetDetailsById(userId);

            table.Rows.Add(student.FirstName, student.CNP, student.Address);

            ViewData["table"] = table;

            ExportInvoicePDF(table, @"PDF/Invoice.pdf", "Invoice");

            return View();
        }

        [Authorize(Roles = "Secretary,Student")]
        public async Task<ViewResult> CourseMaterials()
        {
            DataTable table = new DataTable();

            table.Columns.Add("Student");
            table.Columns.Add("Course");
            table.Columns.Add("Course Materials");
            table.Columns.Add("Seminary Materials");
            table.Columns.Add("Laboratory Materials");

            var userId = userManager.GetUserId(User);
            var student = studentService.GetDetailsById(userId);

            var badges = (from course in context.Courses
                          from attendance in context.CourseAttendances
                          from material in context.CourseMaterials
                          where student.StudentId == attendance.StudentId &&
                                course.CourseId == attendance.CourseId &&
                                course.CourseId == material.CourseId
                          select new { student.FirstName, course.Name, material.LinkCourseMaterial,
                                       material.LinkSeminarMaterial, material.LinkLaboratoryMaterial }).ToList();

            foreach (var schoolSituation in badges)
            {
                table.Rows.Add(schoolSituation.FirstName, schoolSituation.Name, schoolSituation.LinkCourseMaterial,
                                schoolSituation.LinkSeminarMaterial, schoolSituation.LinkLaboratoryMaterial);
            }

            ViewData["table"] = table;

            return View();
        }

        [Authorize(Roles = "Secretary,Student")]
        public async Task<ViewResult> CourseMaterialsPDF()
        {
            DataTable table = new DataTable();

            table.Columns.Add("Student");
            table.Columns.Add("Course");
            table.Columns.Add("Course Materials");
            table.Columns.Add("Seminary Materials");
            table.Columns.Add("Laboratory Materials");

            var userId = userManager.GetUserId(User);
            var student = studentService.GetDetailsById(userId);

            var badges = (from course in context.Courses
                          from attendance in context.CourseAttendances
                          from material in context.CourseMaterials
                          where student.StudentId == attendance.StudentId &&
                                course.CourseId == attendance.CourseId &&
                                course.CourseId == material.CourseId
                          select new
                          {
                              student.FirstName,
                              course.Name,
                              material.LinkCourseMaterial,
                              material.LinkSeminarMaterial,
                              material.LinkLaboratoryMaterial
                          }).ToList();

            foreach (var schoolSituation in badges)
            {
                table.Rows.Add(schoolSituation.FirstName, schoolSituation.Name, schoolSituation.LinkCourseMaterial,
                                schoolSituation.LinkSeminarMaterial, schoolSituation.LinkLaboratoryMaterial);
            }

            ViewData["table"] = table;

            ExportCourseMaterialsPDF(table, @"PDF/CourseMaterials.pdf", "Course Materials");

            return View("CourseMaterials");
        }
    }
}
