using NodeTest.Entities;

namespace NodeTest.Interfaces;

public interface INodeService
{
    public Task<Node> CreateNodeAsync(string nodeType);
    Task<Node> FindAsync(Guid nodeId);
}
