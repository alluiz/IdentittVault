using IdentittVault.Entities;
using IdentittVault.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentittVault.Services
{
    public interface ICrudService<TEntity> where TEntity : Entity
    {
        Task CreateAsync(TEntity entity);
        Task DeleteAsync(Guid id);
        List<TEntity> ReadAll();
        Task<TEntity> ReadAsync(Guid id);
        List<TEntity> ReadByName(string name, ReadOperation operation);
        Task UpdateAsync(TEntity entity);
    }
}