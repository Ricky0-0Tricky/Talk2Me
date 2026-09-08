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
    public class ReactionsController : Controller
    {
        private readonly AppDbContext _context;

        public ReactionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Reactions
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Reactions.Include(r => r.Comment).Include(r => r.Forum).Include(r => r.Reactor);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Reactions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reaction = await _context.Reactions
                .Include(r => r.Comment)
                .Include(r => r.Forum)
                .Include(r => r.Reactor)
                .FirstOrDefaultAsync(m => m.ReactionId == id);
            if (reaction == null)
            {
                return NotFound();
            }

            return View(reaction);
        }

        // GET: Reactions/Create
        public IActionResult Create()
        {
            ViewData["CommentId"] = new SelectList(_context.Comments, "CommentId", "Content");
            ViewData["ForumId"] = new SelectList(_context.Forums, "ForumId", "State");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "PasswordHash");
            return View();
        }

        // POST: Reactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReactionId,IsUpvote,ForumId,CommentId,UserId")] Reaction reaction)
        {
            if (ModelState.IsValid)
            {
                reaction.ReactionId = Guid.NewGuid();
                _context.Add(reaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CommentId"] = new SelectList(_context.Comments, "CommentId", "Content", reaction.CommentId);
            ViewData["ForumId"] = new SelectList(_context.Forums, "ForumId", "State", reaction.ForumId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "PasswordHash", reaction.UserId);
            return View(reaction);
        }

        // GET: Reactions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reaction = await _context.Reactions.FindAsync(id);
            if (reaction == null)
            {
                return NotFound();
            }
            ViewData["CommentId"] = new SelectList(_context.Comments, "CommentId", "Content", reaction.CommentId);
            ViewData["ForumId"] = new SelectList(_context.Forums, "ForumId", "State", reaction.ForumId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "PasswordHash", reaction.UserId);
            return View(reaction);
        }

        // POST: Reactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ReactionId,IsUpvote,ForumId,CommentId,UserId")] Reaction reaction)
        {
            if (id != reaction.ReactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReactionExists(reaction.ReactionId))
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
            ViewData["CommentId"] = new SelectList(_context.Comments, "CommentId", "Content", reaction.CommentId);
            ViewData["ForumId"] = new SelectList(_context.Forums, "ForumId", "State", reaction.ForumId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "PasswordHash", reaction.UserId);
            return View(reaction);
        }

        // GET: Reactions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reaction = await _context.Reactions
                .Include(r => r.Comment)
                .Include(r => r.Forum)
                .Include(r => r.Reactor)
                .FirstOrDefaultAsync(m => m.ReactionId == id);
            if (reaction == null)
            {
                return NotFound();
            }

            return View(reaction);
        }

        // POST: Reactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var reaction = await _context.Reactions.FindAsync(id);
            if (reaction != null)
            {
                _context.Reactions.Remove(reaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReactionExists(Guid id)
        {
            return _context.Reactions.Any(e => e.ReactionId == id);
        }
    }
}
