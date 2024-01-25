using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MVCAuth1.Data;
using MVCAuth1.Models;

namespace MVCAuth1.Controllers;

public class ImagesController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    public ImagesController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult List()
    {
        IEnumerable<ImageStore> data = _dbContext.ImageStores;
        foreach(var obj in data)
        {
            obj.ImageDataAsBase64 = Convert.ToBase64String(obj.ImageData);
        }
        return View(data);
    }
    public async Task<IActionResult> ViewDetail(int? id)
    {
        if(id == null)
        {
            return NotFound("Do not specify image id");
        }
        var result = await _dbContext.ImageStores.FindAsync(id);
        if(result == null)
        {
            return NotFound($"Can not find image with id {id}");
        }
        result.ImageDataAsBase64 = Convert.ToBase64String(result.ImageData);
        return View(result);
    }
    public IActionResult Post()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Post(ImageStore data)
    {
        if(data.ImageFile != null)
        {
            byte[] imageBinaryData;
            using(var fs = data.ImageFile.OpenReadStream())
            {
                using var reader = new BinaryReader(fs);
                imageBinaryData = reader.ReadBytes((int)fs.Length);
            }
            data.ImageData = imageBinaryData;
            await _dbContext.ImageStores.AddAsync(data);
            await _dbContext.SaveChangesAsync();
            return Redirect("/Images/List");
        }
        ModelState.AddModelError(string.Empty, "Something went wrong");
        return View();
    }

    public async Task<IActionResult> Delete(int? id)
    {
        var item = await _dbContext.ImageStores.FindAsync(id);
        if(item != null)
        {
            _dbContext.ImageStores.Remove(item);
            await _dbContext.SaveChangesAsync();
        }
        return Redirect("/Images/List");
    }
}