using EntityLayerr.Concrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayerr.Abstrack
{
    public interface ICategoryRepository
    {
        List<Category> GetCategories();
        Category CategoryAdd(Category category);

        Category GetById(int id);

        void CategoryDelete(int id);

        Category CategoryUpdate(Category category);


    }
}
