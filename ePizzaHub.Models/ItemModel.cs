using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ePizzaHub.Models
{
    public class ItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
        public int ItemId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public int ItemTypeId { get; set; }
    }
}
