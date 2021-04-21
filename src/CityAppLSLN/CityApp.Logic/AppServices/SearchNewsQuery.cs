using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using CityApp.Engine;
using CityApp.Logic.Utils;
using CityApp.Logic.ViewModels;
using MediatR;

namespace CityApp.Logic.AppServices
{
    public class SearchNewsQuery : IRequest<PaginatedList<NewsViewModel>>
    {
        public string Query { get; }
        public int Page { get; }
        public int DefaultPageCount { get; }

        public SearchNewsQuery(string query, int page, int defaultPageCount)
        {
            Query = query;
            Page = page;
            DefaultPageCount = defaultPageCount;
        }
    }

    public class SearchNewsQueryHandler : IRequestHandler<SearchNewsQuery, PaginatedList<NewsViewModel>>
    {
        private const string searchEndpoint = "https://scdata.search.windows.net";

        public async Task<PaginatedList<NewsViewModel>> Handle(SearchNewsQuery request,
            CancellationToken cancellationToken)
        {
            var searchKey = Environment.GetEnvironmentVariable("SearchServiceKey");
            var searchIndex = "azure-sql-news-data-index";
            var indexClient = new SearchClient(new Uri(searchEndpoint, UriKind.RelativeOrAbsolute), searchIndex,
                new AzureKeyCredential(searchKey));
            var searchOptions = new SearchOptions
            {
                QueryType = SearchQueryType.Simple,
                SearchMode = SearchMode.Any
            };
            var data = await indexClient.SearchAsync<NewsViewModel>($"{request.Query ?? ""}*", searchOptions,
                cancellationToken);
            var list = new List<NewsViewModel>();
            foreach (var currentData in data.Value.GetResults())
            {
                list.Add(currentData.Document);
            }

            return PaginatedList<NewsViewModel>.Create(list.AsQueryable(), request.Page,
                request.DefaultPageCount);
        }
    }
}