using System.IO;
using System.Threading.Tasks;

namespace SmartTourism360.Application.Interfaces
{
    public interface IStorageService
    {
        /// <summary>
        /// Uploads a file stream to MinIO storage and returns the relative storage path.
        /// </summary>
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, string folder = "uploads");

        /// <summary>
        /// Deletes a file from MinIO storage.
        /// </summary>
        Task<bool> DeleteFileAsync(string storagePath);

        /// <summary>
        /// Returns the public URL of the file.
        /// </summary>
        string GetPublicUrl(string storagePath);
    }
}
