﻿using Microsoft.AspNetCore.Mvc;
using SimpleApp.Core.Models;
using System;
using System.Linq;
using SimpleApp.Web.ViewModels;
using SimpleApp.Core.Interfaces.Logics;
using SimpleApp.Web.ViewModels.Products;
using AutoMapper;

namespace SimpleApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductLogic _productLogic;
        private readonly ICategoryLogic _categoryLogic;
        private readonly IMapper _mapper;

        public ProductController(IProductLogic productLogic, ICategoryLogic categoryLogic, IMapper mapper)
        {
            _productLogic = productLogic;
            _categoryLogic = categoryLogic;
            _mapper = mapper;

        }
        // GET: ProductController1
        public ActionResult Index()
        {
            var products = _productLogic.GetAllActive();
            var indexViewModel = _mapper.Map<IndexItemViewModel>(products);
           
            return View(indexViewModel);
        }

        // GET: ProductController1/Details/5
        public ActionResult Details(Guid id)
        {
            if(id == Guid.Empty)
            {
                return NotFound();
            }
            var getResult = _productLogic.GetById(id);
            if(getResult.Success == false)
            {
                return NotFound();
            }
            var productViewModel = _mapper.Map<ProductViewModel>(getResult);
             
            return View(productViewModel);
        }

        // GET: ProductController1/Create
        public ActionResult Create()
        {
            var productViewModel = new ProductViewModel();
            Supply(productViewModel);
            return View(productViewModel);
        }

        // POST: ProductController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid == false)
            {
                Supply(productViewModel);
                return View(productViewModel);
            }
            var getResultCategory = _categoryLogic.GetById(productViewModel.Category);
            if (getResultCategory.Success == false)
            {
                return NotFound();
            }


            var product = _mapper.Map<Product>(productViewModel);

            var addProduct =_productLogic.Add(product);
            if (addProduct.Success == false)
            {
                return View(productViewModel);
            }

            return RedirectToAction("Index");
        }

        // GET: ProductController1/Edit/5
        public ActionResult Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }
            var getResult = _productLogic.GetById(id);
            
            if (getResult.Success == false)
            {
                return NotFound();
            }
            var productViewModel = _mapper.Map<ProductViewModel>(getResult);

            Supply(productViewModel);

            return View(productViewModel);
        }

        // POST: ProductController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel productViewModel)
        {

            if (ModelState.IsValid == false)
            {
                Supply(productViewModel);
                return View(productViewModel);
            }

            var getResult = _productLogic.GetById(productViewModel.Id);
            if (getResult.Success == false)
            {
                return NotFound();
            }
            var getResultCategory = _categoryLogic.GetById(productViewModel.Category);
            if (getResultCategory.Success == false)
            {
                return NotFound();
            }

            var productViewModels = _mapper.Map<ProductViewModel>(getResult);

            var updateResult = _productLogic.Update(getResult.Value);
            if(updateResult.Success == false)
            {
                return BadRequest();
            }
            
            return RedirectToAction("Index");
        }

        // GET: ProductController1/Delete/5
        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var getResult = _productLogic.GetById(id);
            if (getResult.Success == false)
            {
                return NotFound();
            }
            var productViewModel = _mapper.Map<ProductViewModel>(getResult);
            return View(productViewModel);
        }

        // POST: ProductController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeletePost(Guid id)
        {
            var getResult = _productLogic.GetById(id);
            if (getResult.Success == false)
            {
                return NotFound();
            }

            var deleteResult = _productLogic.Delete(getResult.Value);

            if (deleteResult.Success == false)
            {
                return BadRequest();
            }

            return RedirectToAction("Index");

            
        }

        private void Supply(ProductViewModel viewModel)
        {
            var categoriesList = _categoryLogic.GetAllActive();
            viewModel.AvailableCategories = categoriesList.Value.Select(x => new SelectItemViewModel()
            {
                Value = x.Id.ToString(),
                Display = x.Name

            });
        }
    }
}
