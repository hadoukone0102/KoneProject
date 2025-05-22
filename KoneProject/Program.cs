using KoneProject.Datas;
using KoneProject.Models;
using KoneProject.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KoneProject.Middleware;
using KoneProject.Authorisations;


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// 1. Controllers & API
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddHttpContextAccessor();

// 2. Custom services (via extension)
services.AddApplicationServices();

// 3. Swagger configuration (via extension)
services.AddSwaggerDocumentation();

// 4. JWT Authentication configuration (via extension)
builder.Services.AddJwtAuthentication(builder.Configuration);

// 5. Database connection
services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 6. IdentityCore configuration
services.AddIdentityCore<UserModel>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// 7. Logging (optional)
var logging = builder.Logging;
logging.AddConsole();
logging.SetMinimumLevel(LogLevel.Debug);

// Build the app
var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
