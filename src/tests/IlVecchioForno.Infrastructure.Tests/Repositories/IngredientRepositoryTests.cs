using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Infrastructure.Common.Exceptions;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Filters;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Paginations;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Sorters;
using IlVecchioForno.Infrastructure.Persistence.Repositories;
using IlVecchioForno.Infrastructure.Tests.Utilities.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace IlVecchioForno.Infrastructure.Tests.Repositories;

public sealed class IngredientRepositoryTests : SeededInfrastructureTestsBase
{
    private readonly Mock<IFilterService<Ingredient>> _filterServiceMock;
    private readonly Mock<IPaginationService<Ingredient>> _paginationServiceMock;
    private readonly Mock<ISorterService<Ingredient, IngredientsSorter>> _sorterServiceMock;

    public IngredientRepositoryTests(DbContextFixture dbCtxFixture) : base(dbCtxFixture)
    {
        // Mocked dependencies
        this._filterServiceMock = new Mock<IFilterService<Ingredient>>();
        this._paginationServiceMock = new Mock<IPaginationService<Ingredient>>();
        this._sorterServiceMock = new Mock<ISorterService<Ingredient, IngredientsSorter>>();

        // Default filter service behavior
        this._filterServiceMock
            .Setup(f => f.Filter(
                    It.IsAny<IQueryable<Ingredient>>(),
                    It.IsAny<IEnumerable<IFilterType>>()
                )
            )
            .Returns((IQueryable<Ingredient> q, IEnumerable<IFilterType> filters) =>
                filters.Aggregate(q, (current, filter) => filter switch
                    {
                        SearchFilterType searchFilter when !string.IsNullOrEmpty(searchFilter.Search) =>
                            current.Where(i =>
                                EF.Functions.ILike(i.Name, $"%{searchFilter.Search}%")
                            ),

                        SearchFilterType searchFilter when string.IsNullOrEmpty(searchFilter.Search) => current,

                        _ => throw new NotSupportedFilterException(
                            nameof(Ingredient),
                            new NotSupportedException($"Filter type {filter.GetType().Name} is not supported.")
                        )
                    }
                )
            );

        // Default pagination service behavior
        this._paginationServiceMock
            .Setup(p => p.Paginate(
                    It.IsAny<IQueryable<Ingredient>>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()
                )
            )
            .Returns((IQueryable<Ingredient> q, int page, int pageSize) =>
                q.Skip((page - 1) * pageSize)
                    .Take(pageSize)
            );

        // Default sort service behavior
        this._sorterServiceMock
            .Setup(s => s.OrderBy(
                    It.IsAny<IQueryable<Ingredient>>(),
                    It.IsAny<IngredientsSorter>(),
                    It.IsAny<bool>()
                )
            )
            .Returns((IQueryable<Ingredient> q, IngredientsSorter sorter, bool descending) => (sorter, descending) switch
                {
                    (IngredientsSorter.Name, true) =>
                        q.OrderByDescending(p => p.Name)
                            .ThenBy(p => p.Id),

                    (IngredientsSorter.Name, false) =>
                        q.OrderBy(p => p.Name)
                            .ThenBy(p => p.Id),

                    (IngredientsSorter.Id, true) =>
                        q.OrderByDescending(p => p.Id),

                    _ => q.OrderBy(p => p.Id)
                }
            );
    }

    private EfIngredientRepository CreateNewRepository()
    {
        return new EfIngredientRepository(
            this._ctx,
            this._filterServiceMock.Object,
            this._paginationServiceMock.Object,
            this._sorterServiceMock.Object
        );
    }

    [Fact]
    public async Task TestExample()
    {
        // Arrange
        IIngredientRepository repository = this.CreateNewRepository();
        // Act
        Ingredient? result = await repository.FindAsync(1);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(DbMockedTestsData.TestsIngredients[0], result);
    }
}