using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CityApp.Interfaces;
using CityApp.Models;
using Dapper;

namespace CityApp.Services
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(string connectionString) : base(connectionString)
        {
        }

        public override async Task<IEnumerable<Category>> GetAllAsync()
        {
            await using var connection = new SqlConnection(connectionString);
            var categories = await connection.QueryAsync<Category>(
                "SELECT C. * FROM Categories C");

            return categories;
        }

        public override async Task<List<Category>> SearchAsync(string query)
        {
            if (string.IsNullOrEmpty(query))
                return (await GetAllAsync()).ToList();

            await using var connection = new SqlConnection(connectionString);
            var categories = await connection.QueryAsync<Category>(
                "SELECT C.* FROM Categories C WHERE C.Name like @query",
                new {query = "%" + query + "%"});

            return categories.ToList();
        }
    }
}