#nullable disable
namespace LuxuryPropertiesUsa.Application.DTOs.Properties;

public partial class PropertyImagesIdDto
{
    public int Id { get; set; }
    public virtual ICollection<PropertyImageDto> Images { get; set; }
}