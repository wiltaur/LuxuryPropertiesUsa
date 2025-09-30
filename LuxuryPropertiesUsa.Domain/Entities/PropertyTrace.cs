namespace LuxuryPropertiesUsa.Domain.Entities;

public partial class PropertyTrace
{
    public int IdPropertyTrace { get; set; }

    public int IdProperty { get; set; }

    public DateTime DateSale { get; set; }

    public string Name { get; set; } = null!;

    public decimal Value { get; set; }

    public decimal? Tax { get; set; }

    public virtual Property IdPropertyNavigation { get; set; } = null!;
}