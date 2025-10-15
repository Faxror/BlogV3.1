using DataAccessLayer.Concrete;
using DataAccessLayerr.Abstrack;
using EntityLayerr.Concrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayerr.Concrete
{
    public class BlogRepository : IBlogRepository
    {
        private readonly DBContext dBContext;

        public BlogRepository(DBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public Blogs addBlog(Blogs blog)
        {
            dBContext.Blogss.Add(blog);
            dBContext.SaveChanges();
            return blog;
        }

        public List<Author> GetAllAuthor()
        {
            return dBContext.Authors.ToList();
        }

        public List<AppUser> GetAllUser()
        {
            return dBContext.Users.ToList();
        }

        public Author GetAuthor()
        {
           return dBContext.Authors.FirstOrDefault();
        }

        public List<Blogs> getBlogAll()
        {
           return dBContext.Blogss.ToList();
        }

        public Blogs GetBlogs(int id)
        {
            return dBContext.Blogss.FirstOrDefault(b => b.Id == id);
        }

        public Blogs getBlogWithFirst()
        {
            return dBContext.Blogss.OrderByDescending(b => b.BlogTime).FirstOrDefault();
        }


        public Author getByAuthorİd(int id)
        {
            return dBContext.Authors.FirstOrDefault(a => a.AuthorID == id);

        }

        public void removeBlog(int id)
        {
            var deletedid = GetBlogs(id);
            if (deletedid != null)
            {
                dBContext.Blogss.Remove(deletedid);
                dBContext.SaveChanges();
            }
        }

        public Blogs UpdateBlog(Blogs blog)
        {
            dBContext.Blogss.Update(blog);
            dBContext.SaveChanges();
            return blog;
        }
    }
}
