
using Microsoft.EntityFrameworkCore;
using NodeTest.Persistence;

namespace NodeTest;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<NodeContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("NodeDb")));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<NodeContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            // DbInitializer.Initialize(context);
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapGet("/hello", (HttpContext httpContext) =>
        {

            return "Hello World";
        })
        .WithName("Hello!")
        .WithOpenApi();

        app.Run();
    }
}