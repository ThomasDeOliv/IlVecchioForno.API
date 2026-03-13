using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Domain.QuantityTypes;
using IlVecchioForno.Infrastructure.Common.Exceptions;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Filters;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Paginations;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Sorters;
using IlVecchioForno.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace IlVecchioForno.Infrastructure.Tests.Repositories;

public sealed class QuantityTypeRepositoryTests : EmptyInfrastructureTestsBase
{
    private readonly Mock<IFilterService<QuantityType>> _filterServiceMock;
    private readonly Mock<IPaginationService<QuantityType>> _paginationServiceMock;
    private readonly Mock<ISorterService<QuantityType, QuantityTypesSorter>> _sorterServiceMock;

    public QuantityTypeRepositoryTests(DbContextFixture dbCtxFixture) : base(dbCtxFixture)
    {
        // Mocked dependencies
        this._filterServiceMock = new Mock<IFilterService<QuantityType>>();
        this._paginationServiceMock = new Mock<IPaginationService<QuantityType>>();
        this._sorterServiceMock = new Mock<ISorterService<QuantityType, QuantityTypesSorter>>();

        // Default filter service behavior
        this._filterServiceMock
            .Setup(f => f.Filter(
                    It.IsAny<IQueryable<QuantityType>>(),
                    It.IsAny<IEnumerable<IFilterType>>()
                )
            )
            .Returns((IQueryable<QuantityType> q, IEnumerable<IFilterType> filters) =>
                filters.Aggregate(q, (current, filter) => filter switch
                    {
                        SearchFilterType searchFilter when !string.IsNullOrWhiteSpace(searchFilter.Search) =>
                            current.Where(qT =>
                                EF.Functions.ILike(qT.Name, $"%{searchFilter.Search}%")
                                || EF.Functions.ILike(qT.Unit, $"%{searchFilter.Search}%")
                            ),

                        SearchFilterType searchFilter when string.IsNullOrEmpty(searchFilter.Search) => current,

                        _ => throw new NotSupportedFilterException(
                            nameof(QuantityType),
                            new NotSupportedException($"Filter type {filter.GetType().Name} is not supported.")
                        )
                    }
                )
            );

        // Default pagination service behavior
        this._paginationServiceMock
            .Setup(p => p.Paginate(
                    It.IsAny<IQueryable<QuantityType>>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()
                )
            )
            .Returns((IQueryable<QuantityType> q, int page, int pageSize) =>
                q.Skip((page - 1) * pageSize)
                    .Take(pageSize)
            );

        // Default sort service behavior
        this._sorterServiceMock
            .Setup(s => s.OrderBy(
                    It.IsAny<IQueryable<QuantityType>>(),
                    It.IsAny<QuantityTypesSorter>(),
                    It.IsAny<bool>()
                )
            )
            .Returns((IQueryable<QuantityType> q, QuantityTypesSorter sorter, bool descending) => (sorter, descending) switch
                {
                    (QuantityTypesSorter.Name, true) =>
                        q.OrderByDescending(p => p.Name)
                            .ThenBy(p => p.Id),

                    (QuantityTypesSorter.Name, false) =>
                        q.OrderBy(p => p.Name)
                            .ThenBy(p => p.Id),

                    (QuantityTypesSorter.Unit, true) =>
                        q.OrderByDescending(p => p.Unit)
                            .ThenBy(p => p.Id),

                    (QuantityTypesSorter.Unit, false) =>
                        q.OrderBy(p => p.Unit)
                            .ThenBy(p => p.Id),

                    (QuantityTypesSorter.Id, true) =>
                        q.OrderByDescending(p => p.Id),

                    _ => q.OrderBy(p => p.Id)
                }
            );
    }

    private EfQuantityTypeRepository CreateNewRepository()
    {
        return new EfQuantityTypeRepository(
            this._ctx,
            this._filterServiceMock.Object,
            this._paginationServiceMock.Object,
            this._sorterServiceMock.Object
        );
    }
}