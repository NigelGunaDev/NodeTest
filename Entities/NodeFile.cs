namespace NodeTest.Entities;

public class NodeFile : BaseEntity
{
    public string? FileName { get; set; }
    public Guid BaseNodeId { get; set; }
    public Node? BaseNode { get; set; }

}

