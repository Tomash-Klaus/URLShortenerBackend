using Microsoft.EntityFrameworkCore;
using URLShortenerBackend.Data;
using URLShortenerBackend.Repositories.Implementations;
using URLShortenerBackend.Repositories;
using URLShortenerBackend.Services;
using URLShortenerBackend.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using URLShortenerBackend.Filters;
using URLShortenerBackend.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DbContextEF>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors((options) =>
{
    options.AddPolicy(name: "CORS",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") //  for HTTP 
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
        });
});
// Add Identity Services
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<DbContextEF>()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton(typeof(IUrlShortenerService), typeof(UrlShortenerService));
builder.Services.AddScoped(typeof(IGeneralRepository<>), typeof(GeneralRepository<>));
builder.Services.AddScoped(typeof(IUrlRepository), typeof(UrlRepository));
builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));
builder.Services.AddScoped(typeof(IUrlService), typeof(UrlService));
builder.Services.AddScoped<DeleteBasedOnRoleOrOwnRecordFilterAttribute>();

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = builder.Environment.IsDevelopment();
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWT:Secret") ?? throw new InvalidOperationException("JWT Secret is missing.")))
    };
});

builder.Services.AddAuthorization();
builder.Services.Configure<RouteOptions>(opt => opt.LowercaseUrls = true);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();

}

app.UseCors("CORS");
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<TokenValidationMiddleware>();
app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await DatabaseSeed.SeedAsync(serviceProvider);
}

app.Run();
