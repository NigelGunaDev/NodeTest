using NodeTest.Entities;
using NodeTest.Interfaces;

namespace NodeTest.Services;

public class NodeFileService : INodeFileService
{
    public NodeFile CreateNodeFile()
    {
        var nodeFile = new NodeFile
        {
            Id = Guid.NewGuid(),
            FileName = "TestFile",
        };

        return nodeFile;
    }
}
