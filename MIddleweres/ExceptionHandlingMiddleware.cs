using Microsoft.AspNetCore.Mvc;
using ProvaPub.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace ProvaPub.Middleweres
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NoAvailableNumbersException ex)
            {
                await WriteProblemDetailsAsync(context, ex, HttpStatusCode.UnprocessableEntity, "No available numbers");
            }
            catch (NoAvailableProductsException ex)
            {
                await WriteProblemDetailsAsync(context, ex, HttpStatusCode.UnprocessableEntity, "No available products");
            }
            catch (NoAvailableCustomersException ex)
            {
                await WriteProblemDetailsAsync(context, ex, HttpStatusCode.UnprocessableEntity, "No available customers");
            }
            catch (NoAvailablePaymentServiceException ex)
            {
                await WriteProblemDetailsAsync(context, ex, HttpStatusCode.UnprocessableEntity, "No available customers");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado");

                await WriteProblemDetailsAsync(context, ex, HttpStatusCode.InternalServerError, "Unexpected error");
            }
        }

        private static async Task WriteProblemDetailsAsync(HttpContext context, Exception ex, HttpStatusCode statusCode, string title)
        {
            var problem = new ProblemDetails
            {
                Type = $"https://httpstatuses.com/{(int)statusCode}",
                Title = title,
                Status = (int)statusCode,
                Detail = ex.Message,
                Instance = context.Request.Path
            };

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = problem.Status ?? (int)statusCode;

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problem, options));
        }
    }
}