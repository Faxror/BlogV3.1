using BusinesssLayer.Abstrack;
using DataAccessLayerr.Abstrack;
using EntityLayerr.Concrate;
using EntityLayerr.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinesssLayer.Concrete
{
    public class BlogManager : IBlogServices
    {
        private readonly IBlogRepository _blogRepository;

        public BlogManager(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public Blogs addBlog(Blogs blogs)
        {
            return _blogRepository.addBlog(blogs);
        }

        public List<Author> GetAllAuthor()
        {
             return _blogRepository.GetAllAuthor();
        }

        public List<AppUser> GetAllUser()
        {
            return _blogRepository.GetAllUser();
        }

        public Author GetAuthor()
        {
            return _blogRepository.GetAuthor();
        }

        public List<Blogs> getBlogAll()
        {
           return _blogRepository.getBlogAll();
        }

        public Blogs GetBlogs(int id)
        {
            return _blogRepository.GetBlogs(id);
        }

        public Blogs getBlogWithFirst()
        {
            return _blogRepository.getBlogWithFirst();
        }

        public Author getByAuthorİd(int id)
        {
           return _blogRepository.getByAuthorİd(id);
        }

        public List<PostStatisticsViewModel> GetMonthlyStatistics(DateTime startDate, DateTime endDate)
        {
            var blogs = _blogRepository.GetBlogsByDateRange(startDate, endDate);

            var stats = blogs
                .GroupBy(x => x.BlogTime.Month)
                .Select(g => new PostStatisticsViewModel
                {
                    Month = new DateTime(DateTime.Now.Year, g.Key, 1).ToString("MMMM", new CultureInfo("tr-TR")),
                    PostCount = g.Count()
                })
                .OrderBy(x => DateTime.ParseExact(x.Month, "MMMM", new CultureInfo("tr-TR")).Month)
                .ToList();

            return stats;

        }

        public void removeBlog(int id)
        {
            _blogRepository.removeBlog(id);
        }

        public Blogs UpdateBlog(Blogs blogs)
        {
           return _blogRepository.UpdateBlog(blogs);
        }
    }
}
