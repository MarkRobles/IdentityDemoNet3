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
    public class MoviesController : Controller
    {
        private readonly IMovieRepositorie _movieRepositorie;

        public MoviesController(IMovieRepositorie movieRepositorie)
        {
            
            _movieRepositorie = movieRepositorie;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return View(await _movieRepositorie.GetAll());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieRepositorie.GetById(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Description,Price,Preorder")] Movie movie)
        {
            if (ModelState.IsValid)
            {
               await _movieRepositorie.Create(movie);
       
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieRepositorie.GetById(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Description,Price,Preorder")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                  await  _movieRepositorie.Update(movie);
                
                }
                catch (DbUpdateConcurrencyException)
                {

                    var exist = await MovieExists(movie.Id);
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
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieRepositorie.GetById(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _movieRepositorie.GetById(id);
           await  _movieRepositorie.Delete(movie);

            return RedirectToAction(nameof(Index));
        }

        private  async Task<bool> MovieExists(int id)
        {
            return await _movieRepositorie.Exists( id);
        }
    }
}
