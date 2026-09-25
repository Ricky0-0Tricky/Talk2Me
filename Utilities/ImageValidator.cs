using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using static System.Net.Mime.MediaTypeNames;

namespace Talk2Me.Utilities
{
    public static class ImageValidator
    {
        private const long MaxFileSize = 1 * 1024 * 1024; 

        private static readonly string[] AllowedContentTypes =
        {
            "image/jpeg",
            "image/png",
            "image/webp"
        };

        private static readonly string[] AllowedExtensions =
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

        public static async Task<ImageValidationResult> ValidateAndReadAsync(
            IFormFile? file)
        {
            // Check if a file was provided
            if (file == null || file.Length == 0)
            {
                return ImageValidationResult.Invalid(
                    "Please select an image.");
            }

            // Check file size
            if (file.Length > MaxFileSize)
            {
                return ImageValidationResult.Invalid(
                    "The image cannot exceed 5 MB.");
            }

            // Check MIME type
            if (!AllowedContentTypes.Contains(
                file.ContentType.ToLowerInvariant()))
            {
                return ImageValidationResult.Invalid(
                    "Please upload a JPG, PNG, or WebP image.");
            }

            // Check extension
            var extension = Path.GetExtension(file.FileName)
                .ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
            {
                return ImageValidationResult.Invalid(
                    "Please upload a JPG, PNG, or WebP image.");
            }

            byte[] imageData;

            // Read the file into memory
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();
            }

            // Make sure the file is actually a valid image.
            try
            {
                using var image = SixLabors.ImageSharp.Image.Load(imageData);
            }
            catch (UnknownImageFormatException)
            {
                return ImageValidationResult.Invalid(
                    "The uploaded file is not a valid image.");
            }
            catch
            {
                return ImageValidationResult.Invalid(
                    "The uploaded image could not be read.");
            }

            return ImageValidationResult.Valid(imageData);
        }
    }

    public class ImageValidationResult
    {
        public bool IsValid { get; private set; }

        public byte[]? Data { get; private set; }

        public string? Error { get; private set; }

        private ImageValidationResult()
        {
        }

        public static ImageValidationResult Valid(byte[] data)
        {
            return new ImageValidationResult
            {
                IsValid = true,
                Data = data
            };
        }

        public static ImageValidationResult Invalid(string error)
        {
            return new ImageValidationResult
            {
                IsValid = false,
                Error = error
            };
        }
    }
}