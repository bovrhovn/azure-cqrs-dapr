using System;
using System.Threading.Tasks;
using CityApp.Logic.Utils;
using CSharpFunctionalExtensions;
using Newtonsoft.Json;

namespace CityApp.Logic.Decorators
{
    public sealed class AuditLoggingDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> handler;

        public AuditLoggingDecorator(ICommandHandler<TCommand> handler)
        {
            this.handler = handler;
        }

        public Task<Result> HandleAsync(TCommand linkCommand)
        {
            throw new NotImplementedException();
        }

        Result ICommandHandler<TCommand>.Handle(TCommand linkCommand)
        {
            var commandJson = JsonConvert.SerializeObject(linkCommand);

            // TODO: use proper login here
            Console.WriteLine($"Command of type {linkCommand.GetType().Name}: {commandJson}");

            return handler.Handle(linkCommand);
        }
    }
}
