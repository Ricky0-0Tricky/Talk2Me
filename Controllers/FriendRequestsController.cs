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
    public class FriendRequestsController : Controller
    {
        private readonly AppDbContext _context;

        public FriendRequestsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: FriendRequests
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.FriendRequests.Include(f => f.Receiver).Include(f => f.Sender);
            return View(await appDbContext.ToListAsync());
        }

        // GET: FriendRequests/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friendRequest = await _context.FriendRequests
                .Include(f => f.Receiver)
                .Include(f => f.Sender)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (friendRequest == null)
            {
                return NotFound();
            }

            return View(friendRequest);
        }

        // GET: FriendRequests/Create
        public IActionResult Create()
        {
            ViewData["ReceiverId"] = new SelectList(_context.Users, "UserId", "PasswordHash");
            ViewData["SenderId"] = new SelectList(_context.Users, "UserId", "PasswordHash");
            return View();
        }

        // POST: FriendRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestId,Status,Message,RequestDate,SenderId,ReceiverId")] FriendRequest friendRequest)
        {
            if (ModelState.IsValid)
            {
                friendRequest.RequestId = Guid.NewGuid();
                _context.Add(friendRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ReceiverId"] = new SelectList(_context.Users, "UserId", "PasswordHash", friendRequest.ReceiverId);
            ViewData["SenderId"] = new SelectList(_context.Users, "UserId", "PasswordHash", friendRequest.SenderId);
            return View(friendRequest);
        }

        // GET: FriendRequests/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friendRequest = await _context.FriendRequests.FindAsync(id);
            if (friendRequest == null)
            {
                return NotFound();
            }
            ViewData["ReceiverId"] = new SelectList(_context.Users, "UserId", "PasswordHash", friendRequest.ReceiverId);
            ViewData["SenderId"] = new SelectList(_context.Users, "UserId", "PasswordHash", friendRequest.SenderId);
            return View(friendRequest);
        }

        // POST: FriendRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("RequestId,Status,Message,RequestDate,SenderId,ReceiverId")] FriendRequest friendRequest)
        {
            if (id != friendRequest.RequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(friendRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FriendRequestExists(friendRequest.RequestId))
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
            ViewData["ReceiverId"] = new SelectList(_context.Users, "UserId", "PasswordHash", friendRequest.ReceiverId);
            ViewData["SenderId"] = new SelectList(_context.Users, "UserId", "PasswordHash", friendRequest.SenderId);
            return View(friendRequest);
        }

        // GET: FriendRequests/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friendRequest = await _context.FriendRequests
                .Include(f => f.Receiver)
                .Include(f => f.Sender)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (friendRequest == null)
            {
                return NotFound();
            }

            return View(friendRequest);
        }

        // POST: FriendRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var friendRequest = await _context.FriendRequests.FindAsync(id);
            if (friendRequest != null)
            {
                _context.FriendRequests.Remove(friendRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FriendRequestExists(Guid id)
        {
            return _context.FriendRequests.Any(e => e.RequestId == id);
        }
    }
}
