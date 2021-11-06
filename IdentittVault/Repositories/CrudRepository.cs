using IdentittVault.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentittVault.Repositories
{
    public class CrudRepository<TEntity> : ICrudRepository<TEntity> where TEntity : Entity
    {
        protected DbSet<TEntity> DbSet { get; private set; }
        protected IdentittVaultContext Context { get; private set; }

        public CrudRepository(IdentittVaultContext context)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
        }

        public virtual async Task CreateAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity).ConfigureAwait(false);
        }

        public virtual async Task<TEntity> ReadAsync(Guid id)
        {
            TEntity entity = await DbSet.FindAsync(id).ConfigureAwait(false);

            if (entity != null)
                Context.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public virtual List<TEntity> ReadAll()
        {
            List<TEntity> entities = DbSet.ToList();

            return entities;
        }

        public IEnumerable<TEntity> ReadByName(string name, ReadOperation operation)
        {
            return operation switch
            {
                ReadOperation.Equal => DbSet.AsNoTracking().Where(i => i.Name.Equals(name)),
                ReadOperation.Like => DbSet.AsNoTracking().Where(i => EF.Functions.Like(i.Name, $"%{name}%")),
                _ => throw new ArgumentOutOfRangeException(nameof(operation), "The operation must be valid."),
            };
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public virtual void DeleteRange(List<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public virtual async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
