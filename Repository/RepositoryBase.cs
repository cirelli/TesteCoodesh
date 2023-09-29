using System.Linq.Expressions;
using AutoMapper;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public abstract class RepositoryBase<T> where T : class
    {
        protected DataContext DataContext { get; private set; }
        protected readonly IMapper mapper;


        public RepositoryBase(DataContext repositoryContext, IMapper mapper)
        {
            DataContext = repositoryContext;
            this.mapper = mapper;
        }


        public IQueryable<T> FindAll()
            => DataContext.Set<T>().AsNoTracking();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
            => DataContext.Set<T>().Where(expression).AsNoTracking();

        public void Create(T entity)
            => DataContext.Set<T>().Add(entity);

        public void Update(T entity)
            => DataContext.Set<T>().Update(entity);

        public void Delete(T entity)
            => DataContext.Set<T>().Remove(entity);


        protected async Task<RModel?> FirstOrDefaultAsync<RModel>(IQueryable query, CancellationToken cancellationToken)
        {
            var mappedQuery = mapper.ProjectTo<RModel>(query);
            return await mappedQuery.FirstOrDefaultAsync(cancellationToken);
        }

        protected async Task<List<RModel>> ToListAsync<RModel>(IQueryable query, CancellationToken cancellationToken)
        {
            var mappedQuery = mapper.ProjectTo<RModel>(query);
            return await mappedQuery.ToListAsync(cancellationToken);
        }
    }
}
