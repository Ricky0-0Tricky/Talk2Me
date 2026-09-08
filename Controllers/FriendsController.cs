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
    public class FriendsController : Controller
    {
        private readonly AppDbContext _context;

        public FriendsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Friends
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Friends.Include(f => f.FriendUser).Include(f => f.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Friends/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friend = await _context.Friends
                .Include(f => f.FriendUser)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.FriendId == id);
            if (friend == null)
            {
                return NotFound();
            }

            return View(friend);
        }

        // GET: Friends/Create
        public IActionResult Create()
        {
            ViewData["FriendUserId"] = new SelectList(_context.Users, "UserId", "PasswordHash");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "PasswordHash");
            return View();
        }

        // POST: Friends/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FriendId,IsFavorite,IsBlocked,BlockedDate,UserId,FriendUserId")] Friend friend)
        {
            if (ModelState.IsValid)
            {
                friend.FriendId = Guid.NewGuid();
                _context.Add(friend);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FriendUserId"] = new SelectList(_context.Users, "UserId", "PasswordHash", friend.FriendUserId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "PasswordHash", friend.UserId);
            return View(friend);
        }

        // GET: Friends/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friend = await _context.Friends.FindAsync(id);
            if (friend == null)
            {
                return NotFound();
            }
            ViewData["FriendUserId"] = new SelectList(_context.Users, "UserId", "PasswordHash", friend.FriendUserId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "PasswordHash", friend.UserId);
            return View(friend);
        }

        // POST: Friends/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("FriendId,IsFavorite,IsBlocked,BlockedDate,UserId,FriendUserId")] Friend friend)
        {
            if (id != friend.FriendId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(friend);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FriendExists(friend.FriendId))
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
            ViewData["FriendUserId"] = new SelectList(_context.Users, "UserId", "PasswordHash", friend.FriendUserId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "PasswordHash", friend.UserId);
            return View(friend);
        }

        // GET: Friends/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friend = await _context.Friends
                .Include(f => f.FriendUser)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.FriendId == id);
            if (friend == null)
            {
                return NotFound();
            }

            return View(friend);
        }

        // POST: Friends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var friend = await _context.Friends.FindAsync(id);
            if (friend != null)
            {
                _context.Friends.Remove(friend);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FriendExists(Guid id)
        {
            return _context.Friends.Any(e => e.FriendId == id);
        }
    }
}
