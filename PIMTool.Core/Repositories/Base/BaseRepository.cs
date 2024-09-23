using NHibernate;
using PIMTool.Core.Entities;
using PIMTool.Core.Pattern.Interfaces;
using Serilog;

namespace PIMTool.Core.Repositories.Base;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly IUnitOfWork _unitOfWork;

    protected BaseRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void Add(T entity)
    {
        ExecuteInsideTransaction((session) => session.Save(entity));
    }

    public void Delete(T entity)
    {
        ExecuteInsideTransaction((session) => session.Delete(entity));
    }

    public T? Get(int id)
    {
        var session = _unitOfWork.CurrentSession;
        ArgumentNullException.ThrowIfNull(session, nameof(session));
        var entity = session
            .Query<T>()
            .Where(c => c.Id == id)
            .SingleOrDefault();
        return entity;
    }

    public IEnumerable<T> GetAll()
    {
        var session = _unitOfWork.CurrentSession;
        ArgumentNullException.ThrowIfNull(session, nameof(session));
        var queryableCollection = session.Query<T>().ToList();
        return queryableCollection;
    }

    public void Update(T entity)
    {
        ExecuteInsideTransaction((session) =>
        {
            session.Update(entity);
        });
    }

    private void ExecuteInsideTransaction(Action<ISession> action)
    {
        var session = _unitOfWork.CurrentSession;
        try
        {
            action.Invoke(session);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            throw;
        }
    }
}
