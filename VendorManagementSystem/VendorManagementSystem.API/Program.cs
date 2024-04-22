using Microsoft.OpenApi.Models;
using VendorManagementSystem.Application;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Infrastructure;

namespace VendorManagementSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                    {
                        options.SuppressModelStateInvalidFilter = true;
                    });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services
                .AddApplication(builder.Configuration)
                .AddInfrastructer(builder.Configuration);

            IConfigurationSection jwtSettings = builder.Configuration.GetSection("Jwt");
            builder.Services.Configure<JwtSettingsDTO>(jwtSettings);

            IConfigurationSection emailSettings = builder.Configuration.GetSection("Email");
            builder.Services.Configure<EmailSettingsDTO>(emailSettings);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            //app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
