using NodeTest.Entities;
using NodeTest.Interfaces;

namespace NodeTest.Services;

public class NodeFolderService : INodeFolderService
{
    public NodeFolder CreateNodeFolder()
    {
        var nodeFolder = new NodeFolder
        {
            Id = Guid.NewGuid(),
            FolderName = "TestFolder",
        };

        return nodeFolder;
    }
}
