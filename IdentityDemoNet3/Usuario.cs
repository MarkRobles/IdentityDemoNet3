﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemoNet3
{
    public class Usuario
    {
        public string Id { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionNormalizada { get; set; }
        public string ContraseñaHash { get; set; }
    }
}
