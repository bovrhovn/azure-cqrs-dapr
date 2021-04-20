using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CityApp.Engine;
using CityApp.Interfaces;
using CityApp.Models;
using Dapper;

namespace CityApp.Services
{
    public class CityUserRepository : BaseRepository<CityUser>, ICityUserRepository
    {
        public CityUserRepository(string connectionString) : base(connectionString)
        {
        }

        public override async Task<IEnumerable<CityUser>> GetAllAsync()
        {
            await using var connection = new SqlConnection(connectionString);
            var item = await connection.QueryAsync<CityUser>(
                "SELECT C.CityUserId, C.Email,C.FullName,C.Approved, C.ApprovalDate " +
                "FROM CityUsers C ORDER BY C.CityUserId DESC");
            return item;
        }

        public override bool Update(CityUser entity)
        {
            entity.Password = PasswordHash.CreateHash(entity.Password);
            using var connection = new SqlConnection(connectionString);
            
            var item = connection.Execute(
                $"UPDATE CityUsers SET FullName=@{nameof(entity.FullName)},Email=@{nameof(entity.Email)},Password=@{nameof(entity.Password)} " +
                $"WHERE CityUserId=@{nameof(entity.CityUserId)}",
                entity);
            
            return item > 0;
        }

        public override async Task<PaginatedList<CityUser>> GetPagedAsync(int page, int pageCount = 20)
        {
            await using var connection = new SqlConnection(connectionString);
            int offset = (page - 1) * pageCount;
            var query = "SELECT CityUserId, C.Email,C.FullName,C.Approved, C.ApprovalDate " +
                        "FROM CityUsers C ORDER BY C.CityUserId DESC OFFSET @offset ROWS FETCH NEXT @pageCount ROWS ONLY;" +
                        "SELECT COUNT(*) FROM CityUsers";

            var result = await connection.QueryMultipleAsync(query, new {offset, pageCount});

            var cityUsers = result.Read<CityUser>();
            int count = result.ReadSingle<int>();
            return new PaginatedList<CityUser>(cityUsers, count, page, pageCount);
        }

        public override async Task<CityUser> GetDetailsAsync(int id)
        {
            await using var connection = new SqlConnection(connectionString);
            var query = "SELECT CityUserId, C.Email,C.FullName,C.Approved, C.ApprovalDate " +
                        "FROM CityUsers C WHERE CityUserId=@id;" +
                        "SELECT N.NewsId,N.Title, N.Content,N.ShortDescription,N.ExternalLink FROM News N " + 
                        "JOIN News2Users U on U.NewsId=N.NewsId WHERE U.CityUserId=@id;" + 
                        "SELECT E.ElectricityMeasurementId, E.UserId, E.EntryDate,E.LowWatts,E.HighWats FROM " + 
                        "ElectricityMeasurements E WHERE E.UserId=@id";

            var result = await connection.QueryMultipleAsync(query, new {id});

            var cityUsers = result.ReadSingleOrDefault<CityUser>();
            cityUsers.Subscriptions = result.Read<News>().AsList();
            cityUsers.ElectricityMeasurement = result.Read<ElectricityMeasurement>().AsList();
            return cityUsers;
        }

        public override long Insert(CityUser entity)
        {
            entity.Password = PasswordHash.CreateHash(entity.Password);
            entity.Approved = true;
            entity.ApprovalDate = DateTime.Now;
            
            using var connection = new SqlConnection(connectionString);
            
            var item = connection.ExecuteScalar(
                $"INSERT INTO CityUsers(FullName,Email,Password, Approved, ApprovalDate)VALUES(@{nameof(entity.FullName)},@{nameof(entity.Email)},@{nameof(entity.Password)},@{nameof(entity.Approved)},@{nameof(entity.ApprovalDate)});" +
                "SELECT CAST(SCOPE_IDENTITY() as bigint)",
                entity);
            return Convert.ToInt64(item); 
        }

        public async Task<CityUser> LoginAsync(string username, string password)
        {
            await using var connection = new SqlConnection(connectionString);
            var item = await connection.QuerySingleOrDefaultAsync<CityUser>(
                "SELECT U.* " +
                "FROM CityUsers U WHERE U.Email=@username", new {username});

            if (item == null) return null;
            return PasswordHash.ValidateHash(password, item.Password) ? item : null;
        }
    }
}