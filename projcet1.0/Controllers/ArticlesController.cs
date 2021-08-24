using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projcet1._0.Data;
using projcet1._0.Models;
using projcet1._0.Models.ViewModels;
using projcet1._0.Services;

namespace projcet1._0.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private static readonly HashSet<String> AllowedExtensions = new HashSet<String> { ".jpg", ".jpeg", ".png", ".gif" };
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserPermissionsService userPermissions;

        public ArticlesController(ApplicationDbContext context,
            IHostingEnvironment hostingEnvironment,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IUserPermissionsService userPermissions)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            this.userPermissions = userPermissions;
        }

        // GET: Articles
        public async Task<IActionResult> Index()
        {

            var applicationDbContext = _context.Articles.Include(a => a.Categories);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Categories)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            ViewBag.CanEditArticle = userPermissions.CanEditArticle(article);

            return View(article);
        }

        // GET: Articles/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(this._context.Categories, "Id", "CategoryOfArticle");
            return View(new ArticleCreateModel());
        }

        // POST: Articles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(ArticleCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var article = new Article
                {
                   
                    Name = model.Name,
                    Location = model.Location,
                    Text = model.Text,
                    CategoryId = model.CategoryId,
                    Data = DateTime.Now
                };

                article.Author = this.userManager.GetUserId(this.httpContextAccessor.HttpContext.User);
                var fileName = Path.GetFileName(ContentDispositionHeaderValue.Parse(model.File.ContentDisposition).FileName.Trim('"'));
                var fileExt = Path.GetExtension(fileName);
                if (!ArticlesController.AllowedExtensions.Contains(fileExt))
                {
                    this.ModelState.AddModelError(nameof(model.File), "This file type is prohibited");
                }

                var ImgId = Guid.NewGuid();
                var attachmentPath = Path.Combine(this.hostingEnvironment.WebRootPath, "attachments", ImgId.ToString("N") + fileExt);
                article.Img = $"/attachments/{ImgId:N}{fileExt}";
                using (var fileStream = new FileStream(attachmentPath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read))
                {
                    await model.File.CopyToAsync(fileStream);
                }


                this._context.Articles.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryId = new SelectList(this._context.Categories, "Id", "Id");
            return this.View(model);
        }

        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", article.CategoryId);
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Author,Location,Text,Img,CategoryId,Data")] Article article)
        {
            if (id != article.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", article.CategoryId);
            return View(article);
        }

        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Categories)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            var imgPath = this.hostingEnvironment.WebRootPath + "/" + article.Img;
            System.IO.File.Delete(imgPath);
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}
