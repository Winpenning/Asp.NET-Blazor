namespace Testes_do_WASM.Models;

public class Item
{
    public int Id { get; set; }
    public string Name {get; set;}

    public Item(){}
    public Item(string name){
        this.Name = name;
    }
    public Item(int id, string name){
        this.Name = name;
        this.Id = id;
    }
}