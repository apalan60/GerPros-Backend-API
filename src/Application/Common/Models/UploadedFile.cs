namespace GerPros_Backend_API.Application.Common.Models;

public record UploadedFile(
    Stream? Content,
    string? FileName,
    string? ContentType
);