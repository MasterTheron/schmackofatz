using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;
public interface IRecipeService
{
    Task<Recipe> createOrUpdate(Recipe recipe);
    Task<Guid> delete(Guid recipeId);
    Task<IEnumerable<Recipe>> getAll();
    Task<IEnumerable<Recipe>> getChangedRecipes(DateTime lastSync);
    Task<Recipe> setChanged(Guid recipeId);
}
public class RecipeService : IRecipeService {
    private readonly RecipeContext _context; 

    public RecipeService(RecipeContext recipeContext) {
        _context = recipeContext;
        _context.Database.EnsureCreated();
    }

    private async Task<Recipe> getById(Guid? recipeId) {
        return await _context.Recipes.FirstAsync(e => e.Id == recipeId);
    }

    public async Task<IEnumerable<Recipe>> getAll() {
        return await _context.Recipes.ToListAsync();
    }
    
    public async Task<IEnumerable<Recipe>> getChangedRecipes(DateTime lastSync) {
        return await _context.Recipes.Where(recipe => recipe.ChangedAt > lastSync).ToListAsync();
    }

    public async Task<Recipe> createOrUpdate(Recipe recipe) {
        if(recipe.Id != null) {
            var _recipe = await getById(recipe.Id);
            _recipe.Ingredients = recipe.Ingredients;
            _recipe.Steps = recipe.Steps;
            _recipe.Title = recipe.Title;
            _recipe.Tags = recipe.Tags;
            _recipe.ChangedAt = recipe.ChangedAt;             
        } else {
        _context.Recipes.Add(recipe);
        }
        await _context.SaveChangesAsync();
        return recipe;
    }

    public async Task<Guid> delete(Guid recipeId) {
        var recipe = await getById(recipeId);
        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync();
        return recipeId;
    }
    public async Task<Recipe> setChanged(Guid recipeId) {
        var recipe = await getById(recipeId);
        recipe.ChangedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return recipe;
    }

}