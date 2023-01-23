using PhysicalPersonHandbook.Middlewares;

namespace PhysicalPersonHandbook.Infrastructure.Extensions
{
    public static class RequestExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
