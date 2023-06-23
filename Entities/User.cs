namespace NodeTest.Entities;

public class User : BaseEntity
{
    public required List<Node> Node { get; set; }
}
