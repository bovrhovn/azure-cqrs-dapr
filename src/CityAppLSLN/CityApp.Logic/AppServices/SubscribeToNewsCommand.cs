using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;
using CityApp.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CityApp.Logic.AppServices
{
    public class SubscribeToNewsCommand : IRequest<bool>
    {
        public int CityUserId { get; }
        public int NewsId { get; }

        public SubscribeToNewsCommand(int cityUserId, int newsId)
        {
            CityUserId = cityUserId;
            NewsId = newsId;
        }
    }

    public class SubscribeToNewsCommandHandler : IRequestHandler<SubscribeToNewsCommand, bool>
    {
        private readonly ILogger<SubscribeToNewsCommandHandler> logger;
        private readonly ICityUserRepository cityUserRepository;

        public SubscribeToNewsCommandHandler(ILogger<SubscribeToNewsCommandHandler> logger, ICityUserRepository cityUserRepository)
        {
            this.logger = logger;
            this.cityUserRepository = cityUserRepository;
        }

        public async Task<bool> Handle(SubscribeToNewsCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Adding to database");
            try
            {
                await cityUserRepository.AddSubscriptionToNewsAsync(request.CityUserId, request.NewsId);
                logger.LogInformation($"News with id {request.NewsId} has been added to {request.CityUserId}");
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return false;
            }

            return true;
        }
    }
}