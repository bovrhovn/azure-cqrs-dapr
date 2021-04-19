using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CityApp.Interfaces;
using CityApp.Models;
using Dapper;

namespace CityApp.Services
{
    public class ElectricityMeasurementRepository : BaseRepository<ElectricityMeasurement>, IElectricityMeasurementRepository
    {
        public ElectricityMeasurementRepository(string connectionString) : base(connectionString)
        {
        }
        
        public override async Task<IEnumerable<ElectricityMeasurement>> GetAllAsync()
        {
            await using var connection = new SqlConnection(connectionString);
            var categories = await connection.QueryAsync<ElectricityMeasurement>(
                "SELECT C. * FROM ElectricityMeasurements C");

            return categories;
        }

        public override async Task<List<ElectricityMeasurement>> SearchAsync(string query)
        {
            if (string.IsNullOrEmpty(query)) return (await GetAllAsync()).ToList();

            await using var connection = new SqlConnection(connectionString);
            var categories = await connection.QueryAsync<ElectricityMeasurement>(
                "SELECT C.* FROM ElectricityMeasurements C WHERE C.Name like @query",
                new {query = "%" + query + "%"});

            return categories.ToList();
        }
    }
}