using DAL.Models;
using DTO.Gallary;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.IServices
{
    public interface IGallaryServices
    {
        Task<MediaResponse> UploadAsync(IFormFile blob);
        Task<List<MediaResponse>> UploadMultiple(List<IFormFile> blob);

        Task<MediaResponse> DownloadAsync(string blobFilename);

        Task<MediaResponse> DeleteAsync(int id);
        Task<Gallery> GetGalleryByUrl(string url);
        Task<Gallery> GetGalleryById(int id);
    }
}
