using EntityLayerr.Concrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayerr.Abstrack
{
    public interface IBlogRepository
    {
        Blogs GetBlogs(int id);

        List<Blogs> getBlogAll();

        Blogs addBlog(Blogs blog);

        void removeBlog(int id);

        Blogs UpdateBlog(Blogs blog);

        Blogs getBlogWithFirst();

        Author getByAuthorİd(int id);

        Author GetAuthor();

        List<Author> GetAllAuthor();

        List<AppUser> GetAllUser();


        List<Blogs> GetBlogsByDateRange(DateTime startDate, DateTime endDate);
    }
}
