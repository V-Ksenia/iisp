namespace WEB_253504_VILKINA.UI.Services.FileService
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile formFile);
        Task DeleteFileAsync(string fileName);
    }
}
