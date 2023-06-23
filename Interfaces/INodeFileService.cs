using NodeTest.Entities;

namespace NodeTest.Interfaces;

public interface INodeFileService
{
    public Task<NodeFile> CreateNodeFileAsync(Guid baseEntity);

}
