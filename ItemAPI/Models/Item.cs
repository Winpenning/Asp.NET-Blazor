namespace ItemApi.Models;

public class Item
{
    public int Id { get; set; }
    public string Name {get; set;}

    public Item(){}
    public Item(string name){
        this.Name = name;
    }
}