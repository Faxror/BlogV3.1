using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayerr.Concrate
{
    public class Author
    {
        public int AuthorID { get; set; }
        public string AuthorName { get; set; }
        public string AuthorImage { get; set; }

        public string Mail { get; set; }


        public string AuthorAbout { get; set; }

        public string Phone { get; set; }
        public string ShortAbout { get; set; }

        public ICollection<Blogs> Blogss { get; set; }
    }
}
