using System.ComponentModel.DataAnnotations;

namespace Shopping.Aggregator.Settings;

public class ApiSettings
{
    [Required]
    public string CatalogUrl { get; set; }

    [Required]
    public string BasketUrl { get; set; }

    [Required]
    public string OrderingUrl { get; set; }
}
