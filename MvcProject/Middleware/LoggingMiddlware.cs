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
        private readonly string _logFilePath;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;

            string logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            _logFilePath = Path.Combine(logDirectory, "log-file.log");

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var requestBody = await ReadRequestBodyAsync(request);

            // Skip logging request body for HTML content
            if (!IsHtmlContent(request.ContentType))
            {
                string requestLog = $"[{DateTime.UtcNow}] Request: {request.Method} {request.Path}{request.QueryString} Body: {LogInputSanitizer(requestBody)}\n";
                _logger.LogInformation(requestLog);
                await File.AppendAllTextAsync(_logFilePath, requestLog);
            }

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                string errorLog = $"[{DateTime.UtcNow}] Exception: {ex.Message}\n";
                _logger.LogError(ex, errorLog);
                await File.AppendAllTextAsync(_logFilePath, errorLog);
                throw;
            }
            finally
            {
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                // Skip logging response body for HTML content
                if (!IsHtmlContent(context.Response.ContentType))
                {
                    string responseLog = $"[{DateTime.UtcNow}] Response: {context.Response.StatusCode} Body: {LogInputSanitizer(responseText)}\n";
                    _logger.LogInformation(responseLog);
                    await File.AppendAllTextAsync(_logFilePath, responseLog);
                }

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private bool IsHtmlContent(string contentType)
        {
            return contentType?.StartsWith("text/html", StringComparison.OrdinalIgnoreCase) ?? false;
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
