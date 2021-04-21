using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CityApp.Engine;
using CityApp.Interfaces;
using CityApp.Models;
using Dapper;

namespace CityApp.Services
{
    public class ElectricityMeasurementRepository : BaseRepository<ElectricityMeasurement>,
        IElectricityMeasurementRepository
    {
        public ElectricityMeasurementRepository(string connectionString) : base(connectionString)
        {
        }

        public override async Task<IEnumerable<ElectricityMeasurement>> GetAllAsync()
        {
            await using var connection = new SqlConnection(connectionString);
            var measurements = await connection.QueryAsync<ElectricityMeasurement>(
                "SELECT C. * FROM ElectricityMeasurements C");
            return measurements;
        }

        public override async Task<List<ElectricityMeasurement>> SearchAsync(string query)
        {
            if (string.IsNullOrEmpty(query)) return (await GetAllAsync()).ToList();

            await using var connection = new SqlConnection(connectionString);
            var measurements = await connection.QueryAsync<ElectricityMeasurement>(
                "SELECT C.* FROM ElectricityMeasurements C WHERE C.Name like @query",
                new {query = "%" + query + "%"});

            return measurements.ToList();
        }

        public override long Insert(ElectricityMeasurement entity)
        {
            using var connection = new SqlConnection(connectionString);
            var item = connection.ExecuteScalar(
                $"INSERT INTO ElectricityMeasurements(UserId, LowWatts,HighWats,EntryDate,ElectricityId)VALUES" +
                $"(@{nameof(entity.CityUserId)},@{nameof(entity.LowWatts)},@{nameof(entity.HighWats)},@{nameof(entity.EntryDate)},@{nameof(entity.ElectricityId)});" +
                "SELECT CAST(SCOPE_IDENTITY() as bigint)",
                entity);
            return Convert.ToInt64(item);
        }

        public async Task<PaginatedList<ElectricityMeasurement>> GetPagedForUserAsync(int cityUserId,
            int? electricityId, int page, int pageCount = 20)
        {
            await using var connection = new SqlConnection(connectionString);
            int offset = (page - 1) * pageCount;
            var currentQuery =
                "SELECT C.ElectricityMeasurementId, C.UserId, C.ElectricityId, C.EntryDate, C.LowWatts, C.HighWats " +
                "FROM ElectricityMeasurements C WHERE C.UserId=@cityUserId";

            if (electricityId.HasValue)
                currentQuery += " AND C.ElectricityId=@electricityId;";
            else
                currentQuery += ";";

            currentQuery += "SELECT COUNT(*) FROM ElectricityMeasurements";

            SqlMapper.GridReader result;
            if (electricityId.HasValue)
                result =
                    await connection.QueryMultipleAsync(currentQuery,
                        new {offset, pageCount, cityUserId, electricityId = electricityId.Value});
            else
                result =
                    await connection.QueryMultipleAsync(currentQuery,
                        new {offset, pageCount, cityUserId});

            var electricityMeasurements = result.Read<ElectricityMeasurement>();
            var count = result.ReadSingle<int>();
            return new PaginatedList<ElectricityMeasurement>(electricityMeasurements, count, page, pageCount);
        }
    }
}