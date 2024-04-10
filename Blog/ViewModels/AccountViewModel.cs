using System.ComponentModel.DataAnnotations;
using Blog.Models;

namespace Blog.ViewModels;

public class AccountViewModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Slug { get; set; }
    [Required]
    public string Bio { get; set; }
}