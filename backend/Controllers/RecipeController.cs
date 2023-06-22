using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Services;
using backend.Authorization;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]

public class RecipeController : ControllerBase {

    private readonly IRecipeService _recipeService; 
    private readonly IImageService _imageService;

    public RecipeController(IRecipeService recipeService, IImageService imageService) {
        _recipeService = recipeService;
        _imageService = imageService;
    }   

    [HttpGet(Name = "GetAll")]
    public async Task<ActionResult<IEnumerable<Recipe>>> GetAll() {
        var recipeList = await _recipeService.getAll();
        if(recipeList == null) {
            return NotFound();
        }  else {
            return Ok(recipeList);
        }
    }
    
    [AllowAnonymous]
    [HttpGet("sync/{lastSync}")]
    public async Task<ActionResult<IEnumerable<Recipe>>> SyncRecipes(DateTime lastSync) {
        var recipeList = await _recipeService.getChangedRecipes(lastSync);
        if(recipeList == null) {
            return NotFound();
        }  else {
            return Ok(recipeList);
        }    }

    [HttpPost(Name = "CreateOrUpdateRecipe")]
    public async Task<ActionResult> CreateOrUpdate(Recipe recipe) {
        recipe.ChangedAt = DateTime.UtcNow;
        try
        {
            var _recipe = await _recipeService.createOrUpdate(recipe);
            return Ok(_recipe);
        }
        catch (System.Exception)
        {            
            return BadRequest();
        }
    }
    [HttpPost("{recipeId}/image")]
    public async Task<ActionResult> uploadImage(ICollection<IFormFile> files, Guid recipeId) {

        if (files.Count == 0)
            return BadRequest("No files received from the upload");
        try {                
            var isUploaded = await _imageService.upload(files.ElementAt(0), recipeId.ToString());
            if (isUploaded) {
                await _recipeService.setChanged(recipeId);
                return new AcceptedResult();
            } else {
                return BadRequest("Look like the image couldnt upload to the storage");
            }
        } catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{recipeId}")]
    public async Task<ActionResult<Guid>> delete(Guid recipeId) {
        try
        {
            var id = await _recipeService.delete(recipeId);
            return Ok(id);
        }
        catch (System.Exception)
        {
            
            return BadRequest();
        }
    }
}