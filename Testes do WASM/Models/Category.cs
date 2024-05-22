using System.Runtime.Serialization;

namespace Blog.Models;

[DataContract]
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    
    public Category(int id, string name, string slug)
    {
        Id = id;
        Name = name;
        Slug = slug;
    }
    public Category(){}
}