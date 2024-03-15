namespace MVCAuth1.Services;

public class ImagesServeControllerConfiguration : IImagesServeControllerConfiguration
{
    private readonly IConfigurationSection _configurationSection;
    public ImagesServeControllerConfiguration(IConfigurationSection section)
    {
        _configurationSection = section;
    }

    public string? GetImageStoredPath()
    {
        return _configurationSection.GetValue<string>("Path");
    }
}