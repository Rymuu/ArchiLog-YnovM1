using ArchiLibrary.Controllers;
using ArchiLibrary.Data;
using ArchiLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ArchiLibraryTest
{
    public class ControllersTest<TContext, TModel>  where TContext : BaseDbContext where TModel : BaseModel
    {
        /*public readonly DbContextOptions<BaseDbContext> _dbContextOptions;
        public ControllersTest()
        {
            _dbContextOptions = new DbContextOptionsBuilder<BaseDbContext>()
                .UseInMemoryDatabase(databaseName: "archilog")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var _context = new BaseDbContext(_dbContextOptions);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var datas = new List<BaseModel>
            {
                new BaseModel { Name = "test"},
                new BaseModel { Name = "test1"}
            };
            _context.AddRange(datas);
            _context.SaveChanges();
        }  

     
        [Fact]
        public async void TestPost()
        {
            var context= new BaseDbContext(_dbContextOptions);
            BaseController<BaseDbContext, BaseModel> baseController = new BaseController<BaseDbContext, BaseModel>(context);
            await baseController.PostItem(new BaseModel { Name = "test2" });
            //Assert.Equal(3, await context.Set<TModel>.count());
        }*/
    }
}