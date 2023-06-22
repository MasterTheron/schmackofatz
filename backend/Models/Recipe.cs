using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;
public class Recipe {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid? Id {get; set;}
    public string? Title { get; set; }
    public DateTime? ChangedAt {get; set;}
    public IEnumerable<Ingredient>? Ingredients {get; set;}
    public IEnumerable<Step>? Steps {get; set;}
    public IEnumerable<Tag>? Tags {get; set;}
}