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
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;

namespace GASF.Controllers
{
    public class SecretariesController : Controller
    {
        private readonly SecretaryService secretaryService;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public SecretariesController(SecretaryService secretaryService,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.secretaryService = secretaryService;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Secretary")]
        // GET: Secretaries
        public async Task<IActionResult> Index()
        {
            var secretary = secretaryService.GetAllSecretaries();
            return View(secretary);
        }

        [Authorize(Roles = "Secretary")]
        // GET: Secretaries/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var secretary = secretaryService.GetDetailsById(id);

            if (secretary == null)
            {
                return NotFound();
            }

            return View(secretary);
        }

        [Authorize(Roles = "Secretary")]
        // GET: Secretaries/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Secretary")]
        // POST: Secretaries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SecretaryId,CNP,FirstName,LastName,MailAddress,Password,Address,Birthday")] Secretary secretary)
        {
            var user = new IdentityUser { UserName = secretary.MailAddress, Email = secretary.MailAddress };
            var result = await userManager.CreateAsync(user, secretary.Password);

            if (ModelState.IsValid)
            {
                await userManager.AddToRoleAsync(user, "Secretary");
                secretaryService.Create(secretary);
                return RedirectToAction(nameof(Index));
            }
            return View(secretary);
        }
        
        [Authorize(Roles = "Secretary")]
        // GET: Secretaries/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var secretary = secretaryService.GetDetailsById(id);

            if (secretary == null)
            {
                return NotFound();
            }
            return View(secretary);
        }

        [Authorize(Roles = "Secretary")]
        // POST: Secretaries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("SecretaryId,CNP,FirstName,LastName,MailAddress,Password,Address,Birthday")] Secretary secretary)
        {
            if (id != secretary.SecretaryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    secretaryService.UpdateSecretary(secretary);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!secretaryService.SecretaryExists(secretary.SecretaryId))
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
            return View(secretary);
        }

        [Authorize(Roles = "Secretary")]
        // GET: Secretaries/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var secretary = secretaryService.GetDetailsById(id);

            if (secretary == null)
            {
                return NotFound();
            }

            return View(secretary);
        }

        [Authorize(Roles = "Secretary")]
        // POST: Secretaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            secretaryService.DeleteSecretary(id);
            return RedirectToAction(nameof(Index));
        }

        public void ExportRATCertificatePDF(String savePath, string title)
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
            prgAuthor.Add(new Chunk("\nDate : " + DateTime.Now.ToShortDateString()));
            document.Add(prgAuthor);

            //Add a line seperation
            Paragraph p = new Paragraph();
            document.Add(p);

            //Add line break
            document.Add(new Chunk("\n"));
            document.Add(new Chunk("Through my mighty presence it is fortold that SIR/LADY ............................. " +
                "is studnet in year ........... at University ..................,section ...................,year of study ............,tax/budget ........\n"));
            document.Add(new Chunk("\n"));
            document.Add(new Chunk("\n"));
            document.Add(new Chunk("\n"));
            document.Add(new Chunk("Sign Here :\n"));
            Paragraph stamp = new Paragraph();
            BaseFont btnStamp = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            stamp.Alignment = Element.ALIGN_RIGHT;
            stamp.Add(new Chunk("Stamp : "));
            document.Add(stamp);

            document.Close();
            writer.Close();
            fs.Close();
        }

        public void ExportExamCertificatePDF(String savePath, string title)
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
            prgAuthor.Add(new Chunk("\nDate : " + DateTime.Now.ToShortDateString()));
            document.Add(prgAuthor);

            //Add a line seperation
            Paragraph p = new Paragraph();
            document.Add(p);

            //Add line break
            document.Add(new Chunk("\n"));
            document.Add(new Chunk("Through my mighty presence it is fortold that SIR/LADY ............................. " +
                "is studnet in year ........... at University ..................,section ...................,student year of study ............,tax/budget ........" +
                "course ...........,course year of study .........,exam price .......\n"));
            document.Add(new Chunk("\n"));
            document.Add(new Chunk("\n"));
            document.Add(new Chunk("\n"));
            document.Add(new Chunk("Sign Here :\n"));
            Paragraph stamp = new Paragraph();
            BaseFont btnStamp = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            stamp.Alignment = Element.ALIGN_RIGHT;
            stamp.Add(new Chunk("Stamp : "));
            document.Add(stamp);

            document.Close();
            writer.Close();
            fs.Close();
        }

        [Authorize(Roles = "Secretary")]
        public async Task<ViewResult> GenerateCertificate()
        {
           return View();
        }

        [Authorize(Roles = "Secretary")]
        public async Task<ViewResult> GenerateRATCertificatePDF()
        {
            ExportRATCertificatePDF(@"PDF/RATCertificate.pdf", "R.A.T. Certificate");

            return View("GenerateCertificate");
        }

        [Authorize(Roles = "Secretary")]
        public async Task<ViewResult> GenerateExamCertificatePDF()
        {
            ExportExamCertificatePDF(@"PDF/ExamCertificate.pdf", "Exam Certificate");

            return View("GenerateCertificate");
        }
    }
}
