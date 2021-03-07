using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemoNet3
{
    public class LoginViewModel
    {
        public string Descripcion { get; set; }
        [DataType(DataType.Password)]
        public string Contraseña { get; set; }
    }
}
