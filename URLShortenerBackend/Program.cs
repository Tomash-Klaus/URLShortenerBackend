using Microsoft.EntityFrameworkCore;
using URLShortenerBackend.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DbContextEF>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container
builder.Services.AddCors((options) =>
{
    options.AddPolicy(name: "CORS",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
        });
});
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
app.UseAuthorization();

app.MapControllers();

app.Run();
