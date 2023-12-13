using AutoMapper;
using BUS.IService;
using BUS.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Shoe.Controllers
{
    public class FeatureController : Controller
    {
        private IRepositoryFeature_BUS _repositoryFeature_BUS;
        private readonly IMapper _mapper;
        public FeatureController(IRepositoryFeature_BUS repositoryFeature_BUS, IMapper mapper)
        {
            _repositoryFeature_BUS = repositoryFeature_BUS;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var listFeatureDAL = await _repositoryFeature_BUS.GetAll();
            var listFeatures = new List<GUI.Models.Feature>();
            foreach (var item in listFeatureDAL)
            {
                listFeatures.Add(_mapper.Map<DAL.Models.Feature, GUI.Models.Feature>(item));
            }
            return View(listFeatures);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var featureDAL = await _repositoryFeature_BUS.Getone(id.Value);
            if (featureDAL == null)
            {
                return NotFound();
            }
            var feature = _mapper.Map<DAL.Models.Feature, GUI.Models.Feature>(featureDAL);
            return View(feature);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GUI.Models.Feature feature)
        {
            if (ModelState.IsValid)
            {
                var featureDAL = _mapper.Map<GUI.Models.Feature, DAL.Models.Feature>(feature);
                var result = await _repositoryFeature_BUS.Insert(featureDAL);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(feature);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var featureDAL = await _repositoryFeature_BUS.Getone(id.Value);
            if (featureDAL == null)
            {
                return NotFound();
            }
            var feature = _mapper.Map<DAL.Models.Feature, GUI.Models.Feature>(featureDAL);
            return View(feature);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GUI.Models.Feature feature)
        {
            if (id != feature.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var featureDAL = _mapper.Map<GUI.Models.Feature, DAL.Models.Feature>(feature);
                    var result = await _repositoryFeature_BUS.Update(featureDAL);
                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                }
            }
            return View(feature);
        }

        // GET: Features/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var featureDAL = await _repositoryFeature_BUS.Getone(id.Value);
            if (featureDAL == null)
            {
                return NotFound();
            }
            var feature = _mapper.Map<DAL.Models.Feature, GUI.Models.Feature>(featureDAL);

            return View(feature);
        }

        // POST: Features/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var featureDAL = await _repositoryFeature_BUS.Getone(id);
            if (featureDAL == null)
            {
                return NotFound();
            }
            var result = await _repositoryFeature_BUS.Delete(featureDAL);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            var feature = _mapper.Map<DAL.Models.Feature, GUI.Models.Feature>(featureDAL);

            return RedirectToAction(nameof(Index));
        }
    }
}
