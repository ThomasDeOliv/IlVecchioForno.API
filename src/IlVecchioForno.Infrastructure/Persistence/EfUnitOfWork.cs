using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Infrastructure.Common.Exceptions;

namespace IlVecchioForno.Infrastructure.Persistence;

internal sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly IlVecchioFornoDbContext _ctx;

    public EfUnitOfWork(IlVecchioFornoDbContext ctx)
    {
        this._ctx = ctx;
    }

    public int SaveChanges()
    {
        try
        {
            return this._ctx.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new SaveChangesException(ex);
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await this._ctx.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new SaveChangesException(ex);
        }
    }

    public int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        try
        {
            return this._ctx.SaveChanges(acceptAllChangesOnSuccess);
        }
        catch (Exception ex)
        {
            throw new SaveChangesException(ex);
        }
    }

    public async Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            return await this._ctx.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new SaveChangesException(ex);
        }
    }
}