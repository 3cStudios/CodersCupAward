using CodersCupAward.Models;
using Microsoft.EntityFrameworkCore;

namespace CodersCupAward.Services
{
    public class NoTrackingRepository<T> where T : class
    {
        private readonly coderscupawardContext _dbContext;

        public NoTrackingRepository(coderscupawardContext dbContext)
        {
            // Ensure the type for the repository exists in the model of the context.
            if (dbContext.Model.FindEntityType(typeof(T)) == null) throw new System.Exception("Not set of type in context.");

            // Set the context for the repository.
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected async Task AddAsync(T entity) => await SaveChangesAndDetachAsync(entity, EntityState.Added).ConfigureAwait(true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected async Task AddRangeAsync(T[] entities) => await SaveChangesAndDetachAsync(entities, EntityState.Added).ConfigureAwait(true);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected IQueryable<T> Get() => _dbContext.Set<T>().AsNoTracking();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected async Task RemoveAsync(T entity) => await SaveChangesAndDetachAsync(entity, EntityState.Deleted).ConfigureAwait(true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected async Task RemoveRangeAsync(T[] entities) => await SaveChangesAndDetachAsync(entities, EntityState.Deleted).ConfigureAwait(true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected async Task UpdateAsync(T entity) => await SaveChangesAndDetachAsync(entity, EntityState.Modified).ConfigureAwait(true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected async Task UpdateRangeAsync(T[] entities) => await SaveChangesAndDetachAsync(entities, EntityState.Modified).ConfigureAwait(true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityState"></param>
        /// <returns></returns>
        private async Task SaveChangesAndDetachAsync(T entity, EntityState entityState)
        {
            await SaveChangesAndDetachAsync(new T[] { entity }, entityState).ConfigureAwait(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="entityState"></param>
        /// <returns></returns>
        private async Task SaveChangesAndDetachAsync(T[] entities, EntityState entityState)
        {
            // Attach entities with desired state.
            foreach (var entity in entities)
            {
                _dbContext.Entry(entity).State = entityState;
            }

            // Save changes.
            await _dbContext.SaveChangesAsync().ConfigureAwait(true);

            // Detach entities.
            foreach(var entity in entities)
            {
                _dbContext.Entry(entity).State = EntityState.Detached;
            }
        }
    }
}
