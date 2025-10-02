#nullable disable
namespace LuxuryPropertiesUsa.Application.DTOs.Properties;

public partial class PropertyAddDto : PropertyDto
{
    public decimal Price { get; set; }
    public int CodeInternal { get; set; }
    public decimal Year { get; set; }
    public int IdOwner { get; set; }
    public virtual ICollection<PropertyImageDto> Images { get; set; }
}