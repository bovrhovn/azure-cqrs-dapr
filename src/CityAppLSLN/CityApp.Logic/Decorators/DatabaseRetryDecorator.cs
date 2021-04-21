using System;
using System.Threading.Tasks;
using CityApp.Logic.Utils;
using CSharpFunctionalExtensions;

namespace CityApp.Logic.Decorators
{
    public sealed class DatabaseRetryDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> handler;
        private readonly Config config;

        public DatabaseRetryDecorator(ICommandHandler<TCommand> handler, Config config)
        {
            this.handler = handler;
            this.config = config;
        }

        public Task<Result> HandleAsync(TCommand linkCommand)
        {
            throw new NotImplementedException();
        }

        private bool IsDatabaseException(Exception exception)
        {
            var message = exception.InnerException?.Message;

            if (message == null) return false;

            return message.Contains("The connection is broken and recovery is not possible")
                || message.Contains("error occurred while establishing a connection");
        }

        Result ICommandHandler<TCommand>.Handle(TCommand linkCommand)
        {
            for (var currentCounter = 0; ; currentCounter++)
            {
                try
                {
                    var result = handler.Handle(linkCommand);
                    return result;
                }
                catch (Exception ex)
                {
                    if (currentCounter >= config.NumberOfDatabaseRetries || !IsDatabaseException(ex))
                        throw;
                }
            }
        }
    }
}
