using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemoNet3.Models
{
    public class Genre
    {
        [Display(Name = "Genre")]

        public int Id { get; set; }

        [Display(Name = "Genre")]
        [Required]
        public string Name { get; set; }


    }
}
