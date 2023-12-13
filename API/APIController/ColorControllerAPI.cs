using AutoMapper;
using BUS.IService;
using BUS.Service;
using DAL.Models;
using DTO.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorControllerAPI : ControllerBase
    {
        private IRepositoryColor_BUS _repositoryColor_BUS;
        private readonly IMapper _mapper;
        public ColorControllerAPI(IRepositoryColor_BUS repositoryColor_BUS, IMapper mapper)
        {
            _repositoryColor_BUS = repositoryColor_BUS;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<ColorDTO>> GetColors()
        {
            var colors = await _repositoryColor_BUS.GetAll();
            var colorDTOs = new List<ColorDTO>();
            foreach (var item in colors)
            {
                var color = _mapper.Map<DAL.Models.Color, ColorDTO>(item);
                color.Type = "Color";
                colorDTOs.Add(color);
            }
            return colorDTOs;
        }
        [HttpGet("getColor")]
        public async Task<IActionResult> GetColors(bool sort = false, int page = 1)
        {
            var colors = await _repositoryColor_BUS.GetAll();
            if (sort)
            {
                colors = colors.OrderBy(c => c.DisplayOrder).ToList();
            }
            var colorDTOs = new List<ColorDTO>();
            foreach (var item in colors)
            {
                var color = _mapper.Map<DAL.Models.Color, ColorDTO>(item);
                color.Type = "Color";
                colorDTOs.Add(color);
            }
            const int pageSize = 12;
            if (page < 1)
            {
                page = 1;
            }
            int recsCount = colorDTOs.Count();
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            colorDTOs = colorDTOs.Skip(recSkip)
                .Take(pager.PageSize).ToList();
            return Ok(new { colors = colorDTOs, countPage = Math.Ceiling((decimal)recsCount / 12) });
        }

        [HttpGet("GetColor/{id}")]
        public async Task<IActionResult> GetColor(int id)
        {
            var color = await _repositoryColor_BUS.Getone(id);
            if (color == null)
            {
                return BadRequest("Không tìm thấy");
            }
            return Ok(color);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateColorDTO color)
        {
            var colorDAL = _mapper.Map<CreateColorDTO, Color>(color);
            var result = await _repositoryColor_BUS.Insert(colorDAL);
            if (result)
            {
                return Ok(new { Status = 200, Message = "Thêm thành công" });
            }
            return BadRequest("Thêm color thất bại");
        }

        [HttpPut("editcolor/{id}")]
        public async Task<IActionResult> Edit(int id, CreateColorDTO color)
        {
            if (id == 0)
            {
                return BadRequest("Sửa color thất bại");
            }

            var colorDAL = _mapper.Map<CreateColorDTO, Color>(color);
            colorDAL.Id = id;
            var result = await _repositoryColor_BUS.Update(colorDAL);
            if (result)
            {
                return Ok(new { Status = 200, Message = "Sửa thành công" });
            }

            return BadRequest("Sửa color thất bại");
        }

        [HttpDelete("Deletecolor/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var colorDAL = await _repositoryColor_BUS.Getone(id);
            if (colorDAL == null)
            {
                return BadRequest("Xoá color thất bại");
            }
            var result = await _repositoryColor_BUS.Delete(colorDAL);
            if (result)
            {
                return Ok(new { Status = 200, Message = "Xoá thành công" });
            }
            return BadRequest("Xoá color thất bại");
        }
    }
}
