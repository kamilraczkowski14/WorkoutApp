using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutApp.Dtos;
using WorkoutApp.Services;

namespace WorkoutApp.Controllers
{

    [Route("api/calendars/{calendarId}/calendardays/{calendarDayId}/meals/{mealId}/products")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {

            _productService = productService;
        }

        [HttpGet("GetCategories")]
        [AllowAnonymous]
        public ActionResult GetAllCategories()
        {
            var categories = GetAllCategories();

            return Ok(categories);
        }

        [HttpPost("create")]
        public ActionResult CreateProduct([FromRoute] int calendarId, [FromRoute] int calendarDayId, [FromRoute] int mealId,
            [FromBody] CreateProductDto dto)
        {
            var productId = _productService.Create(calendarId, calendarDayId, mealId, dto);

            return Created($"/api/calendars/{calendarId}/calendardays/{calendarDayId}/meals/{mealId}/products/{productId}", null);

        }

        [HttpDelete("{productId}/delete")]
        public ActionResult DeleteProduct([FromRoute] int calendarId, [FromRoute] int calendarDayId, [FromRoute] int mealId,
            [FromRoute] int productId)
        {
            _productService.Delete(calendarId, calendarDayId, mealId, productId);

            return Ok("Usunales Product");
        }

        [HttpGet]
        public ActionResult<List<ProductDto>> GetAllProductsMeal([FromRoute] int calendarId, [FromRoute] int calendarDayId, [FromRoute] int mealId)
        {

            var products = _productService.GetAllProducts(calendarId, calendarDayId, mealId);

            return Ok(products);

        }


        [HttpGet("{productId}")]
        public ActionResult<ProductDto> GetProductById([FromRoute] int calendarId, [FromRoute] int calendarDayId, [FromRoute] int mealId,
            [FromRoute] int productId)
        {
            var product = _productService.GetById(calendarId, calendarDayId, mealId, productId);

            return Ok(product);
        }



        [HttpGet("GetAll")]
        [AllowAnonymous]
        public ActionResult GetAllProducts()
        {
            var products = _productService.GetAll();

            return Ok(products);
        }

    }
}
