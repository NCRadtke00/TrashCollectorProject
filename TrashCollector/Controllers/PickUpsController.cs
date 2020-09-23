﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrashCollector.Data;
using TrashCollector.Models;

namespace TrashCollector.Controllers
{
    public class PickUpsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PickUpsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PickUps
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PickUps.Include(p => p.employees);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PickUps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pickUp = await _context.PickUps
                .Include(p => p.employees)
                .FirstOrDefaultAsync(m => m.PickUpId == id);
            if (pickUp == null)
            {
                return NotFound();
            }

            return View(pickUp);
        }

        // GET: PickUps/Create
        public IActionResult Create()
        {
            ViewData["employeesId"] = new SelectList(_context.Employees, "Id", "Id");
            return View();
        }

        // POST: PickUps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PickUpId,dayOfPickUp,PickUpDay,customerTrashWasNotCollected,customerRecyclingWasNotCollected,discontinuePickUps,pausePickUps,bill,customersId,employeesId")] PickUp pickUp)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pickUp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["employeesId"] = new SelectList(_context.Employees, "Id", "Id", pickUp.employeesId);
            return View(pickUp);
        }

        // GET: PickUps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pickUp = await _context.PickUps.FindAsync(id);
            if (pickUp == null)
            {
                return NotFound();
            }
            ViewData["employeesId"] = new SelectList(_context.Employees, "Id", "Id", pickUp.employeesId);
            return View(pickUp);
        }

        // POST: PickUps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PickUpId,dayOfPickUp,PickUpDay,customerTrashWasNotCollected,customerRecyclingWasNotCollected,discontinuePickUps,pausePickUps,bill,customersId,employeesId")] PickUp pickUp)
        {
            if (id != pickUp.PickUpId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pickUp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PickUpExists(pickUp.PickUpId))
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
            ViewData["employeesId"] = new SelectList(_context.Employees, "Id", "Id", pickUp.employeesId);
            return View(pickUp);
        }

        // GET: PickUps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pickUp = await _context.PickUps
                .Include(p => p.employees)
                .FirstOrDefaultAsync(m => m.PickUpId == id);
            if (pickUp == null)
            {
                return NotFound();
            }

            return View(pickUp);
        }

        // POST: PickUps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pickUp = await _context.PickUps.FindAsync(id);
            _context.PickUps.Remove(pickUp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PickUpExists(int id)
        {
            return _context.PickUps.Any(e => e.PickUpId == id);
        }
    }
}
