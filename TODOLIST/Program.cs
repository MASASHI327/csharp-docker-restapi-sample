using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TODOLIST.Service;
using TODOLIST;
using TODOLIST.Repository;

var builder = WebApplication.CreateBuilder(args);

// ���ϐ��̐ڑ���������擾�i�Ȃ���� appsettings.json ���g�p�j
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
   ?? builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Using Connection String: {connectionString}");

// DbContext ��o�^
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


// JWT�F�ؐݒ�
var configuration = builder.Configuration;
string secretKey = configuration["JwtSettings:SecretKey"]!;
byte[] key = Encoding.UTF8.GetBytes(secretKey);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddScoped<TodolistService>();
builder.Services.AddScoped<TodolistRepository>();
builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Swagger�̐ݒ��ǉ�
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT ����͂��Ă�������"
    };

    c.AddSecurityDefinition("Bearer", jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        Array.Empty<string>()
    }
});
});

//CORS������
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSwagger", builder =>
    {
        builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("Authorization");
    });
});

var app = builder.Build();

// Swagger UI ��L���ɂ���
app.UseSwagger(); // Swagger JSON �G���h�|�C���g
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    options.RoutePrefix = "swagger";  
});// Swagger UI �G���h�|�C���g


app.UseCors("AllowSwagger");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


