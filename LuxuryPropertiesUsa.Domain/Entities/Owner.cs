namespace LuxuryPropertiesUsa.Domain.Entities;

public partial class Owner
{
    public int IdOwner { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public byte[]? Photo { get; set; }

    public DateTime Birthday { get; set; }

    public virtual ICollection<Property> Properties { get; set; } = [];
}