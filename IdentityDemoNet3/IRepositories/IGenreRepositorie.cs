using IdentityDemoNet3.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemoNet3.IRepositories
{
    public interface IGenreRepositorie
    {
        Task<int> Create(Genre Genre);
        Task<List<Genre>> GetAll();
        Task<int> Update(Genre Genre);
        Task<int> Delete(Genre Genre);
        Task<Genre> GetById(int? CategoriaId);
        Task<bool> Exists(int id);
        Task<List<SelectListItem>> DropdownGenres();
    }
}
