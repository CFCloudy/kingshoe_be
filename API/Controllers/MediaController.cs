using Azure;
using BUS.IServices;
using BUS.Services;
using DTO.Gallary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DTO.Utils;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IGallaryServices _repogalarry;
        public MediaController(IGallaryServices repogalarry)
        {
            _repogalarry = repogalarry;
        }

        [HttpPost]
        [Route("upload-media")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            MediaResponse? response = await _repogalarry.UploadAsync(file);

            // Check if we got an error
            if (response.Error == true)
            {
                // We got an error during upload, return an error with details to the client
                return StatusCode(StatusCodes.Status500InternalServerError, response.Status);
                //return Ok(new Response<MediaResponse> { Status =response.Status, Message = "Thêm mới địa chỉ thành công." ,Payload=response});
            }
            else
            {
                // Return a success message to the client about successfull upload
                return Ok(new { Status = response.Status, Message = "Thêm mới địa chỉ thành công.", Payload = response });
            }

        }

        [HttpPost]
        [Route("upload-multipemedia")]
        public async Task<IActionResult> UploadMultiple([FromForm] List<IFormFile> file)
        {
            List< MediaResponse >? response = await _repogalarry.UploadMultiple(file);

            // Check if we got an error
            //if (response.Error == true)
            //{
            //    // We got an error during upload, return an error with details to the client
            //    return StatusCode(StatusCodes.Status500InternalServerError, response.Status);
            //    //return Ok(new Response<MediaResponse> { Status =response.Status, Message = "Thêm mới địa chỉ thành công." ,Payload=response});
            //}
            //else
            //{
            //    // Return a success message to the client about successfull upload
            return Ok(new { Status = response, Message = "Thêm mới địa chỉ thành công.", Payload = response });
            //}

        }
    }
}
