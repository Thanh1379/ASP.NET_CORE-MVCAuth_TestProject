using Microsoft.AspNetCore.Mvc;
using MVCAuth1.Data;
using MVCAuth1.Models;
using MVCAuth1.Services;

namespace MVCAuth1.Controllers;

//Serve Image As Static File
public class ImagesServeController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IImagesServeControllerConfiguration _config;
    private readonly IWebHostEnvironment _env;
    public ImagesServeController(
        ApplicationDbContext dbContext,
        IImagesServeControllerConfiguration imagesServeControllerConfiguration,
        IWebHostEnvironment env
        )
    {
        _dbContext = dbContext;
        _config = imagesServeControllerConfiguration;
        _env = env;
    }

    [HttpGet]
    public PhysicalFileResult Images()
    {
        string storedPath = _config.GetImageStoredPath() ?? "";
        string physicalPath = _env.ContentRootPath +  storedPath + "/20231114140102.png";
        return PhysicalFile(physicalPath, "image/jpeg");
    }
}