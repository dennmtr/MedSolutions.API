using MedSolutions.Domain.Common.Interfaces;
using MedSolutions.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedSolutions.Infrastructure.Common.Repositories;

public class RepositoryBase<TEntity>(MedSolutionsDbContext context) : IRepositoryBase<TEntity> where TEntity : class
{
    private readonly MedSolutionsDbContext _context = context;

    protected DbSet<TEntity> Entities => _context.Set<TEntity>();

    public IQueryable<TEntity> Query(bool asNoTracking = true)
        => asNoTracking ? Entities.AsNoTracking() : Entities;

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Entities.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Entities.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Entities.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

