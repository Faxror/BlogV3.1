using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayerr.Concrate
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }

        public int ConfirimCode { get; set; }

    }
}
