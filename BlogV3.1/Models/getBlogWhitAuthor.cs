using EntityLayerr.Concrate;

namespace BlogV3._1.Models
{
    public class getBlogWhitAuthor
    {
        public EntityLayerr.Concrate.Blogs Blog { get; set; }

        public EntityLayerr.Concrate.Author Author { get; set; }
        public IEnumerable<Comment> Comments { get; set; } // 🔹 Burada liste tipi önemli

    }
}
