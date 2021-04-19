using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CityApp.Engine;
using CityApp.Interfaces;
using Dapper.Contrib.Extensions;

namespace CityApp.Services
{
    public abstract class BaseRepository<TEntity> : IDataRepository<TEntity> where TEntity : class
    {
        protected readonly string connectionString;

        protected BaseRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        IDbConnection connection;
        protected IDbConnection Connection => connection ??= new SqlConnection(connectionString);

        public virtual IEnumerable<TEntity> GetAll()
        {
            using var currentConnection = Connection;
            return currentConnection.GetAll<TEntity>();
        }

        public virtual Task<IEnumerable<TEntity>> GetAllAsync()
        {
            using var currentConnection = Connection;
            return currentConnection.GetAllAsync<TEntity>();
        }

        public virtual bool Delete(TEntity entity)
        {
            using var conn = Connection;
            return conn.Delete(entity);
        }

        public virtual long Insert(TEntity entity)
        {
            using var conn = Connection;
            return conn.Insert(entity);
        }

        public virtual bool Update(TEntity entity)
        {
            using var conn = Connection;
            return conn.Update(entity);
        }

        public virtual Task<TEntity> GetDetailsAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<List<TEntity>> SearchAsync(string query)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<bool> Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<PaginatedList<TEntity>> GetPagedAsync(int page, int pageCount = 20)
        {
            throw new System.NotImplementedException();
        }
    }
}