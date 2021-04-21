using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CityApp.Logic.Decorators
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => this.logger = logger;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            logger.LogInformation($"Handling {typeof(TRequest).Name}");
            var response = await next();
            stopWatch.Stop();
            logger.LogInformation($"Handled {typeof(TResponse).Name} in {stopWatch.ElapsedMilliseconds} ms");
            return response;
        }
    }
}