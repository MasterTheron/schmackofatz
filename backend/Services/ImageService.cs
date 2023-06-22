using backend.Config;
using backend.Helpers;
using backend.Models;
using Microsoft.Extensions.Options;

namespace backend.Services;
public interface IImageService
{
    Task<bool> upload(IFormFile formFile, string v);
}
public class ImageService : IImageService{
    private readonly AzureStorageOptions _storageConfig;

    public ImageService(IOptions<AzureStorageOptions> storageConfig) {
        _storageConfig = storageConfig.Value;
    }

    public async Task<bool> upload(IFormFile formFile, String fileName) {

        bool isUploaded = false;
        if (_storageConfig.AccountKey == string.Empty || _storageConfig.AccountName == string.Empty)
                    throw new Exception("sorry, can't retrieve your azure storage details from appsettings.js, make sure that you add azure storage details there");
        if (StorageHelper.IsImage(formFile)) {
            if (formFile.Length > 0) {
            using (Stream stream = formFile.OpenReadStream()) {
                isUploaded = await StorageHelper.UploadFileToStorage(stream, fileName, _storageConfig);
                }
            }
        } else {
            throw new Exception();
        }
        return isUploaded;
    }
}