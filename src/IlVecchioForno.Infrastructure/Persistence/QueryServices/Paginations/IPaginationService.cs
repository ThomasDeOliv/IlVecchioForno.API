using IlVecchioForno.Domain.Common;

namespace IlVecchioForno.Infrastructure.Persistence.QueryServices.Paginations;

public interface IPaginationService<TEntity>
    where TEntity : class, IAuditable
{
    IQueryable<TEntity> Paginate(IQueryable<TEntity> q, int page, int pageSize);
}