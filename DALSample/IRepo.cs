using System.Collections.Generic;

namespace DALSample
{
    public interface IRepo<T> where T : new()
    {
        T Get(int id);
        IEnumerable<T> GetAll();
        int Insert(T o);
        int Update(T o);
        int UpdateWhatWhere(object what, object where);
        int InsertNoIdentity(T o);
        IEnumerable<T> GetPage(int page, int pageSize);
        int Count();
        IPageable<T> GetPageable(int page, int pageSize);
        IEnumerable<T> GetWhere(object where);
        int Delete(int id);
        int CountWhere(object where);
    }
}