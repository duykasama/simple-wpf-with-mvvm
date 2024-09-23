using NHibernate;
using PIMTool.Core.Pattern.Interfaces;

namespace PIMTool.Core.Pattern.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly ISessionFactory _sessionFactory;
    private readonly ISession _currentSession;
    private readonly ITransaction _transaction;
    private bool _unitOfWorkCompleted = false;

    public ISession CurrentSession => _currentSession;

    public UnitOfWork(ISessionFactory sessionFactory)
    {
        _sessionFactory = sessionFactory;
        _currentSession = _sessionFactory.OpenSession();
        _transaction = _currentSession.BeginTransaction();
    }

    public void Complete()
    {
        if (_unitOfWorkCompleted)
        {
            return;
        }

        _currentSession.Flush();
        _transaction.Commit();
        _currentSession.Close();
        _unitOfWorkCompleted = true;
    }

    public void Rollback()
    {
        if (_unitOfWorkCompleted)
        {
            return;
        }

        _transaction.Rollback();
        _unitOfWorkCompleted = true;
    }

    public void Dispose()
    {
        _transaction.Dispose();
        _currentSession.Dispose();
        GC.SuppressFinalize(this);
    }
}
