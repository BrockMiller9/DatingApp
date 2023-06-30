using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary; // this will be used to store the cloudinary settings
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account
            (
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            ); // this will create a new account with the cloudinary settings

            _cloudinary = new Cloudinary(acc); // this will create a new cloudinary instance with the account settings

        }

        // public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        // {
        //     var uploadResult = new ImageUploadResult(); // this will create a new image upload result

        //     if (file.Length > 0) // this will check if the file length is greater than 0
        //     {
        //         using var stream = file.OpenReadStream(); // this will open the file stream

        //         var uploadParams = new ImageUploadParams // this will create a new image upload params
        //         {
        //             File = new FileDescription(file.FileName, stream), // this will set the file description
        //             Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"), // this will set the transformation
        //             Folder = "da-net7"
        //         };

        //         uploadResult = await _cloudinary.UploadAsync(uploadParams); // this will upload the image to cloudinary
        //     }

        //     return uploadResult; // this will return the upload result
        // }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                    Folder = "da-net7"
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }


        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId); // this will create a new deletion params

            return await _cloudinary.DestroyAsync(deleteParams); // this will delete the photo from cloudinary
        }
    }
}