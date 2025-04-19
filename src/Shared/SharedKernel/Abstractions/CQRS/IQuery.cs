using MediatR;

namespace SharedKernel.Abstractions.CQRS
{
    public interface IQuery<out TResult> : IRequest<TResult>
        where TResult : notnull
    {
    }
}
