using AutoMapper;
using BUS.IService;
using BUS.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Shoe.Controllers
{
    public class BrandController : Controller
    {
        private IRepositoryBrand_BUS _repositoryBrand_BUS;
        private readonly IMapper _mapper;
        public BrandController(IRepositoryBrand_BUS repositoryBrand_BUS, IMapper mapper)
        {
            _repositoryBrand_BUS = repositoryBrand_BUS;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var listBrandsDAL = await _repositoryBrand_BUS.GetAll();
            var listBrands = new List<GUI.Models.Brand>();
            foreach (var item in listBrandsDAL)
            {
                listBrands.Add(_mapper.Map<DAL.Models.Brand, GUI.Models.Brand>(item));
            }
            return View(listBrands);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brandDAL = await _repositoryBrand_BUS.Getone(id.Value);
            if (brandDAL == null)
            {
                return NotFound();
            }
            var brand = _mapper.Map<DAL.Models.Brand, GUI.Models.Brand>(brandDAL);
            return View(brand);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GUI.Models.Brand brand)
        {
            if (ModelState.IsValid)
            {
                var brandDAL = _mapper.Map<GUI.Models.Brand, DAL.Models.Brand>(brand);
                var result = await _repositoryBrand_BUS.Insert(brandDAL);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(brand);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brandDAL = await _repositoryBrand_BUS.Getone(id.Value);
            if (brandDAL == null)
            {
                return NotFound();
            }
            var brand = _mapper.Map<DAL.Models.Brand, GUI.Models.Brand>(brandDAL);
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GUI.Models.Brand brand)
        {
            if (id != brand.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var brandDAL = _mapper.Map<GUI.Models.Brand, DAL.Models.Brand>(brand);
                    var result = await _repositoryBrand_BUS.Update(brandDAL);
                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                }
            }
            return View(brand);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var brandDAL = await _repositoryBrand_BUS.Getone(id.Value);
            if (brandDAL == null)
            {
                return NotFound();
            }
            var brand = _mapper.Map<DAL.Models.Brand, GUI.Models.Brand>(brandDAL);
            return View(brand);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brandDAL = await _repositoryBrand_BUS.Getone(id);
            if (brandDAL == null)
            {
                return NotFound();
            }
            var result = await _repositoryBrand_BUS.Delete(brandDAL);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            var brand = _mapper.Map<DAL.Models.Brand, GUI.Models.Brand>(brandDAL);
            return View(brand);

        }
    }
}
