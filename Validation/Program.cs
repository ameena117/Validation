
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Validation.Data;
using Validation.Dtos;
using Validation.Errors;

namespace Validation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
             ?? throw new InvalidOperationException("Connection string"
            + "'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString)
            );

            builder.Services.AddScoped<IValidator<CreateEditProductDto>,CreateEditProductDtoValidation>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.UseExceptionHandler(op => { });

            app.Run();
        }
    }
}
