using Microsoft.AspNetCore.Mvc;
using ProniaFrontToBack.Models;
using ProniaFrontToBack.ViewModels;

namespace ProniaFrontToBack.Controllers
{
      public class HomeController : Controller
      {
            public IActionResult Index()
            {
                  Product product1 = new Product
                  {
                        Id = 1,
                        Name = "Sample Product",
                        Image = "1-1-270x300.jpg",
                        Price = 49.99m,
                        Rating = 4.5m
                  };

                  Product product2 = new Product
                  {
                        Id = 2,
                        Name = "Another Product",
                        Image = "1-1-270x300.jpg",
                        Price = 79.99m,
                        Rating = 4.0m
                  };

                  Product product3 = new Product
                  {
                        Id = 3,
                        Name = "Third Product",
                        Image = "1-1-270x300.jpg",
                        Price = 99.99m,
                        Rating = 5.0m
                  };

                  Product product4 = new Product
                  {
                        Id = 4,
                        Name = "Fourth Product",
                        Image = "1-1-270x300.jpg",
                        Price = 29.99m,
                        Rating = 3.5m
                  };

                  Product product5 = new Product
                  {
                        Id = 5,
                        Name = "Fifth Product",
                        Image = "1-1-270x300.jpg",
                        Price = 59.99m,
                        Rating = 4.2m
                  };

                  Product product6 = new Product
                  {
                        Id = 6,
                        Name = "Sixth Product",
                        Image = "1-1-270x300.jpg",
                        Price = 89.99m,
                        Rating = 4.8m
                  };

                  Product product7 = new Product
                  {
                        Id = 7,
                        Name = "Seventh Product",
                        Image = "1-1-270x300.jpg",
                        Price = 39.99m,
                        Rating = 3.8m
                  };

                  Product product8 = new Product
                  {
                        Id = 8,
                        Name = "Eighth Product",
                        Image = "1-1-270x300.jpg",
                        Price = 69.99m,
                        Rating = 4.6m
                  };

                  List<Product> products = new List<Product>
                  {
                        product1,
                        product2,
                        product3,
                        product4,
                        product5,
                        product6,
                        product7,
                        product8
                  };
                  // Passing the list of products to the view

                  Slider slider1 = new Slider
                  {
                        Id = 1,
                        Name = "Spring Collection",
                        Image = "1-1-524x617.png",
                        Description = "Discover our new spring collection with vibrant colors and fresh styles.",
                        DiscountRate = 20
                  };

                  Slider slider2 = new Slider
                  {
                        Id = 2,
                        Name = "Summer Sale",
                        Image = "1-4-770x300.jpg",
                        Description = "Enjoy the summer vibes with our exclusive summer sale offers.",
                        DiscountRate = 30
                  };

                  Slider slider3 = new Slider
                  {
                        Id = 3,
                        Name = "Autumn Arrivals",
                        Image = "1-1-1820x443.jpg",
                        Description = "Check out the latest autumn arrivals to refresh your wardrobe.",
                        DiscountRate = 25
                  };

                  List<Slider> sliders = new List<Slider>
                  {
                        slider1,
                        slider2,
                        slider3
                  };
                  HomeVM homeVM = new HomeVM
                  {
                        Products = products,
                        Sliders = sliders
                  };

                  return View(homeVM);
            }
      }
}