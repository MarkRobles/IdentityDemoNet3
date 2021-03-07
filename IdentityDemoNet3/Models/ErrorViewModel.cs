using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace IdentityDemoNet3.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public IEnumerable<IdentityError> Errores { get; set; }
    }
}
