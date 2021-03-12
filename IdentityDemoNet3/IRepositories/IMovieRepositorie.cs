using IdentityDemoNet3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemoNet3.IRepositories
{
    public interface IMovieRepositorie
    {
        Task<int> Create(Movie Movie);
        Task<List<Movie>> GetAll();
        Task<int> Update(Movie Movie);
        Task<int> Delete(Movie Movie);
        Task<Movie> GetById(int? CategoriaId);
        Task<bool> Exists(int id);
    }
}
