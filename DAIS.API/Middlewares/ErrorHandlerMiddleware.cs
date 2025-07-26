namespace DAIS.API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                if (context.Response is HttpResponse response && response.StatusCode == 404)
                {
                    await response.WriteAsJsonAsync(new
                    {
                        message = "Not Found"
                    });

                }
                else if (context.Response is HttpResponse forbiddenResponse && forbiddenResponse.StatusCode == 403)
                {
                    await forbiddenResponse.WriteAsJsonAsync(new
                    {
                        error = "insufficient-permissions",
                        error_discription = "Insufficient permissions to access the resouce",
                        message = "Permission denied"
                    });
                }
                else if (context.Response is HttpResponse unauthorizedResponse && unauthorizedResponse.StatusCode == 401)
                {
                    await unauthorizedResponse.WriteAsJsonAsync(new
                    {
                        message = context.Request.Headers.ContainsKey("Authorization")
                                        ? "Bad credentials"
                                        : "Requires authentication"
                    });
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new
                {
                    error_discription = ex.Message,
                    message = "Internal Server Error."
                });
            }

        }
    }
}
