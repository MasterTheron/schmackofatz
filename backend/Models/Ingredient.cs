using Microsoft.EntityFrameworkCore;

namespace backend.Models;

public class Ingredient {
    public String? Name {get; set;}
    public Double Amount {get; set;}
    public String? Unit {get; set;}
}