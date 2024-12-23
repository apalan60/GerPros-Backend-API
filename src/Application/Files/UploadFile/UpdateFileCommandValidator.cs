namespace GerPros_Backend_API.Application.Files.UploadFile;

public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    public UploadFileCommandValidator()
    {
        RuleFor(v => v.File)
            .NotNull()
            .WithMessage("File is required.");

        RuleFor(v => v.File.Content)
            .NotNull()
            .WithMessage("File content is required.");
        
        RuleFor(v => v.File.FileName)
            .NotNull()
            .WithMessage("File name is required.")
            .NotEmpty()
            .WithMessage("File name is required.");
    }
}
