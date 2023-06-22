using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Tag {
    [Key]
    public string Value {get; set;}
}