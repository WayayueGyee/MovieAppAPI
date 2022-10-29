using Microsoft.EntityFrameworkCore;

namespace MovieAppAPI.Services; 

public abstract class BaseService<TEntity> where TEntity : class {
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    protected BaseService(DbContext context) {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public virtual void Update<TModel>(Guid id, TEntity model) {
        _dbSet.Attach(model);
        _context.Entry(model).State = EntityState.Modified;
        var entry = _context.Entry(model);
        
        var modelType = typeof(TModel);
        var modelProperties = modelType.GetProperties();

        foreach (var modelProperty in modelProperties) {
            if (modelProperty.GetValue(model) is null) {
                entry.Property(modelProperty.Name).IsModified = false;
            }
        }

        _context.SaveChanges();
        
    }
}