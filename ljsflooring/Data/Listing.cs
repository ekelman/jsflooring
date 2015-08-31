using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ljsflooring.Data
{
    public class Listing
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Item title is required.")]
        public string title { get; set; }
         [Required(ErrorMessage = "Item description is required.")]
        public string description { get; set; }
        public string image { get; set; }
        public Nullable<int> displayorder { get; set; }
        public Nullable<int> ratings { get; set; }

        public int CategoryId { get; set; }
    }
}