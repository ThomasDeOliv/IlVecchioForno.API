using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Domain.Common;

namespace IlVecchioForno.Infrastructure.Persistence.QueryServices.Filters;

public interface IFilterService<TEntity>
    where TEntity : class, IAuditable
{
    IQueryable<TEntity> Filter(IQueryable<TEntity> q, IEnumerable<IFilterType> filters);
}