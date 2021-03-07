using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemoNet3
{
    public class RegistroViewModel
    {
        [Display(Name = "Usario")]
        public string Descripcion { get; set; }

        [DataType(DataType.Password)]
        public string Contraseña { get; set; }

        [Compare("Contraseña")]
        [DataType(DataType.Password)]
        public string ConfirmarContraseña { get; set; }
    }
}
