using System.ComponentModel.DataAnnotations.Schema;

namespace NodeTest.Entities;

public abstract class BaseEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
}
