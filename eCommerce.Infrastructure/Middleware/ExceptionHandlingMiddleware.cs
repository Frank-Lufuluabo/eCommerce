

using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate _next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                await _next(context);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException innException)
                {
                    switch (innException.Number)
                    {
                        case 2627: // Unique constraint violation
                            context.Response.StatusCode = StatusCodes.Status409Conflict;
                            await context.Response.WriteAsync("Unique constraint violation");
                            break;

                        case 515: // Cannot insert null
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync("Cannot insert null");
                            break;

                        case 547: // Foreign key constraint violation
                            context.Response.StatusCode = StatusCodes.Status409Conflict;
                            await context.Response.WriteAsync("Foreign key constraint violation");
                            break;
                        default:
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            await context.Response.WriteAsync("An error occurred while processing your request.");
                            break;
                    }
                }
                else 
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("An error occurred while saving the entity changes.");
                }
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("An error occurred: " + ex.Message);
            }
        }
    }
}
