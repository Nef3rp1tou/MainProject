using System.Text.Json;
using System.Web;
using log4net;
using System.Text;
using System.Text.RegularExpressions;


namespace MvcProject.Middleware
{
    public class LoggingMiddleware
    {
        private static readonly HashSet<string> SensitiveFields = new(StringComparer.OrdinalIgnoreCase)
        {
            "password", "pwd", "secret", "token", "apikey", "api_key", "vcc", "cvv",
            "cardnumber", "card_number", "ssn", "key", "__RequestVerificationToken", "input.password", "privateToken"
        };

        private static readonly HashSet<string> ExcludedContentTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "text/html",
            "text/css",
            "text/javascript",
            "application/javascript",
            "application/x-javascript"
        };

        private readonly RequestDelegate _next;
        private readonly ILog _logger;
        private readonly string _logFilePath;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
            _logger = LogManager.GetLogger(typeof(LoggingMiddleware));

            string logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            _logFilePath = Path.Combine(logDirectory, "log-file.log");

            if (!Directory.Exists(logDirectory)) Directory.CreateDirectory(logDirectory);
            if (!File.Exists(_logFilePath)) using (File.Create(_logFilePath)) { }
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var method = request.Method;
            var endpoint = request.Path;

            if (ShouldSkipLogging(request.ContentType))
            {
                await _next(context);
                return;
            }

            string requestBody = await ReadRequestBodyAsync(request);
            string sanitizedRequestBody = SanitizeContent(requestBody, request.ContentType);
            string sanitizedQueryString = SanitizeQueryString(request.QueryString.ToString());

            LogToFile($"[{DateTime.UtcNow}] Request: {method} {endpoint}{sanitizedQueryString} Body: {TruncateString(sanitizedRequestBody, 500)}");

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                LogToFile($"[{DateTime.UtcNow}] Exception at {method} {endpoint}: {SanitizeExceptionMessage(ex.Message)}", isError: true);
                throw;
            }
            finally
            {
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                string responseText = string.Empty;
                int statusCode = context.Response.StatusCode;

                if (!ShouldSkipLogging(context.Response.ContentType))
                {
                    responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                }

                string sanitizedResponse = SanitizeContent(responseText, context.Response.ContentType);

                LogToFile($"[{DateTime.UtcNow}] Response: {method} {endpoint} | Status: {statusCode} | Body: {TruncateString(sanitizedResponse, 500)}");

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private bool ShouldSkipLogging(string contentType)
        {
            if (string.IsNullOrEmpty(contentType)) return false;
            return ExcludedContentTypes.Any(excluded => contentType.Contains(excluded, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            if (request.ContentLength == null || request.ContentLength == 0 || !ShouldLogRequestBody(request.ContentType))
                return "";

            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;
            return body;
        }

        private void LogToFile(string message, bool isError = false)
        {
            if (isError) _logger.Error(message);
            else _logger.Info(message);

            File.AppendAllTextAsync(_logFilePath, message + "\n");
        }

        private bool ShouldLogRequestBody(string contentType) =>
            !string.IsNullOrEmpty(contentType) &&
            !ShouldSkipLogging(contentType) &&
            (contentType.Contains("application/json", StringComparison.OrdinalIgnoreCase) ||
             contentType.Contains("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase) ||
             contentType.Contains("text/plain", StringComparison.OrdinalIgnoreCase));

        private string SanitizeQueryString(string queryString)
        {
            if (string.IsNullOrEmpty(queryString)) return queryString;

            var parameters = HttpUtility.ParseQueryString(queryString);
            return "?" + string.Join("&", parameters.AllKeys
                .Where(key => key != null)
                .Select(key => SensitiveFields.Contains(key!) ? $"{key}=[REDACTED]" : $"{key}={parameters[key]}"));
        }

        private string SanitizeContent(string content, string contentType)
        {
            if (string.IsNullOrWhiteSpace(content)) return content;

            return contentType switch
            {
                var type when type?.Contains("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase) == true
                    => SanitizeFormUrlEncoded(content),

                var type when type?.Contains("application/json", StringComparison.OrdinalIgnoreCase) == true
                    => SanitizeJson(content),

                _ => SanitizeGenericContent(content)
            };
        }

        private string SanitizeJson(string json)
        {
            try
            {
                using var jsonDoc = JsonDocument.Parse(json);
                return SanitizeJsonElement(jsonDoc.RootElement);
            }
            catch (JsonException)
            {
                return json;
            }
        }

        private string SanitizeJsonElement(JsonElement element) =>
            element.ValueKind switch
            {
                JsonValueKind.Object => JsonSerializer.Serialize(element.EnumerateObject()
                    .ToDictionary(
                        prop => prop.Name,
                        prop => SensitiveFields.Contains(prop.Name) ? "[REDACTED]" : JsonSerializer.Deserialize<object>(SanitizeJsonElement(prop.Value))
                    )),

                JsonValueKind.Array => JsonSerializer.Serialize(element.EnumerateArray()
                    .Select(item => JsonSerializer.Deserialize<object>(SanitizeJsonElement(item)))),

                _ => JsonSerializer.Serialize(element)
            };

        private string SanitizeFormUrlEncoded(string formData)
        {
            var parameters = HttpUtility.ParseQueryString(formData);
            return string.Join("&", parameters.AllKeys
                .Where(key => key != null)
                .Select(key => SensitiveFields.Contains(key!) ? $"{key}=[REDACTED]" : $"{key}={parameters[key]}"));
        }

        private string SanitizeGenericContent(string content) =>
            SensitiveFields.Aggregate(content, (current, field) =>
                Regex.Replace(current, $@"\b{field}\s*[=:]\s*[""']?[^""'\s&]+[""']?", $"{field}=[REDACTED]", RegexOptions.IgnoreCase));

        private string SanitizeExceptionMessage(string message) =>
            SensitiveFields.Aggregate(message, (current, field) =>
                current.Replace(field, "[REDACTED]", StringComparison.OrdinalIgnoreCase));

        private string TruncateString(string text, int maxLength) =>
            string.IsNullOrEmpty(text) ? text : text.Length > maxLength ? text.Substring(0, maxLength) + "..." : text;
    }
}
