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
    public class CategoryManager : ICategoryServices
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryManager(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Category CategoryAdd(Category category)
        {
            return _categoryRepository.CategoryAdd(category);
        }

        public void CategoryDelete(int id)
        {
            _categoryRepository.CategoryDelete(id);
        }

        public Category CategoryUpdate(Category category)
        {
          return _categoryRepository.CategoryUpdate(category);
        }

        public Category GetById(int id)
        {
          return _categoryRepository.GetById(id);
        }
        public List<Category> GetCategories()
        {
            return _categoryRepository.GetCategories();
        }
    }
}
