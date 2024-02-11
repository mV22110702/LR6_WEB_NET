using System.Web.Http;
using LR6_WEB_NET.Models.Dto;

namespace LR6_WEB_NET.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception rawException)
    {
        if (rawException is HttpResponseException exception)
        {
            var reader = new StreamReader(exception.Response.Content.ReadAsStream());
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)exception.Response.StatusCode;
            var responseText = reader.ReadToEnd();
            var responseDto = new ResponseDto<int>
            {
                Description = responseText,
                StatusCode = (int)exception.Response.StatusCode,
                TotalRecords = 0
            };
            await context.Response.WriteAsJsonAsync(responseDto);
        }
    }
}