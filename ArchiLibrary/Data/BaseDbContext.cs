using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchiLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ArchiLibrary.Data
{
    public abstract class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions options)
        : base(options)
        {
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeCreatedState();
            ChangeDeletedState();
            return base.SaveChangesAsync(cancellationToken);
        }
        private void ChangeCreatedState()
        {
            var createEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Added);
            foreach (var item in createEntities)
            {
                if (item.Entity is BaseModel model)
                {
                    model.Active = true;
                    model.CreatedAt = DateTime.Now;
                }
            }
        }
        private void ChangeDeletedState()
        {
            var deleteEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);
            foreach (var item in deleteEntities)
            {
                if (item.Entity is BaseModel model)
                {
                    model.Active = false;
                    model.DeletedAt = DateTime.UtcNow;
                    item.State = EntityState.Modified;
                }
            }
        }
    }
}
