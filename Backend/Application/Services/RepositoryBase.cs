using Infrastructure.Persistence;
using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.Services
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        // you can use private readonly for the depency injection if you want
        protected DataContext RepositoryContext { get; set; }
        public RepositoryBase(DataContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }
        public IQueryable<T> FindAll() => RepositoryContext.Set<T>().AsNoTracking();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
            RepositoryContext.Set<T>().Where(expression).AsNoTracking();

        public async Task<T> Find(Expression<Func<T, bool>> expression) => await
         RepositoryContext.Set<T>().FirstOrDefaultAsync(expression);

        public async Task Create(T entity) => await RepositoryContext.Set<T>().AddAsync(entity);

        public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);

        public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);

        // you can add as many functions as you want base on the requirements
    }
}
