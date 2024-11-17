using WEB_253504_VILKINA.UI.HelperClasses;
using WEB_253504_VILKINA.UI.Services.Authentication;
using WEB_253504_VILKINA.UI.Services.CategoryService;
using WEB_253504_VILKINA.UI.Services.FileService;
using WEB_253504_VILKINA.UI.Services.JewelryService;

namespace WEB_253504_VILKINA.UI.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICategoryService, MemoryCategoryService>();
            builder.Services.AddScoped<IJewelryService, MemoryJewelryService>();
            var uriData = builder.Configuration.GetSection("UriData").Get<UriData>()!;
            builder.Services.AddHttpClient<IFileService, ApiFileService>(opt =>
            {
                opt.BaseAddress = new Uri($"{uriData.ApiUri}Files");
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
                };

                return handler;
            });
            builder.Services.Configure<KeycloakData>(builder.Configuration.GetSection("Keycloak"));
            builder.Services.AddHttpClient<ITokenAccessor, KeycloakTokenAccessor>();
        }
    }
}
