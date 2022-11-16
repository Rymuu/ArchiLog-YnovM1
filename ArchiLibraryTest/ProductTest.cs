using ArchiLibrary.Controllers;
using ArchiLibrary.Data;
using ArchiLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ArchiLibraryTest
{
    public class ProductTest
    {
        public readonly DbContextOptions<BaseDbContext> _dbContextOptions;
        public ProductTest()
        {
            _dbContextOptions = new DbContextOptionsBuilder<BaseDbContext>()
                .UseInMemoryDatabase(databaseName: "archilog")
                .Options;

            using var _context = new BaseDbContext(_dbContextOptions);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var datas = new List<Product>
            {
                new Product { Name = "test"},
                new Product { Name = "test1"}
            };
            _context.AddRange(datas);
            _context.SaveChanges();
        }  

     
        [Fact]
        public async void TestPost()
        {
            var context= new BaseDbContext(_dbContextOptions);
            ProductController productController = new ProductController(context);
            await productController.PostItem(new Product { Name = "test2" });
            Assert.Equal(3, await context.Products.CountAsync());
        }
    }
}