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
    public class StyleController : Controller
    {
        private IRepositoryStyle_BUS _repositoryStyle_BUS;
        private readonly IMapper _mapper;
        public StyleController(IRepositoryStyle_BUS repositoryStyle_BUS, IMapper mapper)
        {
            _repositoryStyle_BUS = repositoryStyle_BUS;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var listStylesDAL = await _repositoryStyle_BUS.GetAll();
            var listStyles = new List<Models.Style>();
            foreach (var item in listStylesDAL)
            {
                listStyles.Add(_mapper.Map<DAL.Models.Style, Models.Style>(item));
            }
            return View(listStyles);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var styleDAL = await _repositoryStyle_BUS.Getone(id.Value);
            if (styleDAL == null)
            {
                return NotFound();
            }
            var style = _mapper.Map<DAL.Models.Style, Models.Style>(styleDAL);
            return View(style);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Style style)
        {
            if (ModelState.IsValid)
            {
                var styleDAL = _mapper.Map<Models.Style, DAL.Models.Style>(style);
                var result = await _repositoryStyle_BUS.Insert(styleDAL);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(style);
        }

        // GET: Style/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var styleDAL = await _repositoryStyle_BUS.Getone(id.Value);
            if (styleDAL == null)
            {
                return NotFound();
            }
            var style = _mapper.Map<DAL.Models.Style, Models.Style>(styleDAL);
            return View(style);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Models.Style style)
        {
            if (id != style.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var styleDAL = _mapper.Map<Models.Style, DAL.Models.Style>(style);
                    var result = await _repositoryStyle_BUS.Update(styleDAL);
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
            return View(style);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var styleDAL = await _repositoryStyle_BUS.Getone(id.Value);
            if (styleDAL == null)
            {
                return NotFound();
            }
            var style = _mapper.Map<DAL.Models.Style, Models.Style>(styleDAL);

            return View(style);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var styleDAL = await _repositoryStyle_BUS.Getone(id);
            if (styleDAL == null)
            {
                return NotFound();
            }
            var result = await _repositoryStyle_BUS.Delete(styleDAL);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            var style = _mapper.Map<DAL.Models.Style, Models.Style>(styleDAL);
            return View(style);
        }
    }
}
