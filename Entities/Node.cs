namespace NodeTest.Entities;

public class Node : BaseEntity
{
    public Guid? Parent { get; set; }
    public Guid? Child { get; set; }
}