using AutoMapper;
using BUS.IService;
using BUS.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Shoe.Controllers
{
    public class SizeController : Controller
    {
        private IRepositorySize_BUS _repositorySize_BUS;
        private readonly IMapper _mapper;
        public SizeController(IRepositorySize_BUS repositorySize_BUS, IMapper mapper)
        {
            _repositorySize_BUS = repositorySize_BUS;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var listSizeDAL = await _repositorySize_BUS.GetAll();
            var listSizes = new List<GUI.Models.Size>();
            foreach (var item in listSizeDAL)
            {
                listSizes.Add(_mapper.Map<DAL.Models.Size, GUI.Models.Size>(item));
            }
            return View(listSizes);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sizeDAL = await _repositorySize_BUS.Getone(id.Value);
            if (sizeDAL == null)
            {
                return NotFound();
            }
            var size = _mapper.Map<DAL.Models.Size, GUI.Models.Size>(sizeDAL);
            return View(size);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GUI.Models.Size size)
        {
            if (ModelState.IsValid)
            {
                var sizeDAL = _mapper.Map<GUI.Models.Size, DAL.Models.Size>(size);
                var result = await _repositorySize_BUS.Insert(sizeDAL);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(size);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sizeDAL = await _repositorySize_BUS.Getone(id.Value);
            if (sizeDAL == null)
            {
                return NotFound();
            }
            var size = _mapper.Map<DAL.Models.Size, GUI.Models.Size>(sizeDAL);
            return View(size);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GUI.Models.Size size)
        {
            if (id != size.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var sizeDAL = _mapper.Map<GUI.Models.Size, DAL.Models.Size>(size);
                    var result = await _repositorySize_BUS.Update(sizeDAL);
                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return RedirectToAction(nameof(Index));
            }
            return View(size);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sizeDAL = await _repositorySize_BUS.Getone(id.Value);
            if (sizeDAL == null)
            {
                return NotFound();
            }
            var size = _mapper.Map<DAL.Models.Size, GUI.Models.Size>(sizeDAL);

            return View(size);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sizeDAL = await _repositorySize_BUS.Getone(id);
            if (sizeDAL == null)
            {
                return NotFound();
            }
            var result = await _repositorySize_BUS.Delete(sizeDAL);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            var size = _mapper.Map<DAL.Models.Size, GUI.Models.Size>(sizeDAL);
            return View(size);
            
        }
    }
}
