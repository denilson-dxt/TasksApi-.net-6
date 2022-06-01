using Microsoft.EntityFrameworkCore;
using Task.Data;
using Task.Models;
using Task.Services;

namespace Task.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async System.Threading.Tasks.Task Invoke(HttpContext context, UserService service, DataContext _context)
    {
        var token = context.Request.Headers.Authorization.LastOrDefault("");
        var userEmail = service.ValidateToken(token);
        if (userEmail != null)
        {
            context.Items["User"] = await _context.Users.FirstAsync(u => u.Email == userEmail);
        }
        
    
        await _next(context);
    }
    
}