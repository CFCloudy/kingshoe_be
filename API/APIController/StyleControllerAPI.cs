using BUS.IService;
using Microsoft.AspNetCore.Http;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using BUS.Service;
using DTO.models;

namespace API.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    public class StyleControllerAPI : ControllerBase
    {
        private IRepositoryStyle_BUS _repositoryStyle_BUS;
        private readonly IMapper _mapper;
        public StyleControllerAPI(IRepositoryStyle_BUS repositoryStyle_BUS, IMapper mapper)
        {
            _repositoryStyle_BUS = repositoryStyle_BUS;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<List<StyleDTO>> GetStyles()
        {
            var styles = await _repositoryStyle_BUS.GetAll();
            var styleDTOs = new List<StyleDTO>();
            foreach (var item in styles)
            {
                var style = _mapper.Map<DAL.Models.Style, StyleDTO>(item);
                style.Type = "Style";
                styleDTOs.Add(style);
            }
            return styleDTOs;
        }
        [HttpGet("GetStyles")]
        public async Task<IActionResult> GetStyles(bool sort = false, int page = 1)
        {
            var styles = await _repositoryStyle_BUS.GetAll();
            if (sort)
            {
                styles = styles.OrderBy(c => c.DisplayOrder).ToList();
            }
            var styleDTOs = new List<StyleDTO>();
            foreach (var item in styles)
            {
                var style = _mapper.Map<DAL.Models.Style, StyleDTO>(item);
                style.Type = "Style";
                styleDTOs.Add(style);
            }
            const int pageSize = 12;
            if (page < 1)
            {
                page = 1;
            }
            int recsCount = styleDTOs.Count();
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            styleDTOs = styleDTOs.Skip(recSkip)
                .Take(pager.PageSize).ToList();
            return Ok(new { styles = styleDTOs, countPage = Math.Ceiling((decimal)recsCount / 12 )});
        }
        [HttpGet("GetStyle/{id}")]
        public async Task<IActionResult> GetStyle(int id)
        {
            var style = await _repositoryStyle_BUS.Getone(id);
            if (style == null)
            {
                return BadRequest("Không tìm thấy");
            }
            return Ok(style);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateStyleDTO style)
        {
            var styleDAL = _mapper.Map<CreateStyleDTO, Style>(style);
            var result = await _repositoryStyle_BUS.Insert(styleDAL);
            if (result)
            {
                return Ok(new { Status = 200, Message = "Thêm thành công" });
            }
            return BadRequest("Thêm style thất bại");
        }

        [HttpPut("editStyle/{id}")]
        public async Task<IActionResult> Edit(int id, CreateStyleDTO style)
        {
            if (id == 0)
            {
                return BadRequest("Sửa style thất bại");
            }

            var styleDAL = _mapper.Map<CreateStyleDTO, Style>(style);
            styleDAL.Id = id;
            var result = await _repositoryStyle_BUS.Update(styleDAL);
            if (result)
            {
                return Ok(new { Status = 200, Message = "Sửa thành công" });
            }

            return BadRequest("Sửa style thất bại");
        }

        [HttpDelete("DeleteStyle/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var styleDAL = await _repositoryStyle_BUS.Getone(id);
            if (styleDAL == null)
            {
                return BadRequest("Xoá style thất bại");
            }
            var result = await _repositoryStyle_BUS.Delete(styleDAL);
            if (result)
            {
                return Ok(new { Status = 200, Message = "Xoá thành công" });
            }
            return BadRequest("Xoá style thất bại");
        }
    }
}
