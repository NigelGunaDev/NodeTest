namespace NodeTest.Entities;

public class NodeFolder : BaseEntity
{
    public string? FolderName { get; set; }
    public Guid BaseNodeId { get; set; }
    public Node? BaseNode { get; set; }
}
