using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayerr.Concrate
{
    public class Comment
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // foreign key
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        // navigation property
        public virtual AppUser User { get; set; }
    }

}
