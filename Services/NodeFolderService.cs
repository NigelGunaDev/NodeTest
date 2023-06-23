using NodeTest.Entities;
using NodeTest.Interfaces;
using NodeTest.Persistence;

namespace NodeTest.Services;

public class NodeFolderService : INodeFolderService
{
    private readonly NodeContext _context;

    public NodeFolderService(NodeContext context)
    {
        _context = context;
    }

    public async Task<NodeFolder?> CreateNodeFolderAsync(Guid baseEntity)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var nodeFolder = new NodeFolder
            {
                Id = Guid.NewGuid(),
                FolderName = "TestFolder",
                BaseNodeId = baseEntity,
            };

            var newNodeFolder = await _context.NodeFolder.AddAsync(nodeFolder);

            var parentNode = await _context.Node.FindAsync(nodeFolder.BaseNodeId);
            parentNode.ChildId = newNodeFolder.Entity.Id;

            _ = await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return newNodeFolder.Entity;

        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
        }

        return null;
    }
}
