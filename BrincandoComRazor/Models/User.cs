namespace BrincandoComRazor.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Image { get; set; }
    public string Slug { get; set; }
    public string Bio { get; set; }
    public IList<Post> Posts { get; set; }
    public IList<Role> Roles { get; set; }
    public User(){}
    public User(int id, string name, string email, string passwordHash, string image, string slug, string bio)
    {
        Id = id;
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Image = image;
        Slug = slug;
        Bio = bio;
        Posts = new List<Post>();
        
        Roles = new List<Role>();
    }
}