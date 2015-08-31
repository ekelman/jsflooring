using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ljsflooring.Data
{
    public class Category
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Category name is required.")]
        public string categoryname { get; set; }
        public string image { get; set; }

        public ICollection<Listing> Listings { get; set; }
    }
}