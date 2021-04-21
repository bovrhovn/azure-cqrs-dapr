using System.Threading.Tasks;

namespace CityApp.Logic.Utils
{
    public interface IQuery<TResult>
    {
    }

    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        TResult Handle(TQuery query);
        Task<TResult> HandleAsync(TQuery query);
    }
}