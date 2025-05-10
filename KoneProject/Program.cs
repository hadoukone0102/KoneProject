using KoneProject.Datas;
using KoneProject.Models;
using KoneProject.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KoneProject.Middleware;

var builder = WebApplication.CreateBuilder(args);

// 1. Controllers & API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 2. Custom services (via extension)
builder.Services.AddApplicationServices();

// 3. Swagger configuration (via extension)
builder.Services.AddSwaggerDocumentation();

// 4. JWT Authentication configuration (via extension)
builder.Services.AddJwtAuthentication(builder.Configuration);

// 5. Database connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 6. IdentityCore configuration
builder.Services.AddIdentityCore<UserModel>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// 7. Logging (optional)
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// Build the app
var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
