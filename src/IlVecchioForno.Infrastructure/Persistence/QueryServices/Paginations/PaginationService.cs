using IlVecchioForno.Domain.Common;

namespace IlVecchioForno.Infrastructure.Persistence.QueryServices.Paginations;

internal sealed class PaginationService<TEntity> : IPaginationService<TEntity>
    where TEntity : class, IAuditable
{
    public IQueryable<TEntity> Paginate(IQueryable<TEntity> q, int page, int pageSize)
    {
        return q.Skip((page - 1) * pageSize)
            .Take(pageSize);
    }
}