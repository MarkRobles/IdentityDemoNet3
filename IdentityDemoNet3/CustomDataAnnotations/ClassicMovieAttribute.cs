using IdentityDemoNet3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemoNet3.CustomDataAnnotations
{
    public class ClassicMovieAttribute: ValidationAttribute
    {
        public ClassicMovieAttribute(int year)
        {
            Year = year;
        }

        public int Year { get; }

        public string GetErrorMessage() =>
            $"Classic movies must have a release year no later than {Year}.";

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            //var movie = (Movie)validationContext.ObjectInstance;



            var releaseYear = ((DateTime)value).Year;

            if ( releaseYear > Year)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}
