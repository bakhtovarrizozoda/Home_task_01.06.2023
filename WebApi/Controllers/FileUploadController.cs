using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("[controller]")]
public class FileUploadController : ControllerBase
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileUploadController(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpPost("UploadFile")]
    public string UploadFile(IFormFile file)
    {
        var currentFolder = _webHostEnvironment.WebRootPath;
        var fullpath = Path.Combine(currentFolder, "images", file.FileName);
        using (var stream = System.IO.File.Create(fullpath))
        {
            file.CopyTo(stream);
        }
        return fullpath;
    }

    [HttpDelete("DeleteFile")]
    public bool DeleteFile(string FileName, string folder)
    {
        var path = Path.Combine(_webHostEnvironment.WebRootPath,folder,FileName);
        if (System.IO.File.Exists(path))
        {
           System.IO.File.Delete(path);
           return true;
        }else{
            return false;
        }
    }

    [HttpPut("UpdateFile")]
    public bool UpdateFile(string FileName, string folder, IFormFile NewFile)
    {
        var  path = Path.Combine(_webHostEnvironment.WebRootPath,folder,FileName);
        if (System.IO.File.Exists(path))
        {
            return false;
        }
        var extension = Path.GetExtension(NewFile.FileName);
        var allowedExtensions = new[] { ".doc", ".docx", ".pdf" }; 
        if (!allowedExtensions.Contains(extension))
        {
            return false; 
        }

        System.IO.File.Delete(path);
        using (var stream = new FileStream(path, FileMode.Create))
        {
            NewFile.CopyToAsync(stream).Wait();
        }
        return true;
    } 

    [HttpGet("GetFile")]
    public string ReadFile(string FileName, string folder)
    {
        var path = Path.Combine(_webHostEnvironment.WebRootPath,folder,FileName);
        if (System.IO.File.Exists(path))
        {
            return System.IO.File.ReadAllText(path);
        }
        else{
            return null;
        }
    }

    [HttpGet("DisplayFiles1")]
    public IActionResult DisplayFiles1(string folder)
    {
        var path = Path.Combine(_webHostEnvironment.WebRootPath, folder);
        var files = Directory.GetFiles(path);
        return Ok(files);
    }
}