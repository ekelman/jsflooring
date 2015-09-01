using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ljsflooring.Data
{
    public interface ILjsflooringRepository
    {
        IQueryable<Listing> GetListing();
        IQueryable<Category> GetCategory(bool orderByName, bool orderById);
        IQueryable<Category> GetCategoryByID(int categoryId);
        IQueryable<Listing> GetListingByCategoryId(int categroyId);
        IQueryable<Listing> GetListingById(int listingId);
        bool AddCategory(Category newCategory);
        bool AddListing(Listing newListing);
        bool Save();
        bool UpdateCategory(int categoryId, string categoryname, string image);
        bool UpdateListing(int id, int categoryId, string title, string description, string image);
        bool RemoveListing(int id);
        bool RemoveCategory(int id);
    }
}
