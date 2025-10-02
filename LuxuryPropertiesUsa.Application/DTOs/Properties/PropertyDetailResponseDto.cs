#nullable disable

using LuxuryPropertiesUsa.Application.DTOs.General;

namespace LuxuryPropertiesUsa.Application.DTOs.Properties;

public partial class PropertyDetailResponseDto : InfoTableDto
{
    public int? TotalPages { get; set; }
    public int? TotalRecords { get; set; }
    public virtual ICollection<PropertyFilterDto> Properties { get; set; }
}