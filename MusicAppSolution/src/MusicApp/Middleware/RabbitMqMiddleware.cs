namespace MusicApp.Middleware;

public class RabbitMqMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RabbitMqMiddleware> _logger;
    public RabbitMqMiddleware(RequestDelegate next, ILogger<RabbitMqMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var correlationId = context.TraceIdentifier;
        var request = context.Request;
        var response = context.Response;
        var requestContent = await ReadRequestBody(request);
        var responseContent = await ReadResponseBody(response);
        _logger.LogInformation($"Request: {request.Method} {request.Path} {request.QueryString} {requestContent}");
        await _next(context);
        _logger.LogInformation($"Response: {response.StatusCode} {responseContent}");
    }

    private async Task<string> ReadRequestBody(HttpRequest request)
    {
        request.EnableBuffering();
        var body = await new StreamReader(request.Body).ReadToEndAsync();
        request.Body.Position = 0;
        return body;
    }

    private async Task<string> ReadResponseBody(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);
        return body;
    }
}