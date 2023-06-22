namespace NodeTest.Entities;

public class Node : BaseEntity
{
    public Guid Parent { get; set; }
    public Guid Child { get; set; }
    public NodeEntityType Type { get; set; }
}

public enum NodeEntityType
{
    File, Folder, Dossier
}
