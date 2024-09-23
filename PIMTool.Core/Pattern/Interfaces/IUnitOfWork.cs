using NHibernate;

namespace PIMTool.Core.Pattern.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ISession CurrentSession { get; }
    void Complete();
    void Rollback();
}
