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
using IdentityDemoNet3.ViewModels;

namespace IdentityDemoNet3.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieRepositorie _movieRepositorie;
        private readonly IGenreRepositorie _genreRepositorie;

        public MoviesController(IMovieRepositorie movieRepositorie,IGenreRepositorie genreRepositorie)
        {
            
            _movieRepositorie = movieRepositorie;
            _genreRepositorie = genreRepositorie;
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
        public async Task<IActionResult> Create()
        {

            var viewmodel = new MoviesViewModel
            {
                Genres = await _genreRepositorie.DropdownGenres()
            };
        
            return View(viewmodel);
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MoviesViewModel viewModel)
        {
        var genre =   await _genreRepositorie.GetById(viewModel.Movie.GenreId);
            viewModel.Movie.Genre = genre;

            if (ModelState.IsValid)
            {
               await _movieRepositorie.Create(viewModel.Movie);
       
                return RedirectToAction(nameof(Index)); 
            }

            var viewmodel = new MoviesViewModel
            {
                Genres = await _genreRepositorie.DropdownGenres(),
                Movie = viewModel.Movie
            };


            return View(viewModel);
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

            var viewmodel = new MoviesViewModel
            {
                Movie = movie,
                Genres = await _genreRepositorie.DropdownGenres()
            };

            return View(viewmodel);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MoviesViewModel viewModel)
        {
            if (id != viewModel.Movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                  await  _movieRepositorie.Update(viewModel.Movie);
                
                }
                catch (DbUpdateConcurrencyException)
                {

                    var exist = await MovieExists(viewModel.Movie.Id);
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

            var viewmodel = new MoviesViewModel
            {
                Movie = viewModel.Movie,
                Genres = await _genreRepositorie.DropdownGenres()
            };


            return View(viewModel);
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
