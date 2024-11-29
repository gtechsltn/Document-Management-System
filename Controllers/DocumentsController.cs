using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using DMS.Data;
using DMS.Models;
using DMS.Helpers;
using System.Text.Json;

namespace DMS.Controllers
{
    public class DocumentsController : BaseController
    {
        private readonly IStringLocalizer<DocumentsController> _localizer;
        private readonly DemoDMSContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;      
        private readonly UserInfo _userInfo;


        public DocumentsController(IStringLocalizer<DocumentsController> localizer, DemoDMSContext context, IHttpContextAccessor httpContextAccessor)
        {
            _localizer = localizer;
            _context = context;
            _httpContextAccessor = httpContextAccessor;

            var userJson = _httpContextAccessor.HttpContext.Session.GetString("UserInfo");
            if (userJson != null)
            {
                if (!string.IsNullOrEmpty(userJson))
                {
                    var userInfo = JsonSerializer.Deserialize<UserInfo>(userJson);
                    _userInfo = userInfo;
                }
            }
        }

        // GET: Documents
        [RoleAccessFilter("ConfigA")]
        public async Task<IActionResult> Index(string searchString)
        {

            if (_userInfo == null) return RedirectToAction("Index", "Home");
            Enum.TryParse<Faculty>(_userInfo.Division, out var divisionEnum);
            Enum.TryParse<Department>(_userInfo.Department, out var departmentEnum);

            var documents = from m in _context.Document where m.Faculty == divisionEnum && m.Department == departmentEnum select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                documents = documents.Where(s => s.Name.ToLower()!.Contains(searchString.ToLower()));               
            }

            var folders = from m in _context.Folder select m;

            foreach (var doc in documents)
            {
                var folder = folders.ToList().Find(m => m.Id == doc.ParentId);

                if (folder != null)
                {
                    doc.ParentFolder = folder.Name; // Ensure doc.ParentFolder is of type string
                }
            }


            ViewBag.searchString = searchString;

            return View(await documents.ToListAsync());
        }

        // GET: Documents/Details/5
        [RoleAccessFilter("ConfigA")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Document == null)
            {
                return NotFound();
            }

            var document = await _context.Document.FirstOrDefaultAsync(m => m.Id == id);

            if (document == null)
            {
                return NotFound();
            }

            ViewBag.parentId = document.ParentId;

            return View(document);
        }


        [RoleAccessFilter("ConfigA")]
        public async Task<IActionResult> Download(int? id)
        {
            if (id == null || _context.Document == null)
            {
                return NotFound();
            }

            var document = await _context.Document.FindAsync(id);

            if (document == null)
            {
                return NotFound();
            }

            string filePath = document.FilePath;
            string fileName = document.Name + document.Extension;
            string fileType = document.FileType;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, fileType, fileName);
        }

        // GET: Documents/Create

        [RoleAccessFilter("ConfigB")]
        public IActionResult Create(int parentId)
        {
            var folders = from m in _context.Folder select m;
            var folderlist = new List<SelectListItem>();


            foreach (var folder in folders) //.Where(m => m.ParentId == 0)
            {
                if (folder.IsComposite())
                    folderlist.Add(new SelectListItem { Text = folder.Name, Value = folder.Id.ToString() });
            }

            ViewBag.FolderList = folderlist; // Pass the list to the view

            ViewBag.parentId = parentId;

            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAccessFilter("ConfigB")]
        public async Task<IActionResult> Create(List<IFormFile> files, string name, string authorName, string supervisorName, Level level, Department department, Faculty faculty, DateTime publicationDate, int parentId)
        {
            foreach (var file in files)
            {
                var basePath = Path.Combine("Documents");
                if (parentId > 0)
                    basePath = Path.Combine("Documents/" + parentId); // add changes

                bool basePathExists = Directory.Exists(basePath);

                if (!basePathExists)
                {
                    Directory.CreateDirectory(basePath);
                }

                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var filePath = Path.Combine(basePath, file.FileName);
                var extension = Path.GetExtension(file.FileName);

                if (!System.IO.File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                //var offset = DateTimeOffset.Now.Offset;

                var document = new Document
                {
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    Name = name,
                    FilePath = filePath,
                    FileType = file.ContentType,
                    Extension = extension,
                    Size = file.Length,
                    AuthorName = authorName,
                    SupervisorName = supervisorName,
                    Level = level,
                    Department = department,
                    Faculty = faculty,
                    PublicationDate = publicationDate,
                    ParentId = parentId
                    //PublicationDate = new DateTimeOffset(publicationYear, publicationMonth, publicationDay, 0, 0, 0, offset),
                };

                _context.Document.Add(document);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Folders", new { id = parentId });
        }

        // GET: Documents/Edit/5
        [RoleAccessFilter("ConfigC")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Document == null)
            {
                return NotFound();
            }

            var document = await _context.Document.FindAsync(id);

            if (document == null)
            {
                return NotFound();
            }

            ViewBag.parentId = document.ParentId;

            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAccessFilter("ConfigC")]
        public async Task<IActionResult> Edit(int id, List<IFormFile> files, string name, string authorName, string supervisorName, Level level, Department department, Faculty faculty, DateTimeOffset publicationDate, int parentId)
        {

            if (id == null || _context.Document == null)
            {
                return NotFound();
            }

            var document = await _context.Document.FindAsync(id);

            if (document == null)
            {
                return NotFound();
            }

            if (id != document.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                ViewBag.parentId = parentId;
                bool isEmpty = !files.Any();
                IFormFile file = null;

                if (!isEmpty)
                {
                    file = files[0];
                }

                document.Name = name;
                document.AuthorName = authorName;

                if (!isEmpty)
                {
                    var basePath = Path.Combine("Documents");
                    bool basePathExists = Directory.Exists(basePath);

                    if (!basePathExists)
                    {
                        Directory.CreateDirectory(basePath);
                    }

                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    var filePath = Path.Combine(basePath, file.FileName);

                    if (!System.IO.File.Exists(filePath))
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }

                    document.DateModified = DateTime.Now;
                    document.Extension = Path.GetExtension(file.FileName);
                    document.Size = file.Length;
                    document.FileType = file.ContentType;
                    document.FilePath = filePath;
                    //var offset = DateTimeOffset.Now.Offset;
                    //document.PublicationDate = new DateTimeOffset(publicationYear, publicationMonth, publicationDay, 0, 0, 0, offset);
                }

                try
                {
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index", "Folders", new { id = parentId });
            }

            return View(document);
        }

        // GET: Documents/Delete/5
        [RoleAccessFilter("ConfigC")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Document == null)
            {
                return NotFound();
            }

            var document = await _context.Document.FirstOrDefaultAsync(m => m.Id == id);

            if (document == null)
            {
                return NotFound();
            }

            ViewBag.parentId = document.ParentId;

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [RoleAccessFilter("ConfigC")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Document == null)
            {
                return Problem("Entity set 'DemoDMSContext.Document' is null.");
            }

            var document = await _context.Document.FindAsync(id);

            if (document != null)
            {
                _context.Document.Remove(document);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Folders", new { id = document.ParentId });
        }

        private bool DocumentExists(int id)
        {
            return (_context.Document?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
