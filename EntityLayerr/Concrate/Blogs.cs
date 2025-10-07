using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayerr.Concrate
{
    public class Blogs
    {
        public int Id { get; set; }

        public string BlogTitle { get; set; }
        public string BlogDescription { get; set; }
        
        public string BlogImage { get; set; }

        public DateTime BlogTime { get; set; }

        public int AuthorID { get; set; }

        public Author Author { get; set; }

    }
}
