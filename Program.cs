using Backend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Enable legacy timestamp behavior
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000") // React app URL
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Add controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



// Enable CORS middleware
app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

// Map controllers
app.MapControllers();

app.Run();
