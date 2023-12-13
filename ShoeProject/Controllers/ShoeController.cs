using AutoMapper;
using BUS.IService;
using BUS.Service;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Shoe.Models;
using System.Linq;

namespace Shoe.Controllers
{
    public class ShoeController : Controller
    {
        private IRepositoryShoeVariant_BUS _repositoryShoeVariant_BUS;
        private IRepositoryShoe_BUS repositoryShoe_BUS;
        private IRepositoryBrand_BUS repositoryBrand_BUS;
        private IRepositoryFeature_BUS repositoryFeature_BUS;
        private IRepositoryStyle_BUS repositoryStyle_BUS;
        private readonly IMapper _mapper;
        public ShoeController(IRepositoryShoe_BUS repositoryShoe_BUS, IRepositoryBrand_BUS repositoryBrand_BUS, IRepositoryFeature_BUS repositoryFeature_BUS, IRepositoryStyle_BUS repositoryStyle_BUS, IRepositoryShoeVariant_BUS repositoryShoeVariant_BUS, IMapper mapper)
        {
            this.repositoryShoe_BUS = repositoryShoe_BUS;
            this.repositoryBrand_BUS = repositoryBrand_BUS;
            this.repositoryFeature_BUS = repositoryFeature_BUS;
            this.repositoryStyle_BUS = repositoryStyle_BUS;
            _repositoryShoeVariant_BUS = repositoryShoeVariant_BUS;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var listShoe = await repositoryShoe_BUS.GetAll();
            var listShoes = new List<Models.Shoe>();
            foreach (var item in listShoe)
            {
                var shoe = _mapper.Map<DAL.Models.Shoe, Models.Shoe>(item);
                var shoesDAL = await _repositoryShoeVariant_BUS.GetShoesVariantsByIdShoe(shoe.Id);
                foreach (var shoeVariant in shoesDAL)
                {
                    if (shoeVariant.Stock.HasValue)
                    {
                        shoe.Quantity += shoeVariant.Stock.Value;
                    }
                }
                listShoes.Add(shoe);
            }
            const int pageSize = 10;
            if (page < 1)
            {
                page = 1;
            }
            int recsCount = listShoes.Count();
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var data = listShoes.Skip(recSkip)
                .Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;

            var brands = await GetBrands();
            brands.Insert(0, new Models.Brand());
            var features = await GetFeatures();
            features.Insert(0, new Models.Feature());
            var styles = await GetStyles();
            styles.Insert(0, new Models.Style());
            ViewData["BrandGroup"] = new SelectList(brands, "Id", "BrandName");
            ViewData["Feature"] = new SelectList(features, "Id", "FeatureName");
            ViewData["StyleGroup"] = new SelectList(styles, "Id", "StyleName");
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Search(string nameProduct, int? styleGroup, int? brandGroup, int? feature, int page = 1)
        {
            var listShoe = await repositoryShoe_BUS.GetAll();
            var listShoes = new List<Models.Shoe>();
            foreach (var item in listShoe)
            {
                var shoe = _mapper.Map<DAL.Models.Shoe, Models.Shoe>(item);
                var shoesDAL = await _repositoryShoeVariant_BUS.GetShoesVariantsByIdShoe(shoe.Id);
                foreach (var shoeVariant in shoesDAL)
                {
                    if (shoeVariant.Stock.HasValue)
                    {
                        shoe.Quantity += shoeVariant.Stock.Value;
                    }
                }
                listShoes.Add(shoe);
            }
            if (!string.IsNullOrEmpty(nameProduct))
            {
                listShoes = listShoes.Where(c => c.ProductName.Contains(nameProduct)).ToList();
            }
            if (styleGroup.Value != 0)
            {
                 listShoes = listShoes.Where(c => c.StyleGroup == styleGroup.Value).ToList();
            }
            if (brandGroup.Value != 0)
            {
                 listShoes = listShoes.Where(c => c.BrandGroup == brandGroup.Value).ToList();
            }
            if (feature.Value != 0)
            {
                 listShoes = listShoes.Where(c => c.Feature == feature.Value).ToList();
            }
            
            const int pageSize = 10;
            if (page < 1)
            {
                page = 1;
            }
            int recsCount = listShoes.Count();
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var data = listShoes.Skip(recSkip)
                .Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;

            var brands = await GetBrands();
            brands.Insert(0, new Models.Brand());
            var features = await GetFeatures();
            features.Insert(0, new Models.Feature());
            var styles = await GetStyles();
            styles.Insert(0, new Models.Style());
            ViewData["BrandGroup"] = new SelectList(brands, "Id", "BrandName");
            ViewData["Feature"] = new SelectList(features, "Id", "FeatureName");
            ViewData["StyleGroup"] = new SelectList(styles, "Id", "StyleName");
            return View("Index",data);
        }
        public async Task<IActionResult> Create()
        {
            var brands = await GetBrands();
            var features = await GetFeatures();
            var styles = await GetStyles();
            ViewData["BrandGroup"] = new SelectList(brands, "Id", "BrandName");
            ViewData["Feature"] = new SelectList(features, "Id", "FeatureName");
            ViewData["StyleGroup"] = new SelectList(styles, "Id", "StyleName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Shoe shoe)
        {
            if (ModelState.IsValid)
            {
                var shoeDAL = _mapper.Map<Models.Shoe, DAL.Models.Shoe>(shoe);
                var result = await repositoryShoe_BUS.Insert(shoeDAL);
                if (!result)
                {
                    return View(shoe);
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoeDAL = await repositoryShoe_BUS.Getone(id.Value);
            if (shoeDAL == null)
            {
                return NotFound();
            }
            var brands = await GetBrands();
            var features = await GetFeatures();
            var styles = await GetStyles();
            ViewData["BrandGroup"] = new SelectList(brands, "Id", "BrandName");
            ViewData["Feature"] = new SelectList(features, "Id", "FeatureName");
            ViewData["StyleGroup"] = new SelectList(styles, "Id", "StyleName");
            var shoe = _mapper.Map<DAL.Models.Shoe, Models.Shoe>(shoeDAL);
            return View(shoe);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Models.Shoe shoe)
        {
            if (id != shoe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var shoeDAL = _mapper.Map<Models.Shoe, DAL.Models.Shoe>(shoe);
                    var result = await repositoryShoe_BUS.Update(shoeDAL);
                    if (!result)
                    {
                        return View(shoe);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!ShoeExists(shoe.Id))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
                return RedirectToAction(nameof(Index));
            }
            var brands = await GetBrands();
            var features = await GetFeatures();
            var styles = await GetStyles();
            ViewData["BrandGroup"] = new SelectList(brands, "Id", "BrandName");
            ViewData["Feature"] = new SelectList(features, "Id", "FeatureName");
            ViewData["StyleGroup"] = new SelectList(styles, "Id", "StyleName");
            return View(shoe);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoeDAL = await repositoryShoe_BUS.Getone(id.Value);
            if (shoeDAL == null)
            {
                return NotFound();
            }
            var shoe = _mapper.Map<DAL.Models.Shoe, Models.Shoe>(shoeDAL);
            return View(shoe);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shoeDAL = await repositoryShoe_BUS.Getone(id);
            if (shoeDAL == null)
            {
                return NotFound();
            }

            var result = await repositoryShoe_BUS.Delete(shoeDAL);
            if (!result)
            {
                var shoe = _mapper.Map<DAL.Models.Shoe, Models.Shoe>(shoeDAL);
                return View(shoe);
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id, int page = 1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoeDAL = await repositoryShoe_BUS.Getone(id.Value);
            if (shoeDAL == null)
            {
                return NotFound();
            }
            var shoe = _mapper.Map<DAL.Models.Shoe, Models.Shoe>(shoeDAL);
            var listShoeVariant = await _repositoryShoeVariant_BUS.GetShoesVariantsByIdShoe(id.Value);
            var listShoesVariant = new List<Models.ShoesVariant>();
            foreach (var item in listShoeVariant)
            {
                listShoesVariant.Add(_mapper.Map<DAL.Models.ShoesVariant, Models.ShoesVariant>(item));
            }
            ViewBag.IdShoe = id.Value;
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
            ViewData["listShoesVariant"] = data;
            return View(shoe);
        }
        public async Task<List<Models.Brand>> GetBrands()
        {
            var brands = new List<Models.Brand>();
            { new Models.Brand(); };
            var brandDAL = await repositoryBrand_BUS.GetAll();
            foreach (var brand in brandDAL)
            {
                brands.Add(_mapper.Map<DAL.Models.Brand, Models.Brand>(brand));
            }
            return brands;
        }
        public async Task<List<Models.Feature>> GetFeatures()
        {
            var features = new List<Models.Feature>();
            { new Models.Feature(); };
            var featureDAL = await repositoryFeature_BUS.GetAll();
            foreach (var feature in featureDAL)
            {
                features.Add(_mapper.Map<DAL.Models.Feature, Models.Feature>(feature));
            }
            return features;
        }
        public async Task<List<Models.Style>> GetStyles()
        {
            var styles = new List<Models.Style>();
            { new Models.Style(); };
            var styleDAL = await repositoryStyle_BUS.GetAll();
            foreach (var style in styleDAL)
            {
                styles.Add(_mapper.Map<DAL.Models.Style, Models.Style>(style));
            }
            return styles;
        }
    }
}
