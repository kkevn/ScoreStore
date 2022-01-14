using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScoreStore.Data;
using ScoreStore.Models;

namespace ScoreStore.Controllers
{
    public class ScoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Scores
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Scores.Include(s => s.Game).Include(s => s.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Scores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scores = await _context.Scores
                .Include(s => s.Game)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scores == null)
            {
                return NotFound();
            }

            return View(scores);
        }

        // GET: Scores/Create
        public IActionResult Create()
        {
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Scores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Wins,Losses,UserId,GameId")] Scores scores)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scores);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Id", scores.GameId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", scores.UserId);
            return View(scores);
        }

        // GET: Scores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scores = await _context.Scores.FindAsync(id);
            if (scores == null)
            {
                return NotFound();
            }
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Id", scores.GameId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", scores.UserId);
            return View(scores);
        }

        // POST: Scores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Wins,Losses,UserId,GameId")] Scores scores)
        {
            if (id != scores.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scores);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScoresExists(scores.Id))
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
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Id", scores.GameId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", scores.UserId);
            return View(scores);
        }

        // GET: Scores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scores = await _context.Scores
                .Include(s => s.Game)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scores == null)
            {
                return NotFound();
            }

            return View(scores);
        }

        // POST: Scores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scores = await _context.Scores.FindAsync(id);
            _context.Scores.Remove(scores);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScoresExists(int id)
        {
            return _context.Scores.Any(e => e.Id == id);
        }
    }
}
