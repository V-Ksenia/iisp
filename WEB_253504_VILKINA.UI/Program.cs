using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using WEB_253504_VILKINA.DOMAIN.Models;
using WEB_253504_VILKINA.UI;
using WEB_253504_VILKINA.UI.Authorization;
using WEB_253504_VILKINA.UI.Extensions;
using WEB_253504_VILKINA.UI.HelperClasses;
using WEB_253504_VILKINA.UI.Services.CartService;
using WEB_253504_VILKINA.UI.Services.CategoryService;
using WEB_253504_VILKINA.UI.Services.JewelryService;


var builder = WebApplication.CreateBuilder(args);

var uriData = builder.Configuration.GetSection("UriData").Get<UriData>()!;


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddScoped<IAuthService, KeycloakAuthService>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.RegisterCustomServices();
builder.Services.AddHttpContextAccessor();


builder.Services.AddSingleton<Cart>();


builder.Services.AddHttpClient<IJewelryService, ApiJewelryService>(opt =>
{
	opt.BaseAddress = new Uri(uriData.ApiUri);
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
	var handler = new HttpClientHandler
	{
		ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
	};

	return handler;
});


builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
{
	opt.BaseAddress = new Uri(uriData.ApiUri);
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
    };

    return handler;
});

var keycloakData =
builder.Configuration.GetSection("Keycloak").Get<KeycloakData>();
builder.Services
.AddAuthentication(options =>
{
    options.DefaultScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme =
    OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddJwtBearer()
.AddOpenIdConnect(options =>
{
    options.Authority = $"{keycloakData.Host}/auth/realms/{keycloakData.Realm}";
    options.ClientId = keycloakData.ClientId;
    options.ClientSecret = keycloakData.ClientSecret;
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.Scope.Add("openid");
    options.SaveTokens = true;
    options.RequireHttpsMetadata = false;
    options.MetadataAddress = $"{keycloakData.Host}/realms/{keycloakData.Realm}/.well-known/openid-configuration";
});

//builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));

var app = builder.Build();

			// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
   {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
   }

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
