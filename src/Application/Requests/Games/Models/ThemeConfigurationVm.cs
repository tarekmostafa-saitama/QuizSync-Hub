using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Application.Requests.Games.Models;

public class ThemeConfigurationVm
{
    public string MainThemeColor { get; set; }
    public string SecondaryThemeColor { get; set; }
    public string HeaderLogoUrl { get; set; }
    public string BackgroundImageUrl { get; set; }
    public IFormFile HeaderLogo { get; set; }
    public IFormFile BackgroundImage { get; set; }
}