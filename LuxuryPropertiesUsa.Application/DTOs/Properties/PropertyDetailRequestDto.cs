#nullable disable

using LuxuryPropertiesUsa.Application.DTOs.General;

namespace LuxuryPropertiesUsa.Application.DTOs.Properties;

public partial class PropertyDetailRequestDto : InfoTableDto
{
    public string SearchString { get; set; }
}