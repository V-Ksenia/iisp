using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using WEB_253504_VILKINA.DOMAIN.Entities;
using WEB_253504_VILKINA.DOMAIN.Models;
using WEB_253504_VILKINA.UI.Services.Authentication;
using WEB_253504_VILKINA.UI.Services.CategoryService;
using WEB_253504_VILKINA.UI.Services.FileService;

namespace WEB_253504_VILKINA.UI.Services.JewelryService
{
	public class ApiJewelryService : IJewelryService
	{
		private readonly HttpClient _httpClient;
		ILogger<ApiJewelryService> _logger;
		JsonSerializerOptions _serializerOptions;
		string _pageSize;
        private readonly IFileService _fileService;
        ITokenAccessor _tokenAccessor;
        public ApiJewelryService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiJewelryService> logger, IFileService fileService, ITokenAccessor tokenAccessor)
		{
			_httpClient = httpClient;
            _fileService = fileService;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
			_serializerOptions = new JsonSerializerOptions()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};
			_logger = logger;
            _tokenAccessor = tokenAccessor;
        }
		public async Task<ResponseData<Jewelry>> CreateProductAsync(Jewelry jewelry, IFormFile? formFile)
		{
            jewelry.Image = "Images/noimage.jpg";

            // Сохранить файл изображения
            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                if (!string.IsNullOrEmpty(imageUrl))
                    jewelry.Image = imageUrl;
            }

            _logger.LogInformation($"Jewelry creation: {jewelry.Description}, CategoryId: {jewelry.CategoryId}");

            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Jewelries");
            _logger.LogInformation($"Sent to {uri}");

            try
            {
                _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

                var response = await _httpClient.PostAsJsonAsync(uri, jewelry, _serializerOptions);

                // Логируем полученный статус и контент
                _logger.LogInformation($"Response: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<ResponseData<Jewelry>>(_serializerOptions);
                    return data;
                }

                // Если статус не успешный, логируем содержимое ошибки
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"-----> объект не создан. Ошибка: {response.StatusCode} - Контент: {errorContent}");

                return ResponseData<Jewelry>.Error($"Объект не добавлен. Ошибка: {response.StatusCode}, детали: {errorContent}");
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "Ошибка при отправке HTTP-запроса");
                return ResponseData<Jewelry>.Error("Ошибка сети или недоступен сервер.");
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Ошибка сериализации/десериализации JSON");
                return ResponseData<Jewelry>.Error("Ошибка обработки данных.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Общая ошибка при создании медикамента");
                return ResponseData<Jewelry>.Error("Объект не добавлен из-за неизвестной ошибки.");
            }

        }

		public async Task DeleteProductAsync(int id)
		{
            var uri = new Uri($"{_httpClient.BaseAddress!.AbsoluteUri}jewelries/{id}");

            _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var response = await _httpClient.DeleteAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"Object not deleted. Error: {response.StatusCode}";
                _logger.LogError(errorMessage);
            }
        }

		public async Task<ResponseData<Jewelry>> GetProductByIdAsync(int id)
		{
            var uri = new Uri($"{_httpClient.BaseAddress!.AbsoluteUri}jewelries/{id}");

            _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var response = await _httpClient.GetAsync(uri);

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogError($"-----> object not created. Error: {response.StatusCode}, Content: {content}");


            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"Object not recieved. Error {response.StatusCode}";
                _logger.LogError(errorMessage);

                return ResponseData<Jewelry>.Error("Jewelry not found");

            }

            var data = await response.Content.ReadFromJsonAsync<ResponseData<Jewelry>>(_serializerOptions);
            return data!;
        }

		public async Task<ResponseData<ProductListModel<Jewelry>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = -1)
		{
            pageSize = pageSize == -1 ? 3 : pageSize;

            _pageSize = pageSize.ToString();

            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}jewelries/");
			Console.WriteLine($"Requesting URL: {urlString}");

			
			if (!string.IsNullOrWhiteSpace(categoryNormalizedName))
			{
				urlString.Append($"{categoryNormalizedName}/");
			}

			
			if (pageNo > 1)
			{
				urlString.Append($"pageNo/{pageNo}");
			}

			if (!_pageSize.Equals("3"))
			{
				urlString.Append(QueryString.Create("pageSize", _pageSize));
			}

            _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

			if (response.IsSuccessStatusCode)
			{
				try
				{
					return await response.Content.ReadFromJsonAsync<ResponseData<ProductListModel<Jewelry>>>(_serializerOptions);
				}
				catch (JsonException ex)
				{
					_logger.LogError($"-----> Ошибка десериализации: {ex.Message}");
					return ResponseData<ProductListModel<Jewelry>>.Error($"Ошибка: {ex.Message}");
				}
			}

			_logger.LogError($"-----> Данные не получены от сервера. Статус: {response.StatusCode}");
			return ResponseData<ProductListModel<Jewelry>>.Error($"Данные не получены от сервера. Статус: {response.StatusCode}");

		}

		public async Task UpdateProductAsync(int id, Jewelry jewelry, IFormFile? formFile)
		{
            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                // Добавить в объект Url изображения
                if (!string.IsNullOrEmpty(imageUrl))
                    jewelry.Image = imageUrl;
            }
            var uri = new Uri($"{_httpClient.BaseAddress!.AbsoluteUri}jewelries/{id}");

            _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var response = await _httpClient.PutAsJsonAsync(uri, jewelry, _serializerOptions);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"Object not updated. Error {response.StatusCode}";
                _logger.LogError(errorMessage);
            }

        }
    }
}
