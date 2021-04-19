using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CityApp.Engine;
using CityApp.Interfaces;
using CityApp.Models;
using Dapper;

namespace CityApp.Services
{
    public class NewsRepository : BaseRepository<News>, INewsRepository
    {
        public NewsRepository(string connectionString) : base(connectionString)
        {
        }

        public override async Task<IEnumerable<News>> GetAllAsync()
        {
            await using var connection = new SqlConnection(connectionString);
            var item = await connection.QueryAsync<News>(
                "SELECT C.NewsId, C.Title,C.ShortDescription,C.Content " +
                "FROM News C ORDER BY C.NewsId DESC");
            return item;
        }

        public override async Task<PaginatedList<News>> GetPagedAsync(int page, int pageCount = 20)
        {
            await using var connection = new SqlConnection(connectionString);
            int offset = (page - 1) * pageCount;
            var query = "SELECT C.NewsId, C.Title,C.ShortDescription,C.Content " +
                        "FROM News C ORDER BY C.NewsId DESC OFFSET @offset ROWS FETCH NEXT @pageCount ROWS ONLY;" +
                        "SELECT COUNT(*) FROM News";

            var result = await connection.QueryMultipleAsync(query, new {offset, pageCount});

            var selectedNews = result.Read<News>();
            var count = result.ReadSingle<int>();
            return new PaginatedList<News>(selectedNews, count, page, pageCount);
        }

        public async Task<PaginatedList<News>> SearchPagedAsync(string query, int page, int pageCount = 20)
        {
            await using var connection = new SqlConnection(connectionString);
            int offset = (page - 1) * pageCount;
            var currentQuery = "SELECT C.NewsId, C.Title,C.ShortDescription,C.Content " +
                               "FROM News C ORDER BY C.NewsId DESC WHERE C.Title like '%@query%' OFFSET @offset ROWS FETCH NEXT @pageCount ROWS ONLY;" +
                               "SELECT COUNT(*) FROM News";

            var result = await connection.QueryMultipleAsync(currentQuery, new {offset, pageCount, query});

            var selectedNews = result.Read<News>();
            var count = result.ReadSingle<int>();
            return new PaginatedList<News>(selectedNews, count, page, pageCount);
        }

        public override async Task<News> GetDetailsAsync(int id)
        {
            await using var connection = new SqlConnection(connectionString);
            var query = "SELECT C.* FROM News C WHERE C.NewsId=@id;" +
                        "SELECT B.* FROM Categories B join News2Categories E on B.CategoryId = E.CategoryId WHERE E.NewsId=@id;" +
                        "SELECT B.* FROM CityUsers B join News2Users E on B.CityUserId = E.CityUserId WHERE E.NewsId=@id";

            var result = await connection.QueryMultipleAsync(query, new {id});

            var selectedNews = result.ReadSingle<News>();
            selectedNews.Categories = result.Read<Category>().AsList();
            selectedNews.Users = result.Read<CityUser>().AsList();

            return selectedNews;
        }
    }
}