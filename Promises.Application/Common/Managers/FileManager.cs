using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Promises.Domain.Enums;

namespace Promises.Application.Common.Managers;

public class FileManager
{
    private readonly IHostingEnvironment _hostingEnvironment;

    public FileManager(IHostingEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    public string Upload(IFormFile? file, FileRoot root)
    {
        string uploadFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", root.ToString());

        bool exists = Directory.Exists(uploadFolder);

        if (!exists)
            Directory.CreateDirectory(uploadFolder);

        if (file is not { Length: > 0 })
        {
            switch (root)
            {
                case FileRoot.PersonProfile:
                    return FileRoot.PersonProfile + "/default_person.jpeg";
                case FileRoot.UserProfile:
                    return FileRoot.UserProfile + "/default_user.jpeg";
                case FileRoot.EventPhotos:
                    return FileRoot.EventPhotos + "/default_eventphoto.jpeg";
            }
        }

        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName).ToLower();
        var path = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", root.ToString(), fileName);
        using (var stream = new FileStream(path, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        var url = Path.Combine(root.ToString(), fileName);
        return url;
    }
}