﻿using System.Text.Json;
using log4net;
using System.Text;


namespace MvcProject.Middleware
{
    public class LoggingMiddleware
    {
        private static readonly HashSet<string> SensitiveFields = new(StringComparer.OrdinalIgnoreCase)
        {
            "hash", "secretkey", "merchantid", "password", "pwd", "secret", "token",
            "apikey", "api_key", "key", "__requestverificationtoken", "privatetoken"
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

            if (!request.ContentType?.Contains("application/json") ?? true)
            {
                await _next(context);
                return;
            }

            string requestBody = await ReadRequestBodyAsync(request);
            string sanitizedRequestBody = SanitizeJson(requestBody);

            var requestLog = $"[{DateTime.Now:M/d/yyyy h:mm:ss tt}] Request: {request.Method} {request.Path} Body: {sanitizedRequestBody}";
            LogToFile(requestLog);

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                string sanitizedResponse = SanitizeJson(responseText);
                string logMessage = $"[{DateTime.Now:M/d/yyyy h:mm:ss tt}] Response: {request.Method} {request.Path} | Status: {context.Response.StatusCode} | Body: {sanitizedResponse}";


                LogToFile(logMessage);

                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                var errorLog = $"[{DateTime.Now:M/d/yyyy h:mm:ss tt}] Error: {request.Method} {request.Path} | {SanitizeExceptionMessage(ex.Message)}";
                LogToFile(errorLog, isError: true);
                throw;
            }
        }

        private async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            if (request.ContentLength == null || request.ContentLength == 0)
                return string.Empty;

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

        
        private string SanitizeJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return string.Empty;

            try
            {
                using var jsonDoc = JsonDocument.Parse(json);
                return SanitizeJsonElement(jsonDoc.RootElement);
            }
            catch
            {
                return string.Empty;
            }
        }

        private string SanitizeJsonElement(JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    var obj = element.EnumerateObject()
                        .ToDictionary(
                            prop => prop.Name,
                            prop => SensitiveFields.Contains(prop.Name.ToLower())
                                ? "[REDACTED]"
                                : SanitizeJsonElement(prop.Value)
                        );
                    return JsonSerializer.Serialize(obj, new JsonSerializerOptions
                    {
                        WriteIndented = false,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    });

                case JsonValueKind.Array:
                    var arr = element.EnumerateArray()
                        .Select(item => SanitizeJsonElement(item))
                        .ToList();
                    return JsonSerializer.Serialize(arr);

                default:
                    return JsonSerializer.Serialize(element);
            }
        }

        private string SanitizeExceptionMessage(string message) =>
            SensitiveFields.Aggregate(message, (current, field) =>
                current.Replace(field, "[REDACTED]", StringComparison.OrdinalIgnoreCase));
    }
}
