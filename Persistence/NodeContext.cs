using Microsoft.EntityFrameworkCore;
using NodeTest.Entities;

namespace NodeTest.Persistence;

public class NodeContext : DbContext
{
    public NodeContext(DbContextOptions<NodeContext> options) : base(options)
    {

    }

    public DbSet<Node> Node { get; set; } = null!;
    public DbSet<NodeFolder> NodeFolder { get; set; } = null!;
    public DbSet<NodeFile> NodeFile { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //base.OnModelCreating(modelBuilder);
        _ = modelBuilder.Entity<Node>().HasDiscriminator<string>("EntityType");
        _ = modelBuilder.Entity<NodeFolder>().Property(x => x.FolderName).HasMaxLength(1000);
        _ = modelBuilder.Entity<NodeFile>().Property(x => x.FileName).HasMaxLength(1000);

    }
}
