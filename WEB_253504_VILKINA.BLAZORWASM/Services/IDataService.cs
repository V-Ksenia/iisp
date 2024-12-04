using WEB_253504_VILKINA.DOMAIN.Entities;

namespace WEB_253504_VILKINA.BLAZORWASM.Services
{
    public interface IDataService
    {
        event Action DataLoaded;

        List<Category> Categories { get; set; }

        List<Jewelry> Jewelries { get; set; }

        bool Success { get; set; }

        string ErrorMessage { get; set; }

        int TotalPages { get; set; }

        int CurrentPage { get; set; }

        Category? SelectedCategory { get; set; }

        Task GetProductListAsync(int pageNo = 1);

        Task GetCategoryListAsync();
    }
}
