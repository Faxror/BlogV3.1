using BusinesssLayer.Abstrack;
using DataAccessLayerr.Abstrack;
using EntityLayerr.Concrate;
using System;
using System.Collections.Generic;
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
