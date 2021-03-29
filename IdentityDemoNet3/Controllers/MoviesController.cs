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
using System.Data.OleDb;
using System.Data;
using Spire.Xls;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace IdentityDemoNet3.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieRepositorie _movieRepositorie;
        private readonly IGenreRepositorie _genreRepositorie;
        private readonly IConfiguration _configuration;

        public MoviesController(IMovieRepositorie movieRepositorie,IGenreRepositorie genreRepositorie,IConfiguration  configuration)
        {
            
            _movieRepositorie = movieRepositorie;
            _genreRepositorie = genreRepositorie;
            _configuration = configuration;
        }


        public async Task<IActionResult> Exportar(int? id) {

            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieRepositorie.GetById(id);
            if (movie == null)
            {
                return NotFound();
            }
            //A: Dynamically create Excel file and save it to stream
            Workbook wbToStream = new Workbook();
            Worksheet sheet = wbToStream.Worksheets[0];
            string fileName = string.Concat(movie.Description, ".xls");
            sheet.Range["A1"].Text = "Titulo";
            sheet.Range["A2"].Text = "Fecha de lanzamiento";
            sheet.Range["A3"].Text = "Reseña";
            sheet.Range["A4"].Text = "Precio";
            sheet.Range["A5"].Text = "Genero";
            sheet.Range["A6"].Text = "PreVenta";
            sheet.Range["B1"].Text = movie.Title;
            sheet.Range["B2"].Text = movie.ReleaseDate.ToShortDateString();
            sheet.Range["B3"].Text = movie.Description;
            sheet.Range["B4"].Text = movie.Price.ToString("C2");
            sheet.Range["B5"].Text = movie.Genre.Name;
            sheet.Range["B6"].Text = movie.Preorder==true?"SI":"NO";
            FileStream file_stream = new FileStream(fileName, FileMode.Create);
            wbToStream.SaveToStream(file_stream);
            file_stream.Close();
            //System.Diagnostics.Process.Start(fileName);

            return RedirectToAction("Index");

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
