using Type = Instuments_API.Models.Enumerations.Type;

namespace Instuments_API.ViewModels;

public class ItemViewModel
{
    public string Type { get; set; }
    public string Model { get; set; }
    public string Brand { get; set; }
    public double USDPrice { get; set; }    
}