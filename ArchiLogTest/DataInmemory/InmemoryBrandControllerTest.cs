using ArchiLog.Data;
using ArchiLog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ArchiLogTest.DataInmemory
{
    public class InmemoryBrandControllerTest
    {
        private readonly DbContextOptions<ArchiLogDbContext> _contextOptions;
        public InmemoryBrandControllerTest()
        {
            _contextOptions = new DbContextOptionsBuilder<ArchiLogDbContext>()
            .UseInMemoryDatabase("BrandControllerTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

            using var context = new ArchiLogDbContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.AddRange(
            new Brand { ID = 1, Active = true, CreatedAt = DateTime.Now, DeletedAt = DateTime.Now, Name = "brand1", Slogan = "slogan1" },
            new Brand { ID = 2, Active = true, CreatedAt = DateTime.Now, DeletedAt = DateTime.Now, Name = "brand2", Slogan = "slogan2" });

            context.SaveChanges();
        }
    }
}
