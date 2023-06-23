using NodeTest.Entities;
using NodeTest.Interfaces;
using NodeTest.Persistence;

namespace NodeTest.Services;

public class NodeFileService : INodeFileService
{
    private readonly NodeContext _context;

    public NodeFileService(NodeContext context)
    {
        _context = context;
    }

    public async Task<NodeFile?> CreateNodeFileAsync(Guid baseEntity)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var nodeFile = new NodeFile
            {
                Id = Guid.NewGuid(),
                FileName = "TestFile",
                BaseNodeId = baseEntity
            };
            var newNodeFile = await _context.NodeFile.AddAsync(nodeFile);

            var parentNode = await _context.Node.FindAsync(nodeFile.BaseNodeId);
            parentNode.ChildId = newNodeFile.Entity.Id;

            _ = await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return newNodeFile.Entity;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
        }

        return null;
    }
}
