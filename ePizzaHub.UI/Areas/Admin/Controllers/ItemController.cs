using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using ePizzaHub.Services.Interfaces;
using ePizzaHub.UI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.UI.Areas.Admin.Controllers
{
    public class ItemController : BaseController
    {
        private IItemService _itemService;
        private IService<Category> _categoryService;
        private IService<ItemType> _itemTypeService;
        private IConfiguration _config;
        IBlobStorageService _blobStorageService;
        public ItemController(IItemService itemService, IConfiguration config, IBlobStorageService blobStorageService, IService<Category> categoryService, IService<ItemType> itemTypeService)
        {
            _itemService = itemService;
            _categoryService = categoryService;
            _itemTypeService = itemTypeService;
            _config = config;
            _blobStorageService = blobStorageService;
        }

        public IActionResult Index()
        {
            var data = _itemService.GetItems().Select(item=>new ItemModel
            {
                Id = item.Id,
                Name = item.Name,
                UnitPrice = item.UnitPrice,
                CategoryId = item.CategoryId,
                ItemTypeId = item.ItemTypeId,
                Description = item.Description,
                ImageUrl = item.ImageUrl
            });
            return View(data);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _categoryService.GetAll();
            ViewBag.ItemTypes = _itemTypeService.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(ItemModel model)
        {
            try
            {
                string filename = Path.GetFileName(model.File.FileName);

                model.ImageUrl = _blobStorageService.UploadFileToBlobAsync(filename, model.File.OpenReadStream(), model.File.ContentType).Result;
               
                Item data = new Item
                {
                    Name = model.Name,
                    UnitPrice = model.UnitPrice,
                    CategoryId = model.CategoryId,
                    ItemTypeId = model.ItemTypeId,
                    Description = model.Description,
                    ImageUrl = model.ImageUrl
                };

                _itemService.Add(data);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
            }
            ViewBag.Categories = _categoryService.GetAll();
            ViewBag.ItemTypes = _itemTypeService.GetAll();
            return View();
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Categories = _categoryService.GetAll();
            ViewBag.ItemTypes = _itemTypeService.GetAll();
            var data = _itemService.Find(id);
            ItemModel model = new ItemModel
            {
                Id = data.Id,
                Name = data.Name,
                UnitPrice = data.UnitPrice,
                CategoryId = data.CategoryId,
                ItemTypeId = data.ItemTypeId,
                Description = data.Description,
                ImageUrl = data.ImageUrl
            };
            return View("Create", model);
        }

        [HttpPost]
        public IActionResult Edit(ItemModel model)
        {
            try
            {
                if (model.File != null)
                {
                    string filename = Path.GetFileName(model.File.FileName);

                    _blobStorageService.DeleteBlobData(model.ImageUrl);
                    model.ImageUrl = _blobStorageService.UploadFileToBlobAsync(filename, model.File.OpenReadStream(), model.File.ContentType).Result;
                }

                Item data = new Item
                {
                    Id = model.Id,
                    Name = model.Name,
                    UnitPrice = model.UnitPrice,
                    CategoryId = model.CategoryId,
                    ItemTypeId = model.ItemTypeId,
                    Description = model.Description,
                    ImageUrl = model.ImageUrl
                };

                _itemService.Update(data);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
            }
            ViewBag.Categories = _categoryService.GetAll();
            ViewBag.ItemTypes = _itemTypeService.GetAll();
            return View("Create", model);
        }

        [Route("~/Admin/Item/Delete/{id}/{url}")]
        public IActionResult Delete(int id, string url)
        {
            try
            {
                url = url.Replace("%2F", "/"); //replace to find the file
                _itemService.Delete(id);
                _blobStorageService.DeleteBlobData(url);              
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Index");
        }
    }
}