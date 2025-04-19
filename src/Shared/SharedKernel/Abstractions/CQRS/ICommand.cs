using MediatR;

namespace SharedKernel.Abstractions.CQRS
{
    public interface ICommand : ICommand<Unit>
    {
    }

    public interface ICommand<out TResposne> : IRequest<TResposne>
    {
    }
}
