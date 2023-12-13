using AutoMapper;
using BUS.IService;
using BUS.Service;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GUI.Models;
using DTO.Gallary;
using Newtonsoft.Json;
using BUS.IServices;
using System.Text.RegularExpressions;
using DTO.models;
using Pager = GUI.Models.Pager;

namespace Shoe.Controllers
{
    public class ShoeVariantController : Controller
    {
        private IRepositoryShoeVariant_BUS _repositoryShoeVariant_BUS;
        private IRepositoryShoe_BUS _repositoryShoe_BUS;
        private IRepositoryColor_BUS _repositoryColor_BUS;
        private IRepositorySize_BUS _repositorySize_BUS;
        private readonly IMapper _mapper;
        private readonly IGallaryServices _repogalarry;
        public ShoeVariantController(IRepositoryShoeVariant_BUS repositoryShoeVariant_BUS, IRepositoryColor_BUS repositoryColor_BUS, IRepositorySize_BUS repositorySize_BUS, IMapper mapper, IRepositoryShoe_BUS repositoryShoe_BUS, IGallaryServices repogalarry)
        {
            _repositoryShoeVariant_BUS = repositoryShoeVariant_BUS;
            _repositoryColor_BUS = repositoryColor_BUS;
            _repositorySize_BUS = repositorySize_BUS;
            _repositoryShoe_BUS = repositoryShoe_BUS;
            _mapper = mapper;
            _repogalarry = repogalarry;
        }
        public async Task<IActionResult> Index(int? idShoe, int page = 1)
        {
            if (idShoe == null)
            {
                return NotFound();
            }
            var shoeDAL = await _repositoryShoe_BUS.Getone(idShoe.Value);
            if (shoeDAL == null)
            {
                return NotFound();
            }
            var listShoeVariant = await _repositoryShoeVariant_BUS.GetShoesVariantsByIdShoe(idShoe.Value);
            var listShoesVariant = new List<GUI.Models.ShoesVariant>();
            foreach (var item in listShoeVariant)
            {
                listShoesVariant.Add(_mapper.Map<DAL.Models.ShoesVariant, GUI.Models.ShoesVariant>(item));
            }
            ViewBag.IdShoe = idShoe;
            const int pageSize = 10;
            if (page < 1)
            {
                page = 1;
            }
            int recsCount = listShoesVariant.Count();
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var data = listShoesVariant.Skip(recSkip)
                .Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(data);
        }
        public async Task<IActionResult> Create(int? idShoe)
        {
            if (idShoe == null)
            {
                return NotFound();
            }
            var shoe = await GetShoe(idShoe.Value);
            var colors = await GetColors();
            var sizes = await GetSizes();
            ViewData["Color"] = new SelectList(colors, "Id", "ColorName");
            //ViewData["ProductId"] = new SelectList(, "Id", "ProductName");
            ViewData["Size"] = new SelectList(sizes, "Id", "Size1");
            GUI.Models.ShoesVariant shoesVariant = new GUI.Models.ShoesVariant();
            shoesVariant.ProductId = idShoe;
            shoesVariant.Product = shoe;
            return View(shoesVariant);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GUI.Models.ShoesVariant shoesVariant, string image)
        {
            if (ModelState.IsValid)
            {
                var objImages = JsonConvert.DeserializeObject<List<DTO.models.ImageDTO>>(image);
                var imageId = new List<int>();
                if (objImages != null)
                {
                    foreach (var item in objImages)
                    {
                        Regex regex = new Regex(@"^[\w/\:.-]+;base64,");
                        item.Base64 = regex.Replace(item.Base64, string.Empty);
                        var byteImage = Convert.FromBase64String(item.Base64);
                        using var stream = new MemoryStream(byteImage);
                        var formFile = new FormFile(stream, 0, stream.Length, item.Type, item.Name);
                        MediaResponse? response = await _repogalarry.UploadAsync(formFile);
                        var gallery = await _repogalarry.GetGalleryByUrl(response.Url);
                        imageId.Add(gallery.Id);
                    }
                }
                var shoeDAL = _mapper.Map<GUI.Models.ShoesVariant, DAL.Models.ShoesVariant>(shoesVariant);
                if (imageId.Any())
                {
                    shoeDAL.ImageId = JsonConvert.SerializeObject(imageId);
                }
                var result = await _repositoryShoeVariant_BUS.Insert(shoeDAL);
                if (result)
                {
                    return RedirectToAction("Details", "Shoe", new { id = shoesVariant.ProductId });
                }
            }
            var colors = await GetColors();
            var sizes = await GetSizes();
            ViewData["Color"] = new SelectList(colors, "Id", "ColorName");
            ViewData["Size"] = new SelectList(sizes, "Id", "Id");
            return View(shoesVariant);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var shoesVariantDAL = await _repositoryShoeVariant_BUS.Getone(id.Value);
            if (shoesVariantDAL == null)
            {
                return NotFound();
            }
            var colors = await GetColors();
            var sizes = await GetSizes();
            ViewData["Color"] = new SelectList(colors, "Id", "ColorName");
            ViewData["Size"] = new SelectList(sizes, "Id", "Id");
            var shoeVariant = _mapper.Map<DAL.Models.ShoesVariant, GUI.Models.ShoesVariant>(shoesVariantDAL);
            if (shoesVariantDAL.ImageId != null)
            {
                var images = JsonConvert.DeserializeObject<List<int>>(shoesVariantDAL.ImageId);
                var imagesURl = new List<string>();
                foreach (var item in images)
                {
                    var gallery = await _repogalarry.GetGalleryById(item);
                    if (gallery != null)
                    {
                        imagesURl.Add(gallery.Url);
                    }
                }

                shoeVariant.ImageURL = JsonConvert.SerializeObject(imagesURl);
            }
            return View(shoeVariant);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GUI.Models.ShoesVariant shoesVariant, string image)
        {
            if (id != shoesVariant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var objImages = JsonConvert.DeserializeObject<List<DTO.models.ImageDTO>>(image);
                    var imageId = new List<int>();
                    if (objImages != null)
                    {
                        foreach (var item in objImages)
                        {
                            Regex regex = new Regex(@"^[\w/\:.-]+;base64,");
                            item.Base64 = regex.Replace(item.Base64, string.Empty);
                            var byteImage = Convert.FromBase64String(item.Base64);
                            using var stream = new MemoryStream(byteImage);
                            var formFile = new FormFile(stream, 0, stream.Length, item.Type, item.Name);
                            MediaResponse? response = await _repogalarry.UploadAsync(formFile);
                            var gallery = await _repogalarry.GetGalleryByUrl(response.Url);
                            imageId.Add(gallery.Id);
                        }
                    }
                    var shoeVariantDAL = _mapper.Map<GUI.Models.ShoesVariant, DAL.Models.ShoesVariant>(shoesVariant);
                    var images = JsonConvert.DeserializeObject<List<int>>(shoesVariant.ImageId);
                    if (imageId.Any())
                    {
                        images.AddRange(imageId);
                        shoeVariantDAL.ImageId = JsonConvert.SerializeObject(images);
                    }
                    var result = await _repositoryShoeVariant_BUS.Update(shoeVariantDAL);
                    if (result)
                    {
                        return RedirectToAction("Details", "Shoe", new { id = shoesVariant.ProductId });
                    }
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;

                }
            }
            var colors = await GetColors();
            var sizes = await GetSizes();
            ViewData["Color"] = new SelectList(colors, "Id", "ColorName");
            ViewData["Size"] = new SelectList(sizes, "Id", "Id");
            return View(shoesVariant);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoesVariantDAL = await _repositoryShoeVariant_BUS.Getone(id.Value);
            if (shoesVariantDAL == null)
            {
                return NotFound();
            }
            var shoeVariant = _mapper.Map<DAL.Models.ShoesVariant, GUI.Models.ShoesVariant>(shoesVariantDAL);
            return RedirectToAction("Details", "Shoe", new { id = shoeVariant.ProductId }); ;
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shoesVariantDAL = await _repositoryShoeVariant_BUS.Getone(id);
            if (shoesVariantDAL == null)
            {
                return NotFound();

            }
            var result = await _repositoryShoeVariant_BUS.Delete(shoesVariantDAL);
            if (result)
            {
                return RedirectToAction(nameof(Index), new { idShoe = shoesVariantDAL.ProductId });
            }
            var shoeVariant = _mapper.Map<DAL.Models.ShoesVariant, GUI.Models.ShoesVariant>(shoesVariantDAL);
            return View(shoeVariant);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoesVariantDAL = await _repositoryShoeVariant_BUS.Getone(id.Value);
            if (shoesVariantDAL == null)
            {
                return NotFound();
            }
            var shoeVariant = _mapper.Map<DAL.Models.ShoesVariant, GUI.Models.ShoesVariant>(shoesVariantDAL);
            return View(shoeVariant);
        }
        public async Task<GUI.Models.Shoe> GetShoe(int idShoe)
        {
            var shoeDAL = await _repositoryShoe_BUS.Getone(idShoe);
            return _mapper.Map<DAL.Models.Shoe, GUI.Models.Shoe>(shoeDAL); ;
        }
        public async Task<List<GUI.Models.Color>> GetColors()
        {
            var colors = new List<GUI.Models.Color>();
            { new GUI.Models.Color(); };
            var colorDAL = await _repositoryColor_BUS.GetAll();
            foreach (var color in colorDAL)
            {
                colors.Add(_mapper.Map<DAL.Models.Color, GUI.Models.Color>(color));
            }
            return colors;
        }
        public async Task<List<GUI.Models.Size>> GetSizes()
        {
            var sizes = new List<GUI.Models.Size>();
            { new GUI.Models.Size(); };
            var sizeDAL = await _repositorySize_BUS.GetAll();
            foreach (var size in sizeDAL)
            {
                sizes.Add(_mapper.Map<DAL.Models.Size, GUI.Models.Size>(size));
            }
            return sizes;
        }
    }
}
