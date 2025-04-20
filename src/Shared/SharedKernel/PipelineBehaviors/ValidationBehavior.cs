using FluentValidation;
using MediatR;
using SharedKernel.Abstractions.CQRS;

namespace SharedKernel.PipelineBehaviors
{
    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            ValidationContext<TRequest> context = new(request);
            FluentValidation.Results.ValidationResult[] validationResult =
                await Task.WhenAll(_validators.Select(x => x.ValidateAsync(context, cancellationToken)));

            List<FluentValidation.Results.ValidationFailure> errors =
                validationResult
                .Where(x => x.Errors.Count != 0)
                .SelectMany(x => x.Errors)
                .ToList();

            if(errors.Count != 0)
            {
                throw new ValidationException(errors);
            }

            return await next(cancellationToken);
        }
    }
}
