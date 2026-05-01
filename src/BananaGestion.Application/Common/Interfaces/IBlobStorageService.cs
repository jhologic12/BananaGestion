namespace BananaGestion.Application.Common.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType);
    Task<string> UploadAndConvertToTifAsync(Stream fileStream, string originalFileName);
    Task DeleteFileAsync(string fileUrl);
}
