using ePizzaHub.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

using Serilog;
using ePizzaHub.UI.Interfaces;
using ePizzaHub.UI.Services;
using Microsoft.Extensions.Azure;
using Azure.Identity;
using WebMarkupMin.AspNetCore6;

var builder = WebApplication.CreateBuilder(args);

//logging
builder.Host.UseSerilog((ctx, lc) =>
    lc.ReadFrom.Configuration(ctx.Configuration));
builder.Services.AddTransient<IQueueService, QueueService>();

// Add keyvault services to the container.
builder.Services.AddAzureClients(azureClientFactoryBuilder =>
{
    //help Link: https://learn.microsoft.com/en-us/dotnet/api/overview/azure/identity-readme?view=azure-dotnet
    //Setup Environment Variable
    var VaultUri = new Uri(builder.Configuration["KeyVault:VaultUri"]);
    azureClientFactoryBuilder.AddSecretClient(VaultUri)
    .WithCredential(new DefaultAzureCredential());
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["ConnectionStrings:RedisCache"];
});
builder.Services.AddScoped<IKeyVaultService, KeyVaultService>();

builder.Services.AddTransient<IQueueService, QueueService>();
builder.Services.AddTransient<IBlobStorageService, BlobStorageService>();
//builder.Services.AddMemoryCache();

ConfigureDependencies.RegisterServices(builder.Services, builder.Configuration);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "epizzahubapp";
        options.LoginPath = new PathString("/account/login");
        options.SlidingExpiration = true;
        options.AccessDeniedPath = new PathString("/account/unauthorize");
    });

builder.Services.AddWebMarkupMin(options =>
{
    options.AllowMinificationInDevelopmentEnvironment = true;
    options.AllowCompressionInDevelopmentEnvironment = true;
    options.DisablePoweredByHttpHeaders = true;
}).AddHtmlMinification(options =>
{
    options.MinificationSettings.RemoveRedundantAttributes = true;
    options.MinificationSettings.MinifyInlineJsCode = true;
    options.MinificationSettings.MinifyInlineCssCode = true;
    options.MinificationSettings.MinifyEmbeddedJsonData = true;
    options.MinificationSettings.MinifyEmbeddedCssCode = true;
}).AddHttpCompression();

builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        const int durationInSeconds = 60 * 60 * 24 * 7; //Secs*Mins*Hrs*Days
        ctx.Context.Response.Headers["cache-control"] =
            "public, max-age=" + durationInSeconds;
    }
});
app.UseRouting();
app.UseWebMarkupMin();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
           name: "areas",
           pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
         );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
