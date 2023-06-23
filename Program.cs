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
        _ = builder.Services.AddScoped<INodeFolderService, NodeFolderService>();
        _ = builder.Services.AddScoped<INodeService, NodeService>();

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

        app.MapFileRoutes();
        app.MapFolderRoutes();
        app.MapNodeRoutes();


        app.Run();
    }


}

public static class FileApi
{
    public static void MapFileRoutes(this IEndpointRouteBuilder app)
    {
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
        }).WithName("GetNodeFile").WithTags("File").WithOpenApi();


        _ = app.MapPost("/file", async (HttpResponse response, HttpContext httpContext) =>
        {
            var dbContext = httpContext.RequestServices.GetService<NodeContext>();
            var nodeFileService = httpContext.RequestServices.GetService<INodeFileService>();
            var nodeFile = dbContext!.NodeFile.Add(nodeFileService!.CreateNodeFile());
            _ = await dbContext.SaveChangesAsync();


            return Results.Ok(nodeFile.Entity.Id);
        }).Produces<string>(200).Produces<NodeFile>(201).WithName("AddNodeFile").WithTags("File").WithOpenApi();
    }
}

public static class FolderApi
{
    public static void MapFolderRoutes(this IEndpointRouteBuilder app)
    {
        _ = app.MapGet("/folder/{id}", async ([FromRoute] string id, HttpResponse response, HttpContext httpContext) =>
        {
            var dbContext = httpContext.RequestServices.GetService<NodeContext>();
            var nodeFolder = await dbContext!.NodeFolder.FindAsync(Guid.Parse(id));
            if (nodeFolder == null)
            {
                httpContext.Response.StatusCode = 404;
                return;
            }
            await httpContext.Response.WriteAsJsonAsync(nodeFolder);
        }).WithName("GetNodeFolder").WithTags("Folder").WithOpenApi();


        _ = app.MapPost("/folder", async (HttpResponse response, HttpContext httpContext) =>
        {
            var dbContext = httpContext.RequestServices.GetService<NodeContext>();
            var nodeFolderService = httpContext.RequestServices.GetService<INodeFolderService>();
            var nodeFile = dbContext!.NodeFolder.Add(nodeFolderService!.CreateNodeFolder());
            _ = await dbContext.SaveChangesAsync();


            return Results.Ok(nodeFile.Entity.Id);
        }).Produces<string>(200).Produces<NodeFile>(201).WithName("AddNodeFolder").WithTags("Folder").WithOpenApi();
    }
}

public static class NodeApi
{
    public static void MapNodeRoutes(this IEndpointRouteBuilder app)
    {
        _ = app.MapGet("/node/{id}", async ([FromRoute] string id, HttpResponse response, HttpContext httpContext) =>
        {
            var dbContext = httpContext.RequestServices.GetService<NodeContext>();
            var node = await dbContext!.Node.FindAsync(Guid.Parse(id));
            if (node == null)
            {
                httpContext.Response.StatusCode = 404;
                return;
            }
            await httpContext.Response.WriteAsJsonAsync(node);
        }).WithName("GetNode").WithTags("Node").WithOpenApi();


        _ = app.MapPost("/node", async (HttpResponse response, HttpContext httpContext) =>
        {
            var dbContext = httpContext.RequestServices.GetService<NodeContext>();
            var nodeService = httpContext.RequestServices.GetService<INodeService>();
            var nodeFile = dbContext!.Node.Add(nodeService!.CreateNode());
            _ = await dbContext.SaveChangesAsync();


            return Results.Ok(nodeFile.Entity.Id);
        }).Produces<string>(200).Produces<NodeFile>(201).WithName("AddNode").WithTags("Node").WithOpenApi();

    }
}
