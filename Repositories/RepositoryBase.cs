using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortfolioServer.Api.Entity;

namespace PortfolioServer.Api.Repository
{   
    public abstract class RepositoryBase<TContext, TEntity> 
        where TContext : DbContext
        where TEntity : EntityBase
    {
        public RepositoryBase(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected TContext Context { get; private set; }
        protected DbSet<TEntity> Entities { get => Context.Set<TEntity>(); }

        protected async Task CreateEntityAsync(TEntity entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            Entities.Add(entity);
            await Context.SaveChangesAsync();
        }

        protected async Task UpdateEntityAsync(TEntity entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            Entities.Attach(entity);
            Entities.Update(entity);
            await Context.SaveChangesAsync();
        }

        protected async Task DeleteEntityAsync(TEntity entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));
                
            Entities.Remove(entity);
            await Context.SaveChangesAsync();
        }

        protected async Task<TEntity> FindEntityByIdAsync(Guid id, CancellationToken cancel = default)
        {
            return await Entities.FindAsync(new object[] { id }, cancel);
        }
    }
}