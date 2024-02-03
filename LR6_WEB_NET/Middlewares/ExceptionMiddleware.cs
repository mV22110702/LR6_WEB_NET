using System.Web.Http;

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
            StreamReader reader = new StreamReader(exception.Response.Content.ReadAsStream());
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)exception.Response.StatusCode;
            string responseText = reader.ReadToEnd();
            await context.Response.WriteAsJsonAsync(responseText);

        }
    }
}