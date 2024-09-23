namespace PIMTool.Core.Repositories.Base;

public interface IBaseRepository<T> where T : class
{
    void Add(T entity);
    void Update(T entity);
    IEnumerable<T> GetAll();
    T? Get(int id);
    void Delete(T entity);
}
