using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace CityApp.Logic.Utils
{
    public interface ICommand
    {
    }

    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Result Handle(TCommand linkCommand);
        Task<Result> HandleAsync(TCommand linkCommand);
    }

    public interface IValuedCommandHandler<in TCommand, TValue> : ICommandHandler<TCommand> 
        where TCommand : ICommand
    {
        Task<Result<TValue>> HandleWithResultAsync(TCommand linkCommand);
    }
}