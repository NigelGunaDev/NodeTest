using NodeTest.Entities;

namespace NodeTest.Interfaces;

public interface INodeFolderService
{
    public Task<NodeFolder?> CreateNodeFolderAsync(Guid baseEntity);
}
