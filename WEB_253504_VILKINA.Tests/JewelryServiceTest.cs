using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_253504_VILKINA.API.Data;
using WEB_253504_VILKINA.API.Services.JewerlyService;
using WEB_253504_VILKINA.DOMAIN.Entities;
using WEB_253504_VILKINA.DOMAIN.Models;

namespace WEB_253504_VILKINA.Tests
{
    public class JewelryServiceTest
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<AppDbContext> _contextOptions;
        private readonly JewelryService _jewelryService;

        public JewelryServiceTest()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            using (var context = new AppDbContext(_contextOptions))
            {
                context.Database.EnsureCreated();

                var category1 = new Category {Id = 1, Name = "Earrings", NormalizedName = "earrings" };
                var category2 = new Category {Id = 2, Name = "Bracelets", NormalizedName = "bracelets" };
                context.Categories.AddRange(category1, category2);
                context.SaveChanges();

                var medicines = new List<Jewelry>
            {
                new Jewelry
                {
                    Amount = 50,
                    CategoryId = 1,
                    Description = "Gold Flower Earrings",
                    Image = $"/Images/earrings.png",
                    Price = 5,
                },
                new Jewelry
                {
                    Amount = 50,
                    CategoryId = 2,
                    Description = "Gold Necklace",
                    Image = $"/Images/necklace.png",
                    Price = 10,
                },
                new Jewelry
                {
                    Amount = 50,
                    CategoryId = 1,
                    Description = "Gold Ribbon Ring",
                    Image = $"/Images/ring.png",
                    Price = 4,
                },
                new Jewelry
                {
                    Amount = 50,
                    CategoryId = 2,
                    Description = "Gold Thin Bracelet",
                    Image = $"/Images/bracelet.png",
                    Price = 8,
                },
            };
                context.Jewelrys.AddRange(medicines);
                context.SaveChanges();
            }

        }
        AppDbContext CreateContext() => new AppDbContext(_contextOptions);
        public void Dispose() => _connection.Dispose();

        [Fact]
        public async Task ServiceReturnsFirstPageOfThreeItems()
        {
            using var context = CreateContext();
            var service = new JewelryService(context, Substitute.For<ILogger<JewelryService>>());
            var result = await service.GetProductListAsync(null, 1, 3);

            Assert.IsType<ResponseData<ProductListModel<Jewelry>>>(result);
            Assert.True(result.Successfull);
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(2, result.Data.TotalPages);
            Assert.Equal(context.Jewelrys.First(), result.Data.Items[0]);
        }

        [Fact]
        public void GetMedicineListAsync_WithPageNumber_ReturnsRightPageOfThreeItems()
        {
            using var context = CreateContext();
            var service = new JewelryService(context, Substitute.For<ILogger<JewelryService>>());
            var result = service.GetProductListAsync(null, pageNo: 2).Result;
            Assert.True(result.Successfull);
            Assert.IsType<ResponseData<ProductListModel<Jewelry>>>(result);
            Assert.Equal(2, result.Data?.CurrentPage);
            Assert.Equal(1, result.Data?.Items.Count);
            Assert.Equal(2, result.Data?.TotalPages);
            var expectedItems = context.Jewelrys.AsEnumerable().Skip(3).Take(1).ToList();
            Assert.Equal(expectedItems.Count, result.Data?.Items.Count);
            for (int i = 0; i < expectedItems.Count; i++)
            {
                Assert.Equal(expectedItems[i].Id, result.Data?.Items[i].Id);
                Assert.Equal(expectedItems[i].Description, result.Data?.Items[i].Description);
                Assert.Equal(expectedItems[i].CategoryId, result.Data?.Items[i].CategoryId);
            }
        }

        [Fact]
        public void GetMedicineListAsync_WithCategoryFilter_ReturnsFilteredByCategory()
        {
            using var context = CreateContext();
            var service = new JewelryService(context, Substitute.For<ILogger<JewelryService>>());
            var result = service.GetProductListAsync("earrings").Result;
            Assert.True(result.Successfull);
            Assert.IsType<ResponseData<ProductListModel<Jewelry>>>(result);
            Assert.Equal(1, result.Data?.CurrentPage);
            Assert.Equal(2, result.Data?.Items.Count);
            Assert.Equal(1, result.Data?.TotalPages);
            Assert.Equal(context.Jewelrys.AsEnumerable().Where((b) => b.CategoryId == 1).Take(4), result.Data?.Items);
        }
        [Fact]
        public void GetMedicineListAsync_MaxSizeSucceded_ReturnsMaximumMaxSize()
        {
            using var context = CreateContext();
            var service = new JewelryService(context, Substitute.For<ILogger<JewelryService>>());
            var result = service.GetProductListAsync(null, pageSize: 30).Result;
            Assert.True(result.Successfull);
            Assert.IsType<ResponseData<ProductListModel<Jewelry>>>(result);
            Assert.Equal(1, result.Data?.CurrentPage);
            Assert.Equal(4, result.Data?.Items.Count);
            Assert.Equal(1, result.Data?.TotalPages);
        }

        [Fact]
        public void GetMedicineListAsync_PageNoIncorrect_ReturnsError()
        {
            using var context = CreateContext();
            var service = new JewelryService(context, Substitute.For<ILogger<JewelryService>>());
            var result = service.GetProductListAsync(null, pageNo: 100).Result;
            Assert.False(result.Successfull);
        }
    }
}
