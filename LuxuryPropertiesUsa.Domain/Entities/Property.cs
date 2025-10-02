namespace LuxuryPropertiesUsa.Domain.Entities;

public partial class Property
{
    public int IdProperty { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public decimal Price { get; set; }

    public int CodeInternal { get; set; }

    public decimal Year { get; set; }

    public int IdOwner { get; set; }

    public virtual Owner IdOwnerNavigation { get; set; } = null!;

    public virtual ICollection<PropertyImage> PropertyImages { get; set; } = [];

    public virtual ICollection<PropertyTrace> PropertyTraces { get; set; } = [];
}