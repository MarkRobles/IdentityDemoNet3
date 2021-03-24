﻿using IdentityDemoNet3.CustomDataAnnotations;
using IdentityDemoNet3.IRepositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemoNet3.Models
{
    public class Movie : IValidatableObject
    {

        private const int _classicYear = 1960;
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

       // [ClassicMovie(1960)] //This only works for Movie Type
        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Range(0, 999.99)]
        public decimal Price { get; set; }

        public int GenreId { get; set; }
        
        public Genre Genre { get; set; }

        public bool Preorder { get; set; }

        //This works for Class-LevelValidation
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext )
        {
            var movie = (Movie)validationContext.ObjectInstance; 
          
            if (movie.GenreId == 4 && ReleaseDate.Year > _classicYear)
            {
                yield return new ValidationResult(
                    $"Classic movies must have a release year no later than {_classicYear}.",
                    new[] { nameof(ReleaseDate) });
            }
        }
    }
}
