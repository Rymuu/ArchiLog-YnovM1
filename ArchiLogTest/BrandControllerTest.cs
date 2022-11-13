using Archilog.Controllers;
using ArchiLog.Data;
using ArchiLog.Models;
using Microsoft.EntityFrameworkCore;

namespace ArchiLogTest
{
    public class BrandControllerTest
    {
        private readonly DbContextOptions<ArchiLogDbContext> _contextOptions;

        [Fact]
        public void TestGetAll()
        {
            using var context = CreateContext();
            var controller = new BrandsController(context);
            Brand brand = new Brand { Name = "brand3", Slogan = "slogan3" };
            controller.PostItem(brand);
            var brand1 = context.Brands.Single(b => b.Name == "brand3");
            Assert.Equal("brand3", brand1.Name);


        }
        ArchiLogDbContext CreateContext() => new ArchiLogDbContext(_contextOptions, (context, modelBuilder) =>
        {
            modelBuilder.Entity<Brand>()
                .ToInMemoryQuery(() => context.Brands.Select(b => new Brand { Name = b.Name }));
        });
    }
}