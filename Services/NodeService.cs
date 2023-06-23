using Microsoft.EntityFrameworkCore;
using NodeTest.Entities;
using NodeTest.Interfaces;
using NodeTest.Persistence;

namespace NodeTest.Services;

public class NodeService : INodeService
{
    private readonly NodeContext _context;

    public NodeService(NodeContext context)
    {
        _context = context;
    }

    public async Task<Node?> CreateNodeAsync(string nodeType)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var node = new Node
            {
                Id = Guid.NewGuid(),
                EntityType = nodeType // check if this actually is the name of an Entity
            };

            var newNode = await _context.Node.AddAsync(node);
            _ = await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return newNode.Entity;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
        }
        return null;
    }

    public async Task<Node> FindAsync(Guid nodeId)
    {
        //var node = await _context.Node.FindAsync(nodeId);

        var node = await _context.Node.FirstOrDefaultAsync(e => e.Id == nodeId);
        if (node != null)
        {
            //var entityType = Type.GetType(node.EntityType);
            var entityType = System.AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).First(x => x.Name == node.EntityType);
            var dbSetProperty = _context.GetType().GetProperty(entityType.Name);
            var dbSet = dbSetProperty.GetValue(_context) as IQueryable;

            if (node.EntityType.Equals("NodeFile"))
            {

                var test = dbSet.Cast<NodeFile>(); // THIS IS WHERE I FALL SHORT
                var results = test.Where(x => x.BaseNodeId.Equals(nodeId)).ToList();
                node.RelatedProps = results;
            }

            if (node.EntityType.Equals("NodeFolder"))
            {

                var test = dbSet.Cast<NodeFolder>(); // THIS IS WHERE I FALL SHORT
                var results = test.Where(x => x.BaseNodeId.Equals(nodeId)).ToList();
                node.RelatedProps = results;
            }


            //test.Where("BaseNodeId == @0", node.Id)
            //var results = test.Where(x => x.BaseNodeId.Equals(nodeId)).ToList();
            //node.RelatedProps = results;
            //.ToDynamicList();


            //var entities = dbSet.
            //.Where("BaseEntityId == @0", baseEntity.Id)
            //.ToDynamicList();

            //var dbSet2 = _context.Set(entityType);

            //var objectContext = ((IObjectContextAdapter)_context).ObjectContext;
            //var objectSet = objectContext.CreateObjectSet(entityType);
            //var query = objectSet.Where("it.Id = @id", new ObjectParameter("id", 1));

            //var objectContext = ((IObjectContextAdapter)_context).ObjectContext;
            //var results = objectContext.ExecuteStoreQuery(entityType, "SELECT * FROM MyTable WHERE Id = {0}", 1);

            //var results = _context.Database.SqlQuery(entityType, "SELECT * FROM MyTable WHERE Id = @p0", 1);
        }
        return node;
    }
}
