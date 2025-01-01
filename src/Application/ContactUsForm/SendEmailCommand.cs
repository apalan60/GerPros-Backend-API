using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Application.Common.Models;

namespace GerPros_Backend_API.Application.ContactUsForm;

public class SendEmailCommand : IRequest<Unit>
{
    public required ContactForm ContactForm { get; init; }
    
    public UploadedFile[]? Files { get; init; }
}

public class ContactForm
{
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string CustomerTitle { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CustomerType { get; set; } = string.Empty;
    public string ContactTime { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public string ServiceType { get; set; } = string.Empty;
    public string OtherServiceType { get; set; } = string.Empty;

    public string KnowMethod { get; set; } = string.Empty;
    public string OtherKnowMethod { get; set; } = string.Empty;

    public string Comment { get; set; } = string.Empty;
}

public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, Unit>
{
    private readonly IEmailService _emailService;

    public SendEmailCommandHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task<Unit> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        await _emailService.SendContactFormAsync(request.ContactForm, request.Files);
        return Unit.Value;
    }
}
