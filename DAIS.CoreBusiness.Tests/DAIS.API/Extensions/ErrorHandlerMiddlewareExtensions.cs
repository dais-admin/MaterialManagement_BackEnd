using DAIS.API.Middlewares;

namespace DAIS.API.Extensions
{
    public static class ErrorHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder) 
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        
        }
    }
}
