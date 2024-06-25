using Bogus;
using Microsoft.EntityFrameworkCore;
using WebPizza.Data.Entities;
using WebPizza.Interfaces;

namespace WebPizza.Data
{
    public static class SeederDB
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<PizzaDbContext>();
                var imageService = scope.ServiceProvider.GetService<IImageService>();
                var configuration = scope.ServiceProvider.GetService<IConfiguration>();
                context.Database.Migrate();

                using var httpClient = new HttpClient();

                if (!context.Categories.Any())
                {
                    var supy = new CategoryEntity
                    {
                        Name = "Супи",
                        Image = "supy.webp"
                    };
                    var pizza = new CategoryEntity
                    {
                        Name = "Піца",
                        Image = "pizza.webp"
                    }; 
                    var sushi = new CategoryEntity
                    {
                        Name = "Суші",
                        Image = "sushi.webp"
                    };

                    context.Categories.Add(supy);
                    context.Categories.Add(pizza);
                    context.Categories.Add(sushi);
                    context.SaveChanges();
                }

                // Ingredient seed
                if (context.Ingredients.Count() < 1)
                {
                    Faker faker = new Faker();

                    var fakeIngredient = new Faker<IngredientEntity>("uk")
                        .RuleFor(o => o.DateCreated, f => DateTime.UtcNow.AddDays(f.Random.Int(-10, -1)))
                        .RuleFor(c => c.Name, f => f.Commerce.ProductMaterial());

                    var fakeIngredients = fakeIngredient.Generate(10);

                    foreach (var ingredient in fakeIngredients)
                    {
                        var imageUrl = faker.Image.LoremFlickrUrl(keywords: "fruit", width: 1000, height: 800);
                        var imageBase64 = GetImageAsBase64(httpClient, imageUrl);

                        ingredient.Image = imageService.SaveImageAsync(imageBase64).Result;
                    }

                    context.Ingredients.AddRange(fakeIngredients);
                    context.SaveChanges();
                }

            }
        }
        private static string GetImageAsBase64(HttpClient httpClient, string imageUrl)
        {
            var imageBytes = httpClient.GetByteArrayAsync(imageUrl).Result;
            return Convert.ToBase64String(imageBytes);
        }
    }
}
