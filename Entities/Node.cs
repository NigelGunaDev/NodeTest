namespace NodeTest.Entities;

public class Node : BaseEntity
{
    public Guid? ParentId { get; set; }
    public Guid? ChildId { get; set; }
    public required string EntityType { get; set; }
    public IDictionary<string, object>? RelatedProps { get; set; }
}
