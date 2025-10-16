using EntityLayerr.Concrate;
using EntityLayerr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinesssLayer.Abstrack
{
    public interface IBlogServices
    {
        Blogs GetBlogs(int id);

        List<Blogs> getBlogAll();

        Blogs addBlog(Blogs blogs);

        void removeBlog(int id);

        Blogs UpdateBlog(Blogs blogs);

        Blogs getBlogWithFirst();

        Author getByAuthorİd(int id);

       Author GetAuthor();

       List<Author> GetAllAuthor();

       List<AppUser> GetAllUser();

        List<PostStatisticsViewModel> GetMonthlyStatistics(DateTime startDate, DateTime endDate);

    }
}
