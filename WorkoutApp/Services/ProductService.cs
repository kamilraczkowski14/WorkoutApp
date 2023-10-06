using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WorkoutApp.Authorization;
using WorkoutApp.DAL;
using WorkoutApp.Dtos;
using WorkoutApp.Exceptions;
using WorkoutApp.Models;

namespace WorkoutApp.Services
{
    public interface IProductService
    {
        int Create(int calendarId, int calendarDayId, int mealId, CreateProductDto dto);
        void Delete(int calendarId, int calendarDayId, int mealId, int productId);
        List<ProductDto> GetAllProducts(int calendarId, int calendarDayId, int mealId);
        ProductDto GetById(int calendarId, int calendarDayId, int mealId, int productId);
        List<Product> GetAll();

        List<ProductCategory> GetAllCategories();
    }

    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorization;
        private readonly IUserContextService _userContextService;

        public ProductService(ApplicationDbContext context, IMapper mapper, IAuthorizationService authorization,
            IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _authorization = authorization;
            _userContextService = userContextService;
        }

        public int Create(int calendarId, int calendarDayId, int mealId, CreateProductDto dto)
        {

            var meal = GetMeal(calendarId, calendarDayId, mealId);

            var productCategory = _context.ProductCategories
                 .FirstOrDefault(pc => pc.ProductCategoryName == dto.ProductCategoryName);

            if (productCategory == null)
            {
                throw new BadRequestException("Podales zla kategorie");
            }

            var newProduct = _mapper.Map<Product>(dto);

            newProduct.MealId = mealId;
            newProduct.ProductCategoryId = productCategory.ProductCategoryId;

            meal.TotalKcal += newProduct.ProductKcal;


            _context.Products.Add(newProduct);
            _context.SaveChanges();

            var productId = newProduct.ProductId;

            return productId;
        }

        public void Delete(int calendarId, int calendarDayId, int mealId, int productId)
        {

            var product = GetProduct(calendarId, calendarDayId, mealId, productId);

            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public List<Product> GetAll()
        {
            var products = _context.Products
               .Include(p => p.Meal)
               .ToList();

            return products;
        }

        public List<ProductCategory> GetAllCategories()
        {
            var categories = _context.ProductCategories
                .ToList();

            return categories;
        }

        public List<ProductDto> GetAllProducts(int calendarId, int calendarDayId, int mealId)
        {

            var meal = GetMeal(calendarId, calendarDayId, mealId);

            var productsDtos = _mapper.Map<List<ProductDto>>(meal.Products);

            return productsDtos;

        }

        public ProductDto GetById(int calendarId, int calendarDayId, int mealId, int productId)
        {

            var product = GetProduct(calendarId, calendarDayId, mealId, productId);

            var productDto = _mapper.Map<ProductDto>(product);

            return productDto;

        }


        private Meal GetMeal(int calendarId, int calendarDayId, int mealId)
        {
            var calendar = _context
              .Calendars
              .Include(c => c.CalendarDays)
              .FirstOrDefault(c => c.CalendarId == calendarId);

            if (calendar == null)
            {
                throw new NotFoundException("Calendar not found");
            }

            var authorizationResult = _authorization.AuthorizeAsync(_userContextService.User, calendar,
                new ResourceOperationRequirement(ResourceOperation.Read)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbid");
            }

            var calendarDay = _context.CalendarDays
                .FirstOrDefault(c => c.CalendarDayId == calendarDayId);

            if (calendarDay == null)
            {
                throw new NotFoundException("Calendar day not found");
            }

            if (calendarDay.CalendarId != calendarId)
            {
                throw new ForbidException("Forbid");
            }



            var meal = _context.Meals
                .Include(m => m.CalendarDay)
                .Include(m => m.Products)
                .FirstOrDefault(m => m.MealId == mealId);

            if (meal == null)
            {
                throw new NotFoundException("Meal not found");
            }

            if (meal.CalendarDayId != calendarDayId)
            {
                throw new ForbidException("Forbid");
            }

            return meal;
        }

        private Product GetProduct(int calendarId, int calendarDayId, int mealId, int productId)
        {

            var meal = GetMeal(calendarId, calendarDayId, mealId);

            var product = _context.Products
               .Include(p => p.Meal)
               .FirstOrDefault(m => m.ProductId == productId);

            if (product == null)
            {
                throw new NotFoundException("Product not found");
            }

            if (product.MealId != mealId)
            {
                throw new ForbidException("Forbid");
            }

            return product;
        }
    }
}
