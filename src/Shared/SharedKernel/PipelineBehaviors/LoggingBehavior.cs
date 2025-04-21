using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace SharedKernel.PipelineBehaviors
{
    public class LoggingBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : notnull
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[START] Handling request: {RequestType} | Response: {ResponseType} \n Data: {Request}", 
                typeof(TRequest).Name, typeof(TResponse).Name, request
            );

            Stopwatch stopwatch = Stopwatch.StartNew();
            TResponse response = await next(cancellationToken);
            stopwatch.Stop();

            TimeSpan elapsed = stopwatch.Elapsed;
            if (elapsed.Seconds > 3)
            {
                _logger.LogWarning("[SLOW] Handled request: {RequestType} | Response: {ResponseType} \n Data: {Request} \n Elapsed Time: {ElapsedMilliseconds} ms",
                    typeof(TRequest).Name, typeof(TResponse).Name, request, elapsed.TotalMilliseconds
                );
            }

            _logger.LogInformation("[END] Handled request: {RequestType} | Response: {ResponseType} \n Data: {Request} \n Elapsed Time: {ElapsedMilliseconds} ms",
                typeof(TRequest).Name, typeof(TResponse).Name, response, elapsed.TotalMilliseconds
            );

            return response;
        }
    }
}
