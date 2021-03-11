﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemoNet3
{
    public class Usuario:IdentityUser
    {
        public string Locale { get; set; } = "es-MX";


        public string  OrgId { get; set; }


        public class Organization {
            public string Id { get; set; }
            public string Name { get; set; }
        }
    }
}
