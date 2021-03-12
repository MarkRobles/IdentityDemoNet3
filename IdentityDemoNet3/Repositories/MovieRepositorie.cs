using IdentityDemoNet3.IRepositories;
using IdentityDemoNet3.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemoNet3.Repositories
{
    public class MovieRepositorie:IMovieRepositorie
    {
        private readonly IdentityDemoUserDbContext _context;

        public MovieRepositorie(IdentityDemoUserDbContext context)
        {
            _context = context;

        }

        public Task<List<Movie>> GetAll()
        {
            return _context.Movies.Include(g=> g.Genre).ToListAsync();
        }

        public async Task<int> Create(Movie Movie)
        {
            await _context.AddAsync(Movie);
            var resultado = await _context.SaveChangesAsync();
            return resultado;
        }

        public async Task<int> Delete(Movie Movie)
        {
            _context.Remove(Movie);
            var resultado = await _context.SaveChangesAsync();
            return resultado;
        }

        public async Task<Movie> GetById(int? Id)
        {
            return await _context.Movies.Include(G=> G.Genre).FirstOrDefaultAsync(G=> G.Id ==Id);
        }

        public async Task<int> Update(Movie Movie)
        {
            _context.Update(Movie);
            var resultado = await _context.SaveChangesAsync();
            return resultado;
        }


        public async Task<bool> Exists(int id)
        {
            return await _context.Movies.AnyAsync(e => e.Id == id);
        }
    }
}
