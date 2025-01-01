using GerPros_Backend_API.Application.Common.Models;
using GerPros_Backend_API.Application.ContactUsForm;
using Microsoft.AspNetCore.Mvc;

namespace GerPros_Backend_API.Web.Endpoints;

public class ContactForm : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(SendMail, "Send");
    }

    public async Task SendMail(ISender sender, [FromForm] Application.ContactUsForm.ContactForm formData,
        [FromForm] IFormFileCollection? files)
    {
        var fileModels = new List<UploadedFile>();

        if (files == null)
        {
            await sender.Send(new SendEmailCommand { ContactForm = formData, Files = null });
        }
        else
        {
            foreach (var file in files)
            {
                if (file.Length <= 0)
                {
                    continue;
                }

                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);

                fileModels.Add(new UploadedFile(file.OpenReadStream(),
                    file.FileName,
                    file.ContentType));
            }

            await sender.Send(new SendEmailCommand { ContactForm = formData, Files = fileModels.ToArray() });
        }
    }
}
