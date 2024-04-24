namespace Instuments_API.Models;

public class Order
{
    public int Id { get; set; } // 1
    public List<Item> Items { get; set; } // drum, drumtick, soundbox
    public Seller Seller { get; set; } // paulo
}