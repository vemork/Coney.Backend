using Coney.Backend.Data;
using Coney.Backend.Repositories;
using Coney.Backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("name=LocalConnection"));
builder.Services.AddScoped<CountryRepository, CountryRepository>();
builder.Services.AddScoped<UserService, UserService>();
builder.Services.AddScoped<AuthService, AuthService>();
builder.Services.AddScoped<EmailService, EmailService>();
builder.Services.AddScoped<UserRepository, UserRepository>();
builder.Services.AddScoped<RiffleRepository, RiffleRepository>();
builder.Services.AddScoped<TicketRepository, TicketRepository>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173", "https://devissonv.github.io")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<UrlSettings>(builder.Configuration.GetSection("Urls"));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();