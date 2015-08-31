using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;

namespace ljsflooring.Data
{
    class LjsflooringMigrationsConfiguration
        : DbMigrationsConfiguration<LjsflooringContext>
    {
        public LjsflooringMigrationsConfiguration()
        {
            this.AutomaticMigrationDataLossAllowed = true;
            this.AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(LjsflooringContext context)
        {
            base.Seed(context);

#if DEBUG
            if (context.Category.Count() == 0)
            {
                var category = new Category()
                {
                    categoryname = "Tile",
                    image = "Tile.jpg",
                    Listings = new List<Listing>()
                    {
                        new Listing()
                        {
                            title = "12X12 Tile",
                            description = "12x12 Tile Installation",
                            image = "12X12Tile.jpg"
                        },
                        new Listing()
                        {
                            title = "12X24 Tile",
                            description = "12x24 Tile Installation",
                            image = "12X24Tile.jpg"
                        },
                        new Listing()
                        {
                            title = "16X16 Tile",
                            description = "16x16 Tile Installation",
                            image = "16X16Tile.jpg"
                        },
                    }
                };

                context.Category.Add(category);

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                }
            }
#endif
        }
    }
}
