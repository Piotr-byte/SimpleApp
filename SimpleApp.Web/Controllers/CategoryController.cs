﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SimpleApp.Core.Interfaces.Logics;
using SimpleApp.Core.Models.Entities;
using SimpleApp.Web.ViewModels.Categories;

namespace SimpleApp.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryLogic _categoryLogic;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryLogic categoryLogic, IMapper mapper)
        {
            _categoryLogic = categoryLogic;
            _mapper = mapper;
        }

        public async Task<ActionResult> IndexAsync()
        {
            var categories = await _categoryLogic.GetAllActiveAsync();

            var indexViewModel = new IndexViewModel()
            {
                Categories = _mapper.Map<IList<IndexItemViewModel>>(categories.Value)
            };

            return View(indexViewModel);
        }

        public async Task<IActionResult> DetailsAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var getResult = await _categoryLogic.GetByIdAsync(id);

            if (getResult.Success == false)
            {
                return NotFound();
            }

            var categoryViewModel = _mapper.Map<CategoryViewModel>(getResult.Value);

            return View(categoryViewModel);
        }

        public ActionResult Create()
        {
            var categoryViewModel = new CategoryViewModel();
            return View(categoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid == false)
            {
                return View(categoryViewModel);
            }

            var category = _mapper.Map<Category>(categoryViewModel);

            var addResult = await _categoryLogic.AddAsync(category);

            if (addResult.Success == false)
            {
                addResult.AddErrorToModelState(ModelState);
                return View(categoryViewModel);
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> EditAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var getResult = await _categoryLogic.GetByIdAsync(id);

            if (getResult.Success == false)
            {
                return NotFound();
            }

            var categoryViewModel = _mapper.Map<CategoryViewModel>(getResult.Value);
            return View(categoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid == false)
            {
                return View(categoryViewModel);
            }

            var getResult = await _categoryLogic.GetByIdAsync(categoryViewModel.Id);

            if (getResult.Success == false)
            {
                getResult.AddErrorToModelState(ModelState);
                return NotFound();
            }

            _mapper.Map(categoryViewModel, getResult.Value);

            var result = await _categoryLogic.UpdateAsync(getResult.Value);

            if (result.Success == false)
            {
                result.AddErrorToModelState(ModelState);
                return View(categoryViewModel);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var getResult = await _categoryLogic.GetByIdAsync(id);

            if (getResult.Success == false)
            {
                return NotFound();
            }

            var categoryViewModel = _mapper.Map<CategoryViewModel>(getResult.Value);
            return View(categoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<ActionResult> DeletePostAsync(Guid id)
        {
            var getResult = await _categoryLogic.GetByIdAsync(id);

            if (getResult.Success == false)
            {
                return NotFound();
            }

            var deleteResult = await _categoryLogic.DeleteAsync(getResult.Value);

            if (deleteResult.Success == false)
            {
                return BadRequest();
            }

            return RedirectToAction("Index");
        }
    }
}
