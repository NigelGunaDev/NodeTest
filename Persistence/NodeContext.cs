using Microsoft.EntityFrameworkCore;
using NodeTest.Entities;

namespace NodeTest.Persistence;

public class NodeContext : DbContext
{
    public NodeContext(DbContextOptions<NodeContext> options) : base(options)
    {

    }

    public DbSet<Node> Node { get; set; } = null!;
    public DbSet<NodeFile> NodeFile { get; set; } = null!;
    public DbSet<NodeFolder> NodeFolder { get; set; } = null!;
    public DbSet<User> User { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //base.OnModelCreating(modelBuilder);
        //_ = modelBuilder.Entity<Node>().HasDiscriminator<string>("EntityType");
        //_ = modelBuilder.Entity<NodeFolder>().Property(x => x.FolderName).HasMaxLength(1000);
        //_ = modelBuilder.Entity<NodeFile>().Property(x => x.FileName).HasMaxLength(1000);

        //_ = modelBuilder.Entity<User>().HasMany(x => x.Node).WithMany();


        _ = modelBuilder.Entity<NodeFile>()
            .HasOne(e => e.BaseNode)
            .WithOne()
            .HasForeignKey<NodeFile>(e => e.BaseNodeId);

        _ = modelBuilder.Entity<NodeFolder>()
            .HasOne(e => e.BaseNode)
            .WithOne()
            .HasForeignKey<NodeFolder>(e => e.BaseNodeId);

        _ = modelBuilder.Entity<Node>().Ignore(x => x.RelatedProps);
    }
}
