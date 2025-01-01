using GerPros_Backend_API.Application.Common.Models;
using GerPros_Backend_API.Application.ContactUsForm;

namespace GerPros_Backend_API.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendContactFormAsync(ContactForm requestContactForm, UploadedFile[]? requestFiles);
}
