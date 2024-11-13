using ePizzaHub.Core;
using ePizzaHub.Core.Entities;
using ePizzaHub.Repositories.Impelmentations;
using ePizzaHub.Repositories.Implementations;
using ePizzaHub.Repositories.Interfaces;
using ePizzaHub.Services.Implementations;
using ePizzaHub.Services.Interfaces;
using ePizzaHub.Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ePizzaHub.Services
{
    public static class ConfigureDependencies
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            //database
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DbConnection"));
            });
            services.AddScoped<DbContext, AppDbContext>();

            //repositories
            services.AddScoped<IRepository<User>, Repository<User>>();
            services.AddScoped<IRepository<Item>, Repository<Item>>();
            services.AddScoped<IRepository<ItemType>, Repository<ItemType>>();
            services.AddScoped<IRepository<Category>, Repository<Category>>();
            services.AddScoped<IRepository<CartItem>, Repository<CartItem>>();
            services.AddScoped<IRepository<Cart>, Repository<Cart>>();
            services.AddScoped<IRepository<PaymentDetail>, Repository<PaymentDetail>>();
            services.AddScoped<IRepository<Order>, Repository<Order>>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            //services
            services.AddScoped<IService<Item>, Service<Item>>();
            services.AddScoped<IService<Category>, Service<Category>>();
            services.AddScoped<IService<ItemType>, Service<ItemType>>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IOrderService, OrderService>();
        }
    }
}
