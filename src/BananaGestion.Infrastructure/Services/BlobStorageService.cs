using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BananaGestion.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.Processing;

namespace BananaGestion.Infrastructure.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly IConfiguration _configuration;
    private readonly string _containerName;

    public BlobStorageService(IConfiguration configuration)
    {
        _configuration = configuration;
        _containerName = _configuration["BlobStorage:ContainerName"] ?? "bananagestion";
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
    {
        var connectionString = _configuration["BlobStorage:ConnectionString"];
        
        if (string.IsNullOrEmpty(connectionString))
        {
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            Directory.CreateDirectory(uploadsPath);
            var filePath = Path.Combine(uploadsPath, $"{Guid.NewGuid()}_{fileName}");
            await using var fs = File.Create(filePath);
            fileStream.Position = 0;
            await fileStream.CopyToAsync(fs);
            return $"/uploads/{Path.GetFileName(filePath)}";
        }

        var blobServiceClient = new BlobServiceClient(connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync();
        
        var blobClient = containerClient.GetBlobClient($"{Guid.NewGuid()}_{fileName}");
        fileStream.Position = 0;
        var options = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
        };
        await blobClient.UploadAsync(fileStream, options);
        
        return blobClient.Uri.ToString();
    }

    public async Task<string> UploadAndConvertToTifAsync(Stream fileStream, string originalFileName)
    {
        using var memoryStream = new MemoryStream();
        
        using (var image = await Image.LoadAsync(fileStream))
        {
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(1920, 1080)
            }));

            var encoder = new TiffEncoder();
            await image.SaveAsync(memoryStream, encoder);
        }

        memoryStream.Position = 0;
        var tifFileName = Path.GetFileNameWithoutExtension(originalFileName) + ".tif";
        return await UploadFileAsync(memoryStream, tifFileName, "image/tiff");
    }

    public async Task DeleteFileAsync(string fileUrl)
    {
        var connectionString = _configuration["BlobStorage:ConnectionString"];
        
        if (string.IsNullOrEmpty(connectionString))
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileUrl.TrimStart('/'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return;
        }

        var blobServiceClient = new BlobServiceClient(connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
        var blobName = fileUrl.Split('/').Last();
        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync();
    }
}
