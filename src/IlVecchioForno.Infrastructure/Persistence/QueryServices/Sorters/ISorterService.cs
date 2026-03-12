using IlVecchioForno.Domain.Common;

namespace IlVecchioForno.Infrastructure.Persistence.QueryServices.Sorters;

internal interface ISorterService<TEntity, in TSorter>
    where TEntity : class, IAuditable
    where TSorter : struct, Enum
{
    IOrderedQueryable<TEntity> OrderBy(IQueryable<TEntity> q, TSorter sorter, bool descending);
}