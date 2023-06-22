using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NodeTest.Entities;
using NodeTest.Interfaces;
using NodeTest.Persistence;
using NodeTest.Services;

namespace NodeTest;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        _ = builder.Services.AddAuthorization();

        _ = builder.Services.AddEndpointsApiExplorer();
        _ = builder.Services.AddSwaggerGen();

        _ = builder.Services.AddDbContext<NodeContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("NodeDb")));
        _ = builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        _ = builder.Services.AddScoped<INodeFileService, NodeFileService>();

        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


        _ = builder.Services.AddCors(options =>
        {
            options.AddPolicy(MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      _ = policy.AllowAnyOrigin()
                                                          .AllowAnyHeader()
                                                          .AllowAnyMethod();
                                  });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            _ = app.UseSwagger();
            _ = app.UseSwaggerUI();
            _ = app.UseDeveloperExceptionPage();
            _ = app.UseMigrationsEndPoint();
        }

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<NodeContext>();
            _ = context.Database.EnsureDeleted();
            _ = context.Database.EnsureCreated();
            // DbInitializer.Initialize(context);
        }

        _ = app.UseHttpsRedirection();
        _ = app.UseCors(MyAllowSpecificOrigins);
        _ = app.UseAuthorization();

        _ = app.MapGet("/hello", (HttpResponse response, HttpContext httpContext) =>
        {
            return "Hello World";
        }).WithName("Hello").WithOpenApi();


        _ = app.MapGet("/file/{id}", async ([FromRoute] string id, HttpResponse response, HttpContext httpContext) =>
        {
            var dbContext = httpContext.RequestServices.GetService<NodeContext>();
            var nodeFile = await dbContext!.NodeFile.FindAsync(Guid.Parse(id));
            if (nodeFile == null)
            {
                httpContext.Response.StatusCode = 404;
                return;
            }
            await httpContext.Response.WriteAsJsonAsync(nodeFile);
        }).WithName("GetNodeFile").WithOpenApi();


        _ = app.MapPost("/file", async (HttpResponse response, HttpContext httpContext) =>
        {
            var dbContext = httpContext.RequestServices.GetService<NodeContext>();
            var nodeFileService = httpContext.RequestServices.GetService<INodeFileService>();
            var nodeFile = dbContext!.NodeFile.Add(nodeFileService!.CreateNodeFile());
            _ = await dbContext.SaveChangesAsync();


            return Results.CreatedAtRoute("GetNodeFile", nodeFile.Entity);
        }).Produces<string>(200).Produces<NodeFile>(201).WithName("AddNodeFile").WithOpenApi();


        app.Run();
    }
}
