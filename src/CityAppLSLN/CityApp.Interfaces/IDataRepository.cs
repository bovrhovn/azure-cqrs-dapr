using System.Collections.Generic;
using System.Threading.Tasks;

namespace CityApp.Interfaces
{
    public interface IDataRepository<TEntity> where TEntity : class
    {
        bool Delete(TEntity entity);
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();
        long Insert(TEntity entity);
        bool Update(TEntity entity);
        Task<TEntity> GetDetailsAsync(int id);
        Task<List<TEntity>> SearchAsync(string query);
        Task<bool> Delete(int id);
    }
}