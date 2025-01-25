
using System.Text.Json;

namespace MvcProject.Middleware
{
    public class LoggingMiddleware
    {
        private static readonly HashSet<string> SkipNameList = new()
    {
        "Password",
        "password",
        "VCC",
        "apikey",
        "APIKey",
        "secret",
        "Secret"
    };

        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var requestBody = await ReadRequestBodyAsync(request);

            _logger.LogInformation("[{Time}] Request: {Method} {Path}{QueryString} Body: {Body}",
                DateTime.UtcNow,
                request.Method,
                request.Path,
                request.QueryString,
                LogInputSanitizer(requestBody).Replace("\n", " ").Replace("\r", " ").Replace("  ", " ").Trim());

            var originalBodyStream = context.Response.Body;

            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{Time}] An exception occurred.", DateTime.UtcNow);
                throw;
            }
            finally
            {
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                _logger.LogInformation("[{Time}] Response: {StatusCode} Body: {Body}",
                    DateTime.UtcNow,
                    context.Response.StatusCode,
                    LogInputSanitizer(responseText).Replace("\n", " ").Replace("\r", " ").Replace("  ", " ").Trim());

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private string LogInputSanitizer(string input)
        {
            if (!SkipNameList.Any(input.Contains)) return input;

            try
            {
                using var jsonDoc = JsonDocument.Parse(input);
                var root = jsonDoc.RootElement;

                if (root.ValueKind != JsonValueKind.Object) return input;

                var sanitizedObject = new Dictionary<string, JsonElement>();
                var modified = false;

                foreach (var property in root.EnumerateObject())
                {
                    if (SkipNameList.Contains(property.Name))
                    {
                        sanitizedObject[property.Name] = JsonDocument.Parse("\"***\"").RootElement;
                        modified = true;
                    }
                    else
                    {
                        sanitizedObject[property.Name] = property.Value.Clone();
                    }
                }

                return modified ? JsonSerializer.Serialize(sanitizedObject) : input;
            }
            catch (JsonException)
            {
                return input;
            }
        }

        private async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;
            return body;
        }
    }
}
