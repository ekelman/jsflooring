using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ljsflooring.Data
{
    public class LjsflooringRepository : ILjsflooringRepository
    {
        LjsflooringContext _ctx;

        public LjsflooringRepository(LjsflooringContext ctx)
        {
            _ctx = ctx;
        }

        public IQueryable<Listing> GetListing()
        {
            return _ctx.Listing;
        }

        public IQueryable<Category> GetCategory(bool orderByName, bool orderById)
        {
            if (orderByName)
            {
                return _ctx.Category.OrderBy(c => c.categoryname);
            }

            if (orderById)
            {
                return _ctx.Category.OrderBy(c => c.id);
            }
            else
            {
                return _ctx.Category;
            }
            
        }

        public bool AddCategory(Category newCategory)
        {
            try
            {
                _ctx.Category.Add(newCategory);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AddListing(Listing newListing)
        {
            try
            {
                _ctx.Listing.Add(newListing);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Save()
        {
            try
            {
                return _ctx.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                // TODO log this error
                return false;
            }
        }


        public IQueryable<Category> GetCategoryByID(int categoryId)
        {
            return _ctx.Category.Where(r => r.id == categoryId);
        }

        public bool UpdateCategory(int categoryId, string categoryname, string image)
        {
            var category = _ctx.Category.Find(categoryId);
            category.categoryname = categoryname;
            if (image != null)
            {
                category.image = image;
            }
            return true;
        }

        public IQueryable<Listing> GetListingByCategoryId(int categroyId)
        {
            return _ctx.Listing.Where(l => l.CategoryId == categroyId).OrderByDescending(o => o.id);           
        }
    }
}