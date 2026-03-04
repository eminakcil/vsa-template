using System.Diagnostics;
using MediatR;
using Serilog.Context;

namespace VsaTemplate.Common.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var requestName = typeof(TRequest).Name;

        using (LogContext.PushProperty("RequestName", requestName))
        {
            logger.LogInformation("Processing request {@Request}", request);

            var start = Stopwatch.GetTimestamp();

            try
            {
                var response = await next();
                var elapsed = Stopwatch.GetElapsedTime(start);

                logger.LogInformation(
                    "Processed {RequestName} successfully in {ElapsedMilliseconds}ms",
                    requestName,
                    elapsed.TotalMilliseconds
                );

                return response;
            }
            catch (Exception ex)
            {
                var elapsed = Stopwatch.GetElapsedTime(start);
                logger.LogError(
                    ex,
                    "Request {RequestName} failed after {ElapsedMilliseconds}ms",
                    requestName,
                    elapsed.TotalMilliseconds
                );
                throw;
            }
        }
    }
}
