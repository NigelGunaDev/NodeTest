using NodeTest.Entities;
using NodeTest.Interfaces;

namespace NodeTest.Services;

public class NodeService : INodeService
{
    public Node CreateNode()
    {
        var node = new Node
        {
            Id = Guid.NewGuid(),
        };

        return node;
    }
}
