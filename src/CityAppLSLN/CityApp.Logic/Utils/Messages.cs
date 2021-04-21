using System;
using System.Diagnostics;
using CSharpFunctionalExtensions;

namespace CityApp.Logic.Utils
{
    public sealed class Messages
    {
        private readonly IServiceProvider provider;

        public Messages(IServiceProvider provider) => this.provider = provider;

        public T Dispatch<T>(IQuery<T> query)
        {
            var type = typeof(IQueryHandler<,>);
            Type[] typeArgs = {query.GetType(), typeof(T)};
            var handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = provider.GetService(handlerType);
            if (handler == null)
            {
                Debug.Write("Cast to service has been null");
                throw new InvalidCastException("Cast to service has been null");
            }
            return (T) handler.Handle((dynamic) query);
        }

        public Result Dispatch(ICommand command)
        {
            var type = typeof(ICommandHandler<>);
            Type[] typeArgs = {command.GetType()};
            var handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = provider.GetService(handlerType);
            return (Result) handler.Handle((dynamic) command);
        }

        public Result<T> DispatchValue<V, T>(ICommand command) where T : class where V : ICommand
        {
            var type = typeof(IValuedCommandHandler<V, T>);
            Type[] typeArgs = {command.GetType()};
            var handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = provider.GetService(handlerType);
            return (Result<T>) handler.Handle((dynamic) command);
        }
    }
}