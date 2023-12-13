using BUS.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DTO.models;
using Microsoft.AspNetCore.Authorization;
using BUS.IServices;
using System.Linq;
using Newtonsoft.Json;
using AutoMapper;
using DAL.Models;

namespace API.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoeControllerAPI : ControllerBase
    {
        private IRepositoryShoeVariant_BUS _repositoryShoeVariant_BUS;
        private IRepositoryShoe_BUS repositoryShoe_BUS;
        private IRepositoryBrand_BUS repositoryBrand_BUS;
        private IRepositoryFeature_BUS repositoryFeature_BUS;
        private IRepositoryStyle_BUS repositoryStyle_BUS;
        private readonly IGallaryServices _repogalarry;
        private readonly IMapper _mapper;
        private readonly ShoeStoreContext _context;
        public ShoeControllerAPI(IRepositoryShoe_BUS repositoryShoe_BUS, IRepositoryBrand_BUS repositoryBrand_BUS, IRepositoryFeature_BUS repositoryFeature_BUS, IRepositoryStyle_BUS repositoryStyle_BUS, IRepositoryShoeVariant_BUS repositoryShoeVariant_BUS, IGallaryServices repogalarry, IMapper mapper, ShoeStoreContext context)
        {
            this.repositoryShoe_BUS = repositoryShoe_BUS;
            this.repositoryBrand_BUS = repositoryBrand_BUS;
            this.repositoryFeature_BUS = repositoryFeature_BUS;
            this.repositoryStyle_BUS = repositoryStyle_BUS;
            _repositoryShoeVariant_BUS = repositoryShoeVariant_BUS;
            _repogalarry = repogalarry;
            _mapper = mapper;
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> GetShoes(FilterShoeDTO? filterShoeDTO,bool isClient, int page = 1)
        {
            var shoes = await repositoryShoe_BUS.GetAll();
            if (filterShoeDTO == null)
            {

            }
            if (filterShoeDTO != null)
            {
                if (filterShoeDTO.BrandDTOs != null && filterShoeDTO.BrandDTOs.Any())
                {
                    shoes = shoes.Where(c => filterShoeDTO.BrandDTOs.Select(c => c.Id).Contains(c.BrandGroup == null ? 0 : c.BrandGroup.Value)).ToList();
                }
                if (filterShoeDTO.FeatureDTOs != null && filterShoeDTO.FeatureDTOs.Any())
                {
                    shoes = shoes.Where(c => filterShoeDTO.FeatureDTOs.Select(c => c.Id).Contains(c.Feature == null ? 0 : c.Feature.Value)).ToList();
                }
                if (filterShoeDTO.StyleDTOs != null && filterShoeDTO.StyleDTOs.Any())
                {
                    shoes = shoes.Where(c => filterShoeDTO.StyleDTOs.Select(c => c.Id).Contains(c.StyleGroup == null ? 0 : c.StyleGroup.Value)).ToList();
                }
                if (!string.IsNullOrEmpty(filterShoeDTO.NameShoe))
                {
                    shoes = shoes.Where(c => c.ProductName.Contains(filterShoeDTO.NameShoe)).ToList();
                }
                if (filterShoeDTO.IsAscending.HasValue)
                {
                    if (filterShoeDTO.IsAscending.Value)
                    {
                        shoes = shoes.OrderBy(c => c.DisplayPrice).ToList();
                    }
                }
                if (filterShoeDTO.IsDecrease.HasValue)
                {
                    if (filterShoeDTO.IsDecrease.Value)
                    {
                        shoes = shoes.OrderByDescending(c => c.DisplayPrice).ToList();
                    }
                }
            }

            var shoeDTOs = new List<ShoeDTO>();
            foreach (var shoe in shoes)
            {
                var shoeDTO = new ShoeDTO();
                shoeDTO.Id = shoe.Id;
                shoeDTO.ProductName = shoe.ProductName;
                shoeDTO.IsHotProduct = shoe.IsHotProduct;
                shoeDTO.Description = shoe.Description;
                shoeDTO.Status = shoe.Status;
                shoeDTO.CreatedDate = shoe.CreatedAt;
                shoeDTO.DescriptionDetail = shoe.DescriptionDetail;
                if (shoe.FeatureNavigation != null)
                {
                    shoeDTO.FeatureName = shoe.FeatureNavigation?.FeatureName;
                    shoeDTO.FeatureId = (int)shoe.FeatureNavigation?.Id;
                }
                if (shoe.BrandGroupNavigation != null)
                {
                    shoeDTO.BrandName = shoe.BrandGroupNavigation?.BrandName;
                    shoeDTO.BrandId = (int)shoe?.BrandGroupNavigation?.Id;
                }
                if (shoe.StyleGroupNavigation != null)
                {
                    shoeDTO.StyleName = shoe.StyleGroupNavigation?.StyleName;
                    shoeDTO.StyleId = (int)shoe.StyleGroupNavigation?.Id;
                }
                shoeDTO.DisplayPrice = shoe.DisplayPrice;
                if (shoe.DisplayImage.HasValue)
                {
                    var gallery = await _repogalarry.GetGalleryById(shoe.DisplayImage.Value);
                    if (gallery != null)
                    {
                        shoeDTO.DisplayImage = gallery.Url;
                        shoeDTO.IdImage = shoe.DisplayImage.Value;
                    }
                }
                var shoesVariants = await _repositoryShoeVariant_BUS.GetShoesVariantsByIdShoe(shoe.Id);
                if (!shoesVariants.Any() && isClient)
                {
                    continue;
                }
                var quantity = 0;
                if (shoesVariants != null)
                {
                    if (filterShoeDTO != null)
                    {
                        if (filterShoeDTO.SizeDTOs != null && filterShoeDTO.SizeDTOs.Any())
                        {
                            shoesVariants = shoesVariants.Where(c => filterShoeDTO.SizeDTOs.Select(c => c.Id).Contains(c.Size == null ? 0 : c.Size.Value)).ToList();
                        }
                        if (filterShoeDTO.ColorDTOs != null && filterShoeDTO.ColorDTOs.Any())
                        {
                            shoesVariants = shoesVariants.Where(c => filterShoeDTO.ColorDTOs.Select(c => c.Id).Contains(c.Color == null ? 0 : c.Color.Value)).ToList();
                        }
                    }
                    foreach (var shoesVariant in shoesVariants)
                    {
                        quantity += shoesVariant.Stock == null ? 0 : shoesVariant.Stock.Value;
                        if (!shoeDTO.Available_sizes.Any() || !shoeDTO.Available_sizes.Any(c => c.Contains(shoesVariant.SizeNavigation?.Size1)))
                        {
                            shoeDTO.Available_sizes.Add(shoesVariant.SizeNavigation?.Size1);
                        }
                        var available_color = new Available_color();
                        if (shoesVariant.ImageId != null)
                        {
                            var images = JsonConvert.DeserializeObject<List<int>>(shoesVariant.ImageId);
                            foreach (var item in images)
                            {
                                var gallery = await _repogalarry.GetGalleryById(item);
                                if (gallery != null)
                                {
                                    if (!available_color.ImageVariants.Any())
                                    {
                                        available_color.ImageVariants.Add(new ImageVariant() { Id = gallery.Id, Url = gallery.Url });
                                    }
                                    else if (!available_color.ImageVariants.Select(c => c.Url).Any(c => c.Contains(gallery.Url)))
                                    {
                                        available_color.ImageVariants.Add(new ImageVariant() { Id = gallery.Id, Url = gallery.Url });
                                    }
                                }

                            }
                        }
                        available_color.Name = shoesVariant.ColorNavigation.ColorName;
                        available_color.Rgba = shoesVariant.ColorNavigation.Rgba;
                        if (!shoeDTO.Available_colors.Any() || !shoeDTO.Available_colors.Any(c => c.Name.Contains(available_color.Name)))
                        {
                            shoeDTO.Available_colors.Add(available_color);
                        }
                    }
                }
                shoeDTO.Quantity = quantity;
                shoeDTOs.Add(shoeDTO);
            }
            var totalItem = shoeDTOs.Count;
            const int pageSize = 12;
            if (page < 1)
            {
                page = 1;
            }
            int recsCount = shoeDTOs.Count();
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            shoeDTOs = shoeDTOs.OrderByDescending(x => x.CreatedDate).ToList();
            shoeDTOs = shoeDTOs.Skip(filterShoeDTO.SkipCount)
                .Take(filterShoeDTO.MaxResultCount).ToList();
            return Ok(new { shoes = shoeDTOs, totalItem = totalItem, countPage = Math.Ceiling((decimal)recsCount / 12) });
        }
        [HttpGet("{shoeId}")]
        public async Task<ShoeDTO> GetShoeVariants(int shoeId)
        {
            var shoesVariantDTOs = new List<ShoesVariantDTO>();
            if (shoeId == null)
            {
                return new ShoeDTO();
            }
            var shoe = await repositoryShoe_BUS.Getone(shoeId);
            if (shoe == null)
            {
                return new ShoeDTO();
            }
            var shoeDTO = new ShoeDTO();
            shoeDTO.Id = shoe.Id;
            shoeDTO.ProductName = shoe.ProductName;
            shoeDTO.IsHotProduct = shoe.IsHotProduct;
            shoeDTO.Status = shoe.Status == null ? 1 : shoe.Status;
            shoeDTO.Description = shoe.Description;
            shoeDTO.DescriptionDetail = shoe.DescriptionDetail;
            if (shoe.FeatureNavigation != null)
            {
                shoeDTO.FeatureName = shoe.FeatureNavigation?.FeatureName;
                shoeDTO.FeatureId = (int)shoe.FeatureNavigation?.Id;
            }
            if (shoe.BrandGroupNavigation != null)
            {
                shoeDTO.BrandName = shoe.BrandGroupNavigation?.BrandName;
                shoeDTO.BrandId = (int)shoe?.BrandGroupNavigation?.Id;
            }
            if (shoe.StyleGroupNavigation != null)
            {
                shoeDTO.StyleName = shoe.StyleGroupNavigation?.StyleName;
                shoeDTO.StyleId = (int)shoe.StyleGroupNavigation?.Id;
            }
            shoeDTO.DisplayPrice = shoe.DisplayPrice;
            shoeDTO.OldPrice = shoe.OldPrice;
            if (shoe.DisplayImage.HasValue)
            {
                var gallery = await _repogalarry.GetGalleryById(shoe.DisplayImage.Value);
                if (gallery != null)
                {
                    shoeDTO.DisplayImage = gallery.Url;
                    shoeDTO.IdImage = shoe.DisplayImage.Value;
                }
            }
            var shoesVariants = await _repositoryShoeVariant_BUS.GetShoesVariantsByIdShoe(shoeId);
            if (shoesVariants == null)
            {
                return shoeDTO;
            }
            var quantity = 0;
            foreach (var shoeVariant in shoesVariants)
            {
                quantity += shoeVariant.Stock == null ? 0 : shoeVariant.Stock.Value;
                if (!shoeDTO.Available_sizes.Any() || !shoeDTO.Available_sizes.Any(c => c.Contains(shoeVariant.SizeNavigation?.Size1)))
                {
                    shoeDTO.Available_sizes.Add(shoeVariant.SizeNavigation?.Size1);
                }
                var available_color = new Available_color();
                if (shoeVariant.ImageId != null)
                {
                    var images = JsonConvert.DeserializeObject<List<int>>(shoeVariant.ImageId);
                    foreach (var item in images)
                    {
                        var gallery = await _repogalarry.GetGalleryById(item);
                        if (gallery != null)
                        {
                            if (!available_color.ImageVariants.Any())
                            {
                                available_color.ImageVariants.Add(new ImageVariant() { Id = gallery.Id, Url = gallery.Url });
                            }
                            else if (!available_color.ImageVariants.Select(c => c.Url).Any(c => c.Contains(gallery.Url)))
                            {
                                available_color.ImageVariants.Add(new ImageVariant() { Id = gallery.Id, Url = gallery.Url });
                            }
                        }

                    }
                }
                available_color.Name = shoeVariant.ColorNavigation.ColorName;
                available_color.Rgba = shoeVariant.ColorNavigation.Rgba;
                available_color.IdColor = shoeVariant.ColorNavigation.Id;
                if (!shoeDTO.Available_colors.Any() || !shoeDTO.Available_colors.Any(c => c.Name.Contains(available_color.Name)))
                {
                    shoeDTO.Available_colors.Add(available_color);
                }
                var shoesVariantDTO = new ShoesVariantDTO();
                shoesVariantDTO.Id = shoeVariant.Id;
                shoesVariantDTO.ProductId = shoeVariant.ProductId;
                shoesVariantDTO.VariantName = shoeVariant.VariantName;
                shoesVariantDTO.Description = shoeVariant.Description;
                shoesVariantDTO.Color = shoeVariant.ColorNavigation?.ColorName;
                shoesVariantDTO.ColorId = shoeVariant.ColorNavigation.Id;
                shoesVariantDTO.Rgba = shoeVariant.ColorNavigation.Rgba;
                shoesVariantDTO.Size = shoeVariant.SizeNavigation?.Size1;
                shoesVariantDTO.SizeId = shoeVariant.SizeNavigation.Id;
                shoesVariantDTO.Quantity = shoeVariant.Stock;
                shoesVariantDTO.Price = shoeVariant.DisplayPrice;
                shoesVariantDTOs.Add(shoesVariantDTO);
            }
            shoeDTO.Quantity = quantity;
            shoeDTO.shoesVariantDTOs = shoesVariantDTOs;
            return shoeDTO;
        }

        [HttpPost("add-shoe")]
        public async Task<IActionResult> AddShoe(CreateShoe shoeDTO)
        {
            var shoe = new Shoe();
            shoe.StyleGroup = shoeDTO.StyleId;
            shoe.BrandGroup = shoeDTO.BrandId;
            shoe.Feature = shoeDTO.FeatureId;
            shoe.ProductName = shoeDTO.ProductName;
            shoe.DisplayPrice = shoeDTO.DisplayPrice;
            shoe.OldPrice = shoeDTO.OldPrice;
            shoe.IsHotProduct = shoeDTO.IsHotProduct;
            shoe.DisplayImage = shoeDTO.ImageId;
            shoe.Description = shoeDTO.Description;
            shoe.DescriptionDetail = shoeDTO.DescriptionDetail;
            var result = await repositoryShoe_BUS.InsertReturnId(shoe);
            if (result == -1)
            {
                return BadRequest("sản phẩm đã tồn tại hoặc thêm dữ liệu thiếu");
            }
            return Ok(new { Status = 200, Payload = "thêm thành công", Id = result });
        }
        [HttpPut("update-shoe/{shoeId}")]
        public async Task<IActionResult> UpdateShoe(int shoeId, CreateShoe shoeDTO)
        {
            var shoe = await repositoryShoe_BUS.Getone(shoeId);
            if (shoe == null)
            {
                return BadRequest("không tìm thấy shoe có id này");
            }
            shoe.StyleGroup = shoeDTO.StyleId;
            shoe.BrandGroup = shoeDTO.BrandId;
            shoe.Feature = shoeDTO.FeatureId;
            shoe.ProductName = shoeDTO.ProductName;
            shoe.DisplayPrice = shoeDTO.DisplayPrice;
            shoe.OldPrice = shoeDTO.OldPrice;
            shoe.IsHotProduct = shoeDTO.IsHotProduct;
            shoe.DisplayImage = shoeDTO.ImageId;
            shoe.Description = shoeDTO.Description;
            shoe.DescriptionDetail = shoeDTO.DescriptionDetail;
            var result = await repositoryShoe_BUS.Update(shoe);
            if (!result)
            {
                return BadRequest("sản phẩm sửa đã tồn tại hoặc thêm dữ liệu thiếu");
            }
            var shoesVariants = await _repositoryShoeVariant_BUS.GetShoesVariantsByIdShoe(shoeId);
            foreach (var image in shoeDTO.UpdateImageVariant)
            {
                var shoesVariantsByColorId = shoesVariants.Where(c => c.Color == image.IdColor);
                foreach (var item in shoesVariantsByColorId)
                {
                    var shoevariant = _context.ShoesVariants.FirstOrDefault(c => c.Id == item.Id);
                    if (shoevariant != null)
                    {
                        shoevariant.ImageId = image.IdImages;
                        await _repositoryShoeVariant_BUS.UpdateImage(shoevariant);
                    }
                }
            }

            return Ok(new { Status = 200, Payload = "sửa thành công" });
        }

        [HttpDelete("delete-shoe/{shoeId}")]
        public async Task<IActionResult> DeleteShoe(int shoeId)
        {
            var shoe = await repositoryShoe_BUS.Getone(shoeId);
            if (shoe == null)
            {
                return BadRequest("không tìm thấy shoe có id này");
            }
            if (_context.ShoesVariants.Any(x => x.ProductId == shoeId))
            {
                shoe.Status = 1;
                _context.Shoes.Update(shoe);
                _context.SaveChanges();
                return Ok(new { Status = 200, Payload = "Xoá thành công" });
            }
            var result = await repositoryShoe_BUS.Delete(shoe);
            if (!result)
            {
                return BadRequest("Xoá không thành công");
            }
            return Ok(new { Status = 200, Payload = "Xoá thành công" });
        }

        [HttpPost("add-shoeVariant")]
        public async Task<IActionResult> AddShoeVariant(CreateShoeVariant createShoeVariant)
        {
            var shoe = await repositoryShoe_BUS.Getone(createShoeVariant.ProductId);
            if (shoe == null)
            {
                return BadRequest("không tìm thấy shoe có id này");
            }
            var shoeVariant = new ShoesVariant();
            shoeVariant.ProductId = createShoeVariant.ProductId;
            shoeVariant.Color = createShoeVariant.Color;
            shoeVariant.DisplayPrice = createShoeVariant.Price;
            shoeVariant.Size = createShoeVariant.Size;
            shoeVariant.ImageId = createShoeVariant.ImageId;
            shoeVariant.Stock = createShoeVariant.Stock;

            var result = await _repositoryShoeVariant_BUS.Insert(shoeVariant);
            if (!result)
            {
                return BadRequest("sản phẩm đã tồn tại hoặc thêm dữ liệu thiếu");
            }
            return Ok(new { Status = 200, Payload = "thêm thành công" });
        }
        [HttpPost("add-shoeVariants")]
        public async Task<IActionResult> AddShoeVariants(CreateShoeVariants createShoeVariant)
        {
            if (createShoeVariant == null)
            {
                return BadRequest("không có dữ liệu");
            }
            var shoe = await repositoryShoe_BUS.Getone(createShoeVariant.ProductId);
            if (shoe == null)
            {
                return BadRequest("không tìm thấy shoe có id này");
            }
            foreach (var size in createShoeVariant.Sizes)
            {
                foreach (var color in createShoeVariant.Colors)
                {
                    var shoeVariant = new ShoesVariant();
                    shoeVariant.ProductId = createShoeVariant.ProductId;
                    shoeVariant.Color = color.ColorId;
                    shoeVariant.Description = createShoeVariant.Description;
                    shoeVariant.DisplayPrice = createShoeVariant.Price;
                    shoeVariant.Size = size.SizeId;
                    shoeVariant.ImageId = color.ImageId;
                    shoeVariant.Stock = color.Quantity;

                    var result = await _repositoryShoeVariant_BUS.Insert(shoeVariant);
                    if (!result)
                    {
                        return BadRequest("sản phẩm đã tồn tại hoặc thêm dữ liệu thiếu");
                    }
                }
            }
            return Ok(new { Status = 200, Payload = "thêm thành công" });
        }
        [HttpPut("update-shoeVariants/{shoeVariantId}")]
        public async Task<IActionResult> UpdateShoeVariant(int shoeVariantId, CreateShoeVariant createShoeVariant)
        {
            var shoeVariant = await _repositoryShoeVariant_BUS.Getone(shoeVariantId);
            if (shoeVariant == null)
            {
                return BadRequest("không tìm thấy shoe có id này");
            }
            shoeVariant.Color = createShoeVariant.Color;
            shoeVariant.DisplayPrice = createShoeVariant.Price;
            shoeVariant.Size = createShoeVariant.Size;
            shoeVariant.ImageId = createShoeVariant.ImageId;
            shoeVariant.Stock = createShoeVariant.Stock;
            var result = await _repositoryShoeVariant_BUS.Update(shoeVariant);
            if (!result)
            {
                return BadRequest("sản phẩm sửa đã tồn tại hoặc thêm dữ liệu thiếu");
            }
            return Ok(new { Status = 200, Payload = "sửa thành công" });
        }

        [HttpDelete("delete-shoeVariant/{shoeVariantId}")]
        public async Task<IActionResult> DeleteShoeVarint(int shoeVariantId)
        {
            var shoeVariant = await _repositoryShoeVariant_BUS.Getone(shoeVariantId);
            if (_repositoryShoeVariant_BUS == null)
            {
                return BadRequest("không tìm thấy shoe có id này");
            }
            if (_context.OrderItems.Any(x => x.ProductVariantId == shoeVariantId))
            {
                shoeVariant.Status = 1;
                _context.ShoesVariants.Update(shoeVariant);
                _context.SaveChanges();
                return Ok(new { Status = 200, Payload = "Xoá thành công" });
            }
            var result = await _repositoryShoeVariant_BUS.Delete(shoeVariant);
            if (!result)
            {
                return BadRequest("Xoá không thành công");
            }
            return Ok(new { Status = 200, Payload = "Xoá thành công" });
        }
    }
}
