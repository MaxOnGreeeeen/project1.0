using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projcet1._0.Data;
using projcet1._0.Models;
using Microsoft.AspNetCore.Http;
using projcet1._0.Models.ViewModels;

namespace projcet1._0.Controllers
{
    public class ArticleLikesController : Controller
    {
        private readonly ApplicationDbContext context;

        public ArticleLikesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: ArticleLikes
        /*
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ArticleLike.Include(a => a.Article).Include(a => a.Like);
            return View(await applicationDbContext.ToListAsync());
        }*/
        public async Task<IActionResult> Index(Int32? articleID)
        {
            if (articleID == null)
            {
                return this.NotFound();
            }

            var article = await this.context.Articles
                .SingleOrDefaultAsync(x => x.Id == articleID);

            if (article == null)
            {
                return this.NotFound();
            }

            var items = await this.context.ArticleLike
                .Include(h => h.Article)
                .Include(h => h.Like)
                .Where(x => x.ArticleID == articleID)
                .ToListAsync();
            this.ViewBag.Hospital = article;
            return this.View(items);
        }

        // GET: ArticleLikes/Create
        public async Task<IActionResult> Create(Int32? articleID)
        {
            if (articleID == null)
            {
                return this.NotFound();
            }

            var article = await this.context.Articles
                .SingleOrDefaultAsync(x => x.Id == articleID);

            if (article == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Articles = article;
            this.ViewData["LabId"] = new SelectList(this.context.Likes, "Id", "Name");
            return this.View(new ArticleLikeCreateModel());
        }

        // POST: ArticleLikes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
     
        public async Task<IActionResult> Create(Int32? articleID, ArticleLikeCreateModel model)
        {
            if (articleID == null)
            {
                return this.NotFound();
            }

            var article = await this.context.Articles
                .SingleOrDefaultAsync(x => x.Id == articleID);

            if (article == null)
            {
                return this.NotFound();
            }

            var item = await this.context.ArticleLike
                .Include(h => h.Article)
                .Include(h => h.Like)
                .SingleOrDefaultAsync(m => m.ArticleID == articleID && m.LikeID == model.LikeID);

            if (item != null)
            {
                item.TotalLikeCount += 1;

                this.ViewBag.Article = article;
                this.ViewData["LikeID"] = new SelectList(this.context.Likes, "Id", "Name", model.LikeID);
                return this.View(model);
            }

            if (this.ModelState.IsValid)
            {
                var articlelike = new ArticleLike
                {
                    ArticleID = article.Id,
                    LikeID = model.LikeID,
                    TotalLikeCount = 0
                };

                this.context.Add(articlelike);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { article = article.Id });
            }

            this.ViewBag.Article = article;
            this.ViewData["LikeID"] = new SelectList(this.context.Likes, "Id", "Name", model.LikeID);
            return this.View(model);
        }

        // GET: ArticleLikes/Edit/5


        // GET: ArticleLikes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleLike = await context.ArticleLike
                .Include(a => a.Article)
                .Include(a => a.Like)
                .FirstOrDefaultAsync(m => m.LikeID == id);
            if (articleLike == null)
            {
                return NotFound();
            }

            return View(articleLike);
        }

        // POST: ArticleLikes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articleLike = await context.ArticleLike.FindAsync(id);
            context.ArticleLike.Remove(articleLike);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleLikeExists(int id)
        {
            return context.ArticleLike.Any(e => e.LikeID == id);
        }
    }
}
