using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IdentityDemoNet3;
using IdentityDemoNet3.Models;
using IdentityDemoNet3.IRepositories;

namespace IdentityDemoNet3.Controllers
{
    public class GenresController : Controller
    {
        private readonly IGenreRepositorie _GenreRepositorie;

        public GenresController(IGenreRepositorie GenreRepositorie)
        {
            
            _GenreRepositorie = GenreRepositorie;
        }

        // GET: Genres
        public async Task<IActionResult> Index()
        {
            return View(await _GenreRepositorie.GetAll());
        }

        // GET: Genres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Genre = await _GenreRepositorie.GetById(id);
            if (Genre == null)
            {
                return NotFound();
            }

            return View(Genre);
        }

        // GET: Genres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Genre Genre)
        {
            if (ModelState.IsValid)
            {
               await _GenreRepositorie.Create(Genre);
       
                return RedirectToAction(nameof(Index));
            }
            return View(Genre);
        }

        // GET: Genres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Genre = await _GenreRepositorie.GetById(id);
            if (Genre == null)
            {
                return NotFound();
            }
            return View(Genre);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Genre Genre)
        {
            if (id != Genre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                  await  _GenreRepositorie.Update(Genre);
                
                }
                catch (DbUpdateConcurrencyException)
                {

                    var exist = await GenreExists(Genre.Id);
                    if (exist)
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
            return View(Genre);
        }

        // GET: Genres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Genre = await _GenreRepositorie.GetById(id);
            if (Genre == null)
            {
                return NotFound();
            }

            return View(Genre);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Genre = await _GenreRepositorie.GetById(id);
           await  _GenreRepositorie.Delete(Genre);

            return RedirectToAction(nameof(Index));
        }

        private  async Task<bool> GenreExists(int id)
        {
            return await _GenreRepositorie.Exists( id);
        }
    }
}
