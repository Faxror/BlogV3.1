using EntityLayerr.Concrate;

namespace BlogV3._1.Models
{
    public class CommentViewModel
    {
        public Comment NewComment { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
    }
}
