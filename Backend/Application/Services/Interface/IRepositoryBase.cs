using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        Task<T> Find(Expression<Func<T, bool>> expression);
        Task Create(T entity);
        Task AddList(List<T> entities);
        void Update(T entity);
        void Delete(T entity);
    }
}
