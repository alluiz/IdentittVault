using IdentittVault.Entities;
using IdentittVault.Repositories;
using IdentittVault.System;
using IdentittVault.System.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentittVault.Services
{
    public class CrudService<TEntity, TRepository> : ICrudService<TEntity> where TEntity : Entity
        where TRepository : ICrudRepository<TEntity>
    {
        protected readonly TRepository repository;
        protected readonly IdentittVaultSecure secure;

        public CrudService(TRepository repository, IdentittVaultSecure secure)
        {
            this.repository = repository;
            this.secure = secure;
        }

        protected virtual void OnCreate(TEntity entity)
        {

        }

        protected virtual void OnDelete(TEntity entity)
        {

        }

        protected virtual void OnRead(List<TEntity> entities)
        {

        }

        protected virtual void OnRead(TEntity entity)
        {

        }

        protected virtual void OnUpdate(TEntity entity)
        {

        }

        internal bool ValidatePassword(string password)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(TEntity entity)
        {
            OnCreate(entity);
            await repository.CreateAsync(entity);
            await repository.SaveChangesAsync();
        }

        public async Task<TEntity> ReadAsync(Guid id)
        {
            TEntity entity = await repository.ReadAsync(id);

            if (entity == null)
            {
                NotFoundEntity(id);
            }

            OnRead(entity);

            return entity;
        }

        public List<TEntity> ReadAll()
        {
            List<TEntity> entities = repository.ReadAll();
            OnRead(entities);

            return entities;
        }

        public List<TEntity> ReadByName(string name, ReadOperation operation)
        {
            List<TEntity> entities = repository.ReadByName(name, operation).ToList();
            OnRead(entities);

            return entities;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            TEntity existentEntity = await ReadAsync(entity.Id);

            if (existentEntity != null)
            {
                OnUpdate(entity);
                repository.Update(entity);
                await repository.SaveChangesAsync();
            }
            else
            {
                NotFoundEntity(entity.Id);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            TEntity entity = await ReadAsync(id);

            if (entity != null)
            {
                OnDelete(entity);
                repository.Delete(entity);
                await repository.SaveChangesAsync();
            }
            else
            {
                NotFoundEntity(id);
            }
        }

        protected static void NotFoundEntity(Guid id)
        {
            throw new IdentittVaultNotFoundException($"The provided id '{id}' was not found. Check the id value.");
        }

        protected static void ConflictValue(string fieldName, string fieldValue)
        {
            throw new IdentittVaultConflictException($"The provided {fieldName} value '{fieldValue}' already exists. Try some other value.");
        }
    }
}
