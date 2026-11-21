using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using ProyectoQ4Backend760.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<FirebaseService>();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TaskService>();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,           // el token debe venir de nuestra app
            ValidateAudience = true,         // y estar dirigido a nuestra app
            ValidateLifetime = true,         // y no estar vencido
            ValidateIssuerSigningKey = true, // y tener una firma valida
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("ProyectoQ4Backend760");
    });
}

app.UseCors("AllowAll");        
app.UseAuthentication();        
app.UseAuthorization();         
app.MapControllers();           

app.Run();