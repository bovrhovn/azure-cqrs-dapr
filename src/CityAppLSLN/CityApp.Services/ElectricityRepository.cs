using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CityApp.Interfaces;
using CityApp.Models;
using Dapper;

namespace CityApp.Services
{
    public class ElectricityRepository : BaseRepository<Electricity>, IElectricityRepository
    {
        public ElectricityRepository(string connectionString) : base(connectionString)
        {
        }

        public override async Task<IEnumerable<Electricity>> GetAllAsync()
        {
            await using var connection = new SqlConnection(connectionString);
            var categories = await connection.QueryAsync<Electricity>(
                "SELECT C. * FROM Electricities C");

            return categories;
        }

        public override async Task<List<Electricity>> SearchAsync(string query)
        {
            if (string.IsNullOrEmpty(query)) return (await GetAllAsync()).ToList();

            await using var connection = new SqlConnection(connectionString);
            var categories = await connection.QueryAsync<Electricity>(
                "SELECT C.* FROM Electricities C WHERE C.Name like @query",
                new {query = "%" + query + "%"});

            return categories.ToList();
        }
    }
}