using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using ePizzaHub.Repositories.Interfaces;
using ePizzaHub.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ePizzaHub.Services.Implementations
{
    public class ItemService : Service<Item>, IItemService
    {
        private readonly IRepository<Item> _itemRepo;
        IConfiguration _config;
        public ItemService(IRepository<Item> itemRepo, IConfiguration config) : base(itemRepo)
        {
            _itemRepo = itemRepo;
            _config = config;   
        }
        public IEnumerable<ItemModel> GetItems()
        {
            return _itemRepo.GetAll().OrderBy(item => item.CategoryId).ThenBy(item => item.ItemTypeId).Select(i => new ItemModel
            {
                Id = i.Id,
                Name = i.Name,
                CategoryId = i.CategoryId,
                Description = i.Description,
                ImageUrl = _config["Storage:ImageAddress"] + i.ImageUrl,
                ItemTypeId = i.ItemTypeId,
                UnitPrice = i.UnitPrice,
            });
        }
    }
}
