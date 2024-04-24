using Instuments_API.Models.Enumerations;
using Type = Instuments_API.Models.Enumerations.Type;

namespace Instuments_API.Models;

public class Item
{
    public int Id { get; set; } // 1
    public Type Type { get; set; } // Instrument, Acessory, SoundBox
    public string Model { get; set; } // STD36
    public string Brand { get; set; } // Shelter
    public double USDPrice { get; set; } // U$ 500 (R$ -> U$)
}