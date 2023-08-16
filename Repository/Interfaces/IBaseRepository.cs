using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyPSG.API.Repositorys.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> Save(T obj);
        Task Update(T obj);
        Task Delete(T obj);
        Task<IEnumerable<T>> GetAll();
    }
}