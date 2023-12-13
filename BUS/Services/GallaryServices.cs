using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BUS.IServices;
using DAL.Models;
using DAL.Repositories.Implements;
using DAL.Repositories.Interfaces;
using DTO.Gallary;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services
{
    public class GallaryServices : IGallaryServices
    {
        private readonly string _storageConnectionString;
        private readonly string _storageContainerName;
        private readonly ILogger<GallaryServices> _logger;
        private readonly IGenericRepository<Gallery> _repoGallery;

        public GallaryServices(IConfiguration configuration, ILogger<GallaryServices> logger)
        {
            _storageConnectionString = configuration.GetValue<string>("BlobConnectionString");
            _storageContainerName = configuration.GetValue<string>("BlobContainerName");
            _logger = logger;
            _repoGallery = new GenericRepository<Gallery>();
        }
        public async Task<Gallery> GetGalleryByUrl(string url)
        {
            var image = _repoGallery.GetAllDataQuery().FirstOrDefault(x => x.Url == url);
            return image;
        }
        public async Task<MediaResponse> DeleteAsync(int id)
        {
            var image = _repoGallery.GetAllDataQuery().FirstOrDefault(x => x.Id == id);
            MediaResponse response = new MediaResponse();
            if (image == null)
            {
                response.Error = true;
                response.Status = "Không tìm thấy media";
                return response;
            }
            else
            {
                response.Status = "Xóa media thành công";
                response.Error = false;
                image.Status = true;
                _repoGallery.DeleteDataCommand(image);
                return response;
            }
        }

        public Task<MediaResponse> DownloadAsync(string blobFilename)
        {
            throw new NotImplementedException();
        }

        public async Task<MediaResponse> UploadAsync(IFormFile blob)
        {
            MediaResponse response = new();

            // Get a reference to a container named in appsettings.json and then create it
            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
            //await container.CreateAsync();
            try
            {
                // Get a reference to the blob just uploaded from the API in a container from configuration settings
                BlobClient client = container.GetBlobClient(blob.FileName);

                // Open a stream for the file we want to upload
                await using (Stream? data = blob.OpenReadStream())
                {
                    // Set the Content-Type to "image/png" for PNG files
                    var options = new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders { ContentType = "image/png" }
                    };

                    // Upload the file async with specified Content-Type
                    await client.UploadAsync(data, options);
                }

                // Everything is OK and file got uploaded
                response.Status = $"File {blob.FileName} Uploaded Successfully";
                response.Error = false;
                response.Url = client.Uri.AbsoluteUri;
                response.ContentLength = client.Name;


                var media = new Gallery()
                {
                    Mime = "jpg/",
                    Url = response.Url,
                    ContentLength = "ok",
                    Status = false,
                };
                _repoGallery.AddDataCommand(media);
            }
            // If the file already exists, we catch the exception and do not upload it
            catch (RequestFailedException ex)
               when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                _logger.LogError($"File with name {blob.FileName} already exists in container. Set another name to store the file in the container: '{_storageContainerName}.'");
                response.Status = $"File with name {blob.FileName} already exists. Please use another name to store your file.";
                response.Error = true;
                return response;
            }
            // If we get an unexpected error, we catch it here and return the error message
            catch (RequestFailedException ex)
            {
                // Log error to console and create a new response we can return to the requesting method
                _logger.LogError($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
                response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
                response.Error = true;
                return response;
            }

            // Return the BlobUploadResponse object
            return response;
        }

        public async Task<Gallery> GetGalleryById(int id)
        {
            var image = _repoGallery.GetAllDataQuery().FirstOrDefault(x => x.Id == id);
            return image;
        }

        public async Task<List<MediaResponse>> UploadMultiple(List<IFormFile> blob)
        {
            List< MediaResponse> response = new();

            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
            try
            {
                foreach (IFormFile file in blob) {
                    BlobClient client = container.GetBlobClient(file.FileName);
                    await using (Stream? data = file.OpenReadStream())
                    {
                        var options = new BlobUploadOptions
                        {
                            HttpHeaders = new BlobHttpHeaders { ContentType = "image/png" }
                        };
                        await client.UploadAsync(data, options);
                        var media = new Gallery()
                        {
                            Mime = "jpg/",
                            Url = client.Uri.AbsoluteUri,
                            ContentLength = "ok",
                            Status = false,
                        };
                        var id=_repoGallery.AddDataCommandReturnId(media);
                        var mediaResponse = new MediaResponse
                        {
                            Id = id,
                            Url = media.Url,
                            ContentLength = media.ContentLength,
                            Mime = media.Mime,
                            Status = media.Status==true ? "true" : "false",
                            Error = false
                        };
                        response.Add(mediaResponse);
                    }
                }
                //response.Status = $"File {blob.FileName} Uploaded Successfully";
                //response.Error = false;
                //response.Url = client.Uri.AbsoluteUri;
                //response.ContentLength = client.Name;

               
            }
            catch (RequestFailedException ex)
               when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                return response;
            }
            catch (RequestFailedException ex)
            {
                return response;
            }

            return response;
        }
    }
}
