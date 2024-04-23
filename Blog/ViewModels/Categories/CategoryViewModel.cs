using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Categories;

public class CategoryViewModel
{
    [Required(ErrorMessage = "The name is required!")] // define 'Name' como obrigatório
    public string Name { get; set; }

    [Required(ErrorMessage = "The slug is required!")]
    public string Slug { get; set; }
}