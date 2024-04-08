using System.Runtime.Serialization;

namespace Blog.Models;

[DataContract]
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
        
    public IList<Post> Posts { get; set; }
    public Category(){}

    public Category(int id, string name, string slug)
    {
        Id = id;
        Name = name;
        Slug = slug;
        Posts = new List<Post>();
    }
}