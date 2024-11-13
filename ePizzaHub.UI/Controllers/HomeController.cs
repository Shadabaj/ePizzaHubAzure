using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using ePizzaHub.Services.Interfaces;
using ePizzaHub.UI.Extensions;
using ePizzaHub.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Diagnostics;

namespace ePizzaHub.UI.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IItemService _itemService;
        private IDistributedCache _cache;
        public HomeController(ILogger<HomeController> logger, IItemService itemService, IDistributedCache cache)
        {
            _logger = logger;
            _itemService = itemService;
            _cache = cache;
        }
        private async Task<IEnumerable<ItemModel>> LoadItems()
        {
            string recordKey = "ItemList";
            IEnumerable<ItemModel> listItems = await _cache.GetRecordAsync<IEnumerable<ItemModel>>(recordKey);
            if (listItems is null)
            {
                listItems = _itemService.GetItems();
                await _cache.SetRecordAsync(recordKey, listItems);
            }
            return listItems;
        }
        public async Task<IActionResult> Index()
        {
            //try
            //{
            //    int x = 0, y = 4;
            //    int z = y / x;
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, ex.Message);
            //}

            //From DB
            //var itemList = _itemService.GetItems();
           
            //From Redis Cache
            IEnumerable<ItemModel> itemList = await LoadItems();

            return View(itemList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}