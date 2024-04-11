using System.ComponentModel.DataAnnotations;
using Blog.Models;

namespace Blog.ViewModels;

public class AccountViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Name { get; set; }
    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "O email é inválido")]
    public string Email { get; set; }
}