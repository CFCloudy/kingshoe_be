using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using AutoMapper;
using BUS.IService;
using BUS.Service;
using Shoe.Models;

namespace Shoe.Controllers
{
    public class ColorController : Controller
    {
        private IRepositoryColor_BUS _repositoryColor_BUS;
        private readonly IMapper _mapper;
        public ColorController(IRepositoryColor_BUS repositoryColor_BUS, IMapper mapper)
        {
            _repositoryColor_BUS = repositoryColor_BUS;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var listColorsDAL = await _repositoryColor_BUS.GetAll();
            var listColors = new List<Models.Color>();
            foreach (var item in listColorsDAL)
            {
                listColors.Add(_mapper.Map<DAL.Models.Color, Models.Color>(item));
            }
            return View(listColors);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colorDAL = await _repositoryColor_BUS.Getone(id.Value);
            if (colorDAL == null)
            {
                return NotFound();
            }
            var color = _mapper.Map<DAL.Models.Color, Models.Color>(colorDAL);

            return View(color);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Color color)
        {
            if (ModelState.IsValid)
            {
                var colorDAL = _mapper.Map<Models.Color, DAL.Models.Color>(color);
                var result = await _repositoryColor_BUS.Insert(colorDAL);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(color);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colorDAL = await _repositoryColor_BUS.Getone(id.Value);
            if (colorDAL == null)
            {
                return NotFound();
            }
            var color = _mapper.Map<DAL.Models.Color, Models.Color>(colorDAL);
            return View(color);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Models.Color color)
        {
            if (id != color.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var colorDAL = _mapper.Map<Models.Color, DAL.Models.Color>(color);
                    var result = await _repositoryColor_BUS.Update(colorDAL);
                    if (result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {

                }
            }
            return View(color);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colorDAL = await _repositoryColor_BUS.Getone(id.Value);
            if (colorDAL == null)
            {
                return NotFound();
            }
            var color = _mapper.Map<DAL.Models.Color, Models.Color>(colorDAL);

            return View(color);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var colorDAL = await _repositoryColor_BUS.Getone(id);
            if (colorDAL == null)
            {
                return NotFound();
            }
            var result = await _repositoryColor_BUS.Delete(colorDAL);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            var color = _mapper.Map<DAL.Models.Color, Models.Color>(colorDAL);

            return View(color);
        }
    }
}
