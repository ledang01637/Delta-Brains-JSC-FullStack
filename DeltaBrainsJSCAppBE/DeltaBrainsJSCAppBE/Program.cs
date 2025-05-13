using DeltaBrainJSC.DB;
using DeltaBrainsJSCAppBE.DTOs;
using DeltaBrainsJSCAppBE.Hubs;
using DeltaBrainsJSCAppBE.Services.Implements;
using DeltaBrainsJSCAppBE.Services.Interfaces;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

Env.Load();

var builder = WebApplication.CreateBuilder(args);



var databaseUrl = Env.GetString("DATABASE_URL");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuth, AuthService>();
builder.Services.AddScoped<IRole, RoleService>();
builder.Services.AddScoped<IUser, UserService>();


builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlServer(databaseUrl)
);
builder.Services.AddSignalR();

var jwtSettings = new TokenRequiment
{
    SecretKey = Env.GetString("JWT_SECRET_KEY"),
    Issuer = Env.GetString("JWT_ISSUER"),
    Audience = Env.GetString("JWT_AUDIENCE"),
    Subject = Env.GetString("JWT_SUBJECT")
};

var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidAudience = jwtSettings.Audience,
                        ValidIssuer = jwtSettings.Issuer,
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            context.Response.Headers.Add("Token-Validation-Error", context.Exception.Message);
                            return System.Threading.Tasks.Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            if (!context.Response.Headers.ContainsKey("Token-Validation-Error"))
                            {
                                context.Response.Headers.Add("Token-Validation-Error", context.ErrorDescription);
                            }
                            return System.Threading.Tasks.Task.CompletedTask;
                        }
                    };
                });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHub<TaskHub>("/hubs/task");
app.MapHub<NotificationHub>("/hubs/notification");


app.MapControllers();

app.Run();
