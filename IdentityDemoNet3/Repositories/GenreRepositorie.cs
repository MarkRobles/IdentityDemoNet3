using IdentityDemoNet3.IRepositories;
using IdentityDemoNet3.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemoNet3.Repositories
{
    public class GenreRepositorie:IGenreRepositorie
    {
        private readonly IdentityDemoUserDbContext _context;

        public GenreRepositorie(IdentityDemoUserDbContext context)
        {
            _context = context;

        }

        public Task<List<Genre>> GetAll()
        {
            return _context.Genres.ToListAsync();
        }

        public async Task<int> Create(Genre Genre)
        {
            await _context.AddAsync(Genre);
            var resultado = await _context.SaveChangesAsync();
            return resultado;
        }

        public async Task<int> Delete(Genre Genre)
        {
            _context.Remove(Genre);
            var resultado = await _context.SaveChangesAsync();
            return resultado;
        }

        public async Task<Genre> GetById(int? Id)
        {
            return await _context.Genres.FindAsync(Id);
        }

        public async Task<int> Update(Genre Genre)
        {
            _context.Update(Genre);
            var resultado = await _context.SaveChangesAsync();
            return resultado;
        }


        public async Task<bool> Exists(int id)
        {
            return await _context.Genres.AnyAsync(e => e.Id == id);
        }
    }
}
