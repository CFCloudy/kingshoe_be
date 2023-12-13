using AutoMapper;
using BUS.IService;
using BUS.Service;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shoe.Models;

namespace Shoe.Controllers
{
    public class ShoeVariantController : Controller
    {
        private IRepositoryShoeVariant_BUS _repositoryShoeVariant_BUS;
        private IRepositoryShoe_BUS _repositoryShoe_BUS;
        private IRepositoryColor_BUS _repositoryColor_BUS;
        private IRepositorySize_BUS _repositorySize_BUS;
        private readonly IMapper _mapper;
        public ShoeVariantController(IRepositoryShoeVariant_BUS repositoryShoeVariant_BUS, IRepositoryColor_BUS repositoryColor_BUS, IRepositorySize_BUS repositorySize_BUS, IMapper mapper, IRepositoryShoe_BUS repositoryShoe_BUS)
        {
            _repositoryShoeVariant_BUS = repositoryShoeVariant_BUS;
            _repositoryColor_BUS = repositoryColor_BUS;
            _repositorySize_BUS = repositorySize_BUS;
            _repositoryShoe_BUS = repositoryShoe_BUS;
            _mapper = mapper;
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
            var listShoesVariant = new List<Models.ShoesVariant>();
            foreach (var item in listShoeVariant)
            {
                listShoesVariant.Add(_mapper.Map<DAL.Models.ShoesVariant, Models.ShoesVariant>(item));
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
            Models.ShoesVariant shoesVariant = new Models.ShoesVariant();
            shoesVariant.ProductId = idShoe;
            shoesVariant.Product = shoe;
            return View(shoesVariant);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.ShoesVariant shoesVariant)
        {
            if (ModelState.IsValid)
            {
                var shoeDAL = _mapper.Map<Models.ShoesVariant, DAL.Models.ShoesVariant>(shoesVariant);
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
            var shoeVariant = _mapper.Map<DAL.Models.ShoesVariant, Models.ShoesVariant>(shoesVariantDAL);
            return View(shoeVariant);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Models.ShoesVariant shoesVariant)
        {
            if (id != shoesVariant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var shoeVariantDAL = _mapper.Map<Models.ShoesVariant, DAL.Models.ShoesVariant>(shoesVariant);
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
                return RedirectToAction(nameof(Index));
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
            var shoeVariant = _mapper.Map<DAL.Models.ShoesVariant, Models.ShoesVariant>(shoesVariantDAL);
            return RedirectToAction("Details", "Shoe", new { id = shoeVariant.ProductId });;
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
            var shoeVariant = _mapper.Map<DAL.Models.ShoesVariant, Models.ShoesVariant>(shoesVariantDAL);
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
            var shoeVariant = _mapper.Map<DAL.Models.ShoesVariant, Models.ShoesVariant>(shoesVariantDAL);
            return View(shoeVariant);
        }
        public async Task<Models.Shoe> GetShoe(int idShoe)
        {
            var shoeDAL = await _repositoryShoe_BUS.Getone(idShoe);
            return _mapper.Map<DAL.Models.Shoe, Models.Shoe>(shoeDAL); ;
        }
        public async Task<List<Models.Color>> GetColors()
        {
            var colors = new List<Models.Color>();
            { new Models.Color(); };
            var colorDAL = await _repositoryColor_BUS.GetAll();
            foreach (var color in colorDAL)
            {
                colors.Add(_mapper.Map<DAL.Models.Color, Models.Color>(color));
            }
            return colors;
        }
        public async Task<List<Models.Size>> GetSizes()
        {
            var sizes = new List<Models.Size>();
            { new Models.Size(); };
            var sizeDAL = await _repositorySize_BUS.GetAll();
            foreach (var size in sizeDAL)
            {
                sizes.Add(_mapper.Map<DAL.Models.Size, Models.Size>(size));
            }
            return sizes;
        }
    }
}
