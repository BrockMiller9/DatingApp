using CloudinaryDotNet.Actions;

namespace API.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file); // this will add the photo to our application

        Task<DeletionResult> DeletePhotoAsync(string publicId); // this will delete the photo from our application
    }
}