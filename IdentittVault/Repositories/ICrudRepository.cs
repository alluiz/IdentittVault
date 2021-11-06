using IdentittVault.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentittVault.Repositories
{
    public interface ICrudRepository<TEntity> where TEntity : Entity
    {
        Task CreateAsync(TEntity entity);
        void Delete(TEntity entity);
        void DeleteRange(List<TEntity> entities);
        List<TEntity> ReadAll();
        Task<TEntity> ReadAsync(Guid id);
        IEnumerable<TEntity> ReadByName(string name, ReadOperation operation);
        Task SaveChangesAsync();
        void Update(TEntity entity);
    }
}