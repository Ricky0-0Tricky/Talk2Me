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
    public class TicketPhotosController : Controller
    {
        private readonly AppDbContext _context;

        public TicketPhotosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TicketPhotos
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.TicketPhotos.Include(t => t.Ticket);
            return View(await appDbContext.ToListAsync());
        }

        // GET: TicketPhotos/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketPhoto = await _context.TicketPhotos
                .Include(t => t.Ticket)
                .FirstOrDefaultAsync(m => m.TicketPhotoId == id);
            if (ticketPhoto == null)
            {
                return NotFound();
            }

            return View(ticketPhoto);
        }

        // GET: TicketPhotos/Create
        public IActionResult Create()
        {
            ViewData["TicketId"] = new SelectList(_context.Tickets, "TicketId", "Description");
            return View();
        }

        // POST: TicketPhotos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TicketPhotoId,Image,TicketId")] TicketPhoto ticketPhoto)
        {
            if (ModelState.IsValid)
            {
                ticketPhoto.TicketPhotoId = Guid.NewGuid();
                _context.Add(ticketPhoto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TicketId"] = new SelectList(_context.Tickets, "TicketId", "Description", ticketPhoto.TicketId);
            return View(ticketPhoto);
        }

        // GET: TicketPhotos/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketPhoto = await _context.TicketPhotos.FindAsync(id);
            if (ticketPhoto == null)
            {
                return NotFound();
            }
            ViewData["TicketId"] = new SelectList(_context.Tickets, "TicketId", "Description", ticketPhoto.TicketId);
            return View(ticketPhoto);
        }

        // POST: TicketPhotos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TicketPhotoId,Image,TicketId")] TicketPhoto ticketPhoto)
        {
            if (id != ticketPhoto.TicketPhotoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketPhoto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketPhotoExists(ticketPhoto.TicketPhotoId))
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
            ViewData["TicketId"] = new SelectList(_context.Tickets, "TicketId", "Description", ticketPhoto.TicketId);
            return View(ticketPhoto);
        }

        // GET: TicketPhotos/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketPhoto = await _context.TicketPhotos
                .Include(t => t.Ticket)
                .FirstOrDefaultAsync(m => m.TicketPhotoId == id);
            if (ticketPhoto == null)
            {
                return NotFound();
            }

            return View(ticketPhoto);
        }

        // POST: TicketPhotos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var ticketPhoto = await _context.TicketPhotos.FindAsync(id);
            if (ticketPhoto != null)
            {
                _context.TicketPhotos.Remove(ticketPhoto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketPhotoExists(Guid id)
        {
            return _context.TicketPhotos.Any(e => e.TicketPhotoId == id);
        }
    }
}
