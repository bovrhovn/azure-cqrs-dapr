using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;
using CityApp.Interfaces;
using CityApp.Logic.ViewModels;
using MediatR;

namespace CityApp.Logic.AppServices
{
    public class GetNewsDetailsQuery : IRequest<NewsDetailsViewModel>
    {
        public int NewsId { get; }
        public int CityUserId { get; }

        public GetNewsDetailsQuery(int newsId, int cityUserId)
        {
            NewsId = newsId;
            CityUserId = cityUserId;
        }
    }

    public class GetNewsDetailsQueryHandler : IRequestHandler<GetNewsDetailsQuery, NewsDetailsViewModel>
    {
        private readonly INewsRepository newsRepository;
        private readonly ICityUserRepository userRepository;

        public GetNewsDetailsQueryHandler(INewsRepository newsRepository, ICityUserRepository userRepository)
        {
            this.newsRepository = newsRepository;
            this.userRepository = userRepository;
        }

        public async Task<NewsDetailsViewModel> Handle(GetNewsDetailsQuery request, CancellationToken cancellationToken)
        {
            var news = await newsRepository.GetDetailsAsync(request.NewsId);
            var newsDetailsViewModel = new NewsDetailsViewModel
            {
                Title = news.Title,
                Content = news.Content,
                NewsId = request.NewsId
            };

            foreach (var newsCategory in news.Categories)
            {
                newsDetailsViewModel.Categories.Add(new CategoryViewModel {Name = newsCategory.Name});
            }

            if (request.CityUserId > -1)
            {
                newsDetailsViewModel.IsCurrentLoggedInUserSubscribed =
                    await userRepository.IsSubscribedToNewsAsync(request.CityUserId, request.NewsId);
            }

            return newsDetailsViewModel;
        }
    }
}