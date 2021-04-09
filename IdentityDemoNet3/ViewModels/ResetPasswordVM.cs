using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemoNet3.ViewModels
{
    public class ResetPasswordVM
    {
        public string Token { get; set; }
        public string Correo { get; set; }

        [DataType(DataType.Password)]
        public string Contraseña { get; set; }
        [DataType(DataType.Password)]
        [Compare("Contraseña")]
        public string ConfirmarContraseña { get; set; }
    }
}
