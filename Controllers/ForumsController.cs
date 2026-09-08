using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Talk2Me.Data;
using Talk2Me.Models;

namespace Talk2Me.Controllers
{
    public class ForumsController : Controller
    {
        private readonly AppDbContext _context;

        public ForumsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Forms
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Forums.Include(f => f.Creator).Include(f => f.Theme);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Forms/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forum = await _context.Forums
                .Include(f => f.Creator)
                .Include(f => f.Theme)
                .FirstOrDefaultAsync(m => m.ForumId == id);
            if (forum == null)
            {
                return NotFound();
            }

            return View(forum);
        }

        // GET: Forms/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "PasswordHash");
            ViewData["ThemeId"] = new SelectList(_context.Themes, "ThemeId", "ThemeName");
            return View();
        }

        // POST: Forms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ForumId,State,CreationDate,UserId,ThemeId")] Forum forum)
        {
            if (ModelState.IsValid)
            {
                forum.ForumId = Guid.NewGuid();
                _context.Add(forum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "PasswordHash", forum.UserId);
            ViewData["ThemeId"] = new SelectList(_context.Themes, "ThemeId", "ThemeName", forum.ThemeId);
            return View(forum);
        }

        // GET: Forms/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forum = await _context.Forums.FindAsync(id);
            if (forum == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "PasswordHash", forum.UserId);
            ViewData["ThemeId"] = new SelectList(_context.Themes, "ThemeId", "ThemeName", forum.ThemeId);
            return View(forum);
        }

        // POST: Forms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ForumId,State,CreationDate,UserId,ThemeId")] Forum forum)
        {
            if (id != forum.ForumId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(forum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ForumExists(forum.ForumId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "PasswordHash", forum.UserId);
            ViewData["ThemeId"] = new SelectList(_context.Themes, "ThemeId", "ThemeName", forum.ThemeId);
            return View(forum);
        }

        // GET: Forms/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forum = await _context.Forums
                .Include(f => f.Creator)
                .Include(f => f.Theme)
                .FirstOrDefaultAsync(m => m.ForumId == id);
            if (forum == null)
            {
                return NotFound();
            }

            return View(forum);
        }

        // POST: Forms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var forum = await _context.Forums.FindAsync(id);
            if (forum != null)
            {
                _context.Forums.Remove(forum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ForumExists(Guid id)
        {
            return _context.Forums.Any(e => e.ForumId == id);
        }
    }
}
