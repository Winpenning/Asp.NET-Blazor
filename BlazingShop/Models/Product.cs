using System.ComponentModel.DataAnnotations;

namespace BlazingShop.Models;

public class Product
{
    [Key]
    [Required(ErrorMessage = "Id é obrigatório")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Título é obrigatório")]
    [MaxLength(150, ErrorMessage = "Título deve ter no máximo 150 caracteres")]
    [MinLength(5, ErrorMessage = "Título deve ter no mínimo 5 caracteres")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Preço é obrigatório")]
    [DataType(DataType.Currency)]
    [Range(1, 99999, ErrorMessage = "Preço deve estar entre 1 e 99999")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Categoria é obrigatória")]
    public int CategoryId { get; set; }

    public Category Cate { get; set; } = null!;

}