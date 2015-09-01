using ljsflooring.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace ljsflooring.Controllers
{
    public class HomeController : Controller
    {
        private ILjsflooringRepository _repo;

        public HomeController(ILjsflooringRepository repo)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {
            var categories = _repo.GetCategory(false, true).ToList();
            return View(categories);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory([Bind(Include = "categoryname,image")] Category category)
        {
            try 
            {
                if (ModelState.IsValid && Request.Files[0].FileName != String.Empty)
                {
                    SetImmageFile setImageFile = new SetImmageFile();
                    category.image = setImageFile.ProcessImageFile(category.image, this.Request, this.Server, this.HttpContext);
                    _repo.AddCategory(category);
                    _repo.Save();
                    return RedirectToAction("Index");
                }
                if (Request.Files[0].FileName == String.Empty)
                {
                    ModelState.AddModelError("ErrorImage", "Image is required.");
                    return View();
                }
            }
            catch
            {
                return View();
            }
            return View();
        }

        public ActionResult EditCategory(int categoryid)
        {
            var category = _repo.GetCategoryByID(categoryid).ToList();
            return View(category);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory(IEnumerable<Category> category)
        {
            if (ModelState.IsValid)
            {
                int categoryid=0;
                string categoryname="";
                string image="";

                foreach(Category categorylist in category)
                {
                    if (Request.Files[0].FileName != String.Empty)
                    {
                        SetImmageFile setImageFile = new SetImmageFile();
                        image = setImageFile.ProcessImageFile(categorylist.image, this.Request, this.Server, this.HttpContext);
                    }
                    else
                    {
                        image = categorylist.image;
                    }
                    categoryid = categorylist.id;
                    categoryname = categorylist.categoryname;
                }
                _repo.UpdateCategory(categoryid, categoryname, image);
                _repo.Save();
            }
            return RedirectToAction("Index");
        }

        public ActionResult ConfirmDeleteCategory(int categoryid)
        {
            ViewBag.categoryid = categoryid;
            return PartialView("_ConfirmDeleteLCategory");
        }

        public ActionResult DeleteCategory(int categoryid)
        {
            try
            {
                var category = _repo.GetCategoryByID(categoryid).ToList();

                _repo.RemoveCategory(categoryid);
                _repo.Save();

                SetImmageFile setImageFile = new SetImmageFile();
                setImageFile.DeleteImageFile(category[0].image, this.HttpContext);

                return Json(new { success = true });
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult AddListing()
        {
            var categories = _repo.GetCategory(true, false);
            ViewBag.category_id_list = new SelectList(categories, "id", "categoryname");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddListing([Bind(Include = "title,description,image,CategoryId")] Listing listing)
        {
            try
            {
                if (ModelState.IsValid && Request.Files[0].FileName != String.Empty)
                {
                    SetImmageFile setImageFile = new SetImmageFile();
                    listing.image = setImageFile.ProcessImageFile(listing.image, this.Request, this.Server, this.HttpContext);
                    _repo.AddListing(listing);
                    _repo.Save();
                    return RedirectToAction("Index");
                }
                if (Request.Files[0].FileName == String.Empty)
                {
                    var categories = _repo.GetCategory(true, false);
                    ViewBag.category_id_list = new SelectList(categories, "id", "categoryname");
                    ModelState.AddModelError("ErrorImage", "Image is required.");
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View();
            }
            return View();
        }

        public ActionResult GetCategoryListings(int categoryid, string categoryname)
        {
            if (categoryid != null)
            {
                var listings = _repo.GetListingByCategoryId(categoryid).ToList();
                ViewBag.CategoryName = categoryname;
                return View(listings);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult GetListigById(int listingid)
        {
            var listings = _repo.GetListingById(listingid).ToList();
            return PartialView("_ListingDetails",listings);
        }

        public ActionResult EditListing(int listingid, string categoryname)
        {
            var listing = _repo.GetListingById(listingid).ToList();
            ViewBag.CategoryName = categoryname;
            var categories = _repo.GetCategory(true, false);
            ViewBag.category_id_list = new SelectList(categories, "id", "categoryname",listing[0].CategoryId);

            return View(listing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditListing(IEnumerable<Listing> listing, string categoryname)
        {
            var categories = _repo.GetCategory(true, false);
            ViewBag.category_id_list = new SelectList(categories, "id", "categoryname");

            int categoryid = 0;
            string listingtitle = "";
            string listingdescription = "";
            int listingid = 0;
            string image = "";

            if (ModelState.IsValid)
            {
                foreach (Listing item in listing)
                {
                    if (Request.Files[0].FileName != String.Empty)
                    {
                        SetImmageFile setImageFile = new SetImmageFile();
                        image = setImageFile.ProcessImageFile(item.image, this.Request, this.Server, this.HttpContext);
                    }
                    else
                    {
                        image = item.image;
                    }
                    categoryid = item.CategoryId;
                    listingtitle = item.title;
                    listingdescription = item.description;
                    listingid = item.id;
                }
                _repo.UpdateListing(listingid, categoryid, listingtitle, listingdescription, image);
                _repo.Save();
            }
            return RedirectToAction("GetCategoryListings", new { categoryid = categoryid, categoryname = categoryname });
        }

        public ActionResult ConfirmDelete(int listingid, string categoryname, int categoryid) 
        {
            ViewBag.listingid = listingid;
            ViewBag.categoryname = categoryname;
            ViewBag.categoryid = categoryid;
            return PartialView("_ConfirmDeleteListing");
        }

        public ActionResult DeleteListing(int listingid, int categoryid, string categoryname)
        {
            try
            {
                var listing = _repo.GetListingById(listingid).ToList();

                _repo.RemoveListing(listingid);
                _repo.Save();

                SetImmageFile setImageFile = new SetImmageFile();
                setImageFile.DeleteImageFile(listing[0].image, this.HttpContext);

                return Json(new { success = true });
            }
            catch
            {
                return RedirectToAction("GetCategoryListings", new { categoryid = categoryid, categoryname = categoryname });
            }
        }
    }
}