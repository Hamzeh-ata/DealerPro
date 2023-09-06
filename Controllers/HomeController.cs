using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Text;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    private readonly IFileProvider _fileProvider;

    public HomeController()
    {
        var pagesPath = Path.Combine(Directory.GetCurrentDirectory(), "Pages/home");
        _fileProvider = new PhysicalFileProvider(pagesPath);
    }

    [HttpGet("home")]
    public IActionResult GetHomePage()
    {
        var fileInfo = _fileProvider.GetFileInfo("home.html");

        if (!fileInfo.Exists || fileInfo.IsDirectory)
        {
            return NotFound();
        }

        using (var reader = new StreamReader(fileInfo.CreateReadStream(), Encoding.UTF8))
        {
            var fileContents = reader.ReadToEnd();
            return Content(fileContents, "text/html"); // Adjust the content type as per your file type
        }
    }
}
