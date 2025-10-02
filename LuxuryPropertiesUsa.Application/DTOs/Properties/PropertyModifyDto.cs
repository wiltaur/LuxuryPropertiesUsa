#nullable disable
namespace LuxuryPropertiesUsa.Application.DTOs.Properties;

public partial class PropertyModifyDto : PropertyDto
{
    public int Id { get; set; }
    public decimal? Price { get; set; }
    public int? CodeInternal { get; set; }
    public decimal? Year { get; set; }
    public int? IdOwner { get; set; }
}