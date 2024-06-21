using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Text;
using Twitter_Clone.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Twitter_Clone.API.Services;
using TwitterClone.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Read JWT settings from configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");

// JWT Authentication configuration
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"]
    };
});

// Add services to the container
builder.Services.AddSingleton<DatabaseContext>(serviceProvider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    //var logger = serviceProvider.GetRequiredService<ILogger<DatabaseContext>>();
    //return new DatabaseContext(connectionString, logger);
    return new DatabaseContext(builder.Configuration);
});
builder.Services.AddScoped<MemberRepository>();
builder.Services.AddScoped<TweetRepository>();
builder.Services.AddScoped<TestDataSeeder>(); 
builder.Services.AddScoped<IUserService, UserService>();



builder.Services.AddControllers();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TwitterClone.API", Version = "v1" });
});


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Show detailed error pages in development
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TwitterClone.API v1"));
}

app.UseHttpsRedirection();

app.UseCors("AllowAll"); // Enable CORS

app.UseAuthorization();

app.MapControllers();

app.Run();
