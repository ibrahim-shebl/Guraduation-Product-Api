using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsGuradution.Dtos;
using ProductsGuradution.Models;
using ProductsGuradution.Services;

namespace ProductsGuradution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductsService _productsService;
         
        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" , ".webp" };
        private long _maxAllowedPosterSize = 5*1025*1024;

        public ProductsController(IProductsService productsService , IMapper mapper)
        {
            _productsService = productsService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var products = await _productsService.GetAll();

            var data = _mapper.Map<IEnumerable<ProductDetailsDto>>(products);

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var product = await _productsService.GetById(id);

            if (product == null)
                return NotFound();

            var dto = _mapper.Map<ProductDetailsDto>(product);

            return Ok(dto);
        }

        //[HttpGet("GetByGenreId")]
        //public async Task<IActionResult> GetByGenreIdAsync(byte genreId)
        //{
        //    var movies = await _productsService.GetAll();
        //    var data = _mapper.Map<IEnumerable<ProductDetailsDto>>(movies);

        //    return Ok(data);
        //}

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] ProductDto dto)
        {
            if (dto.CatImg == null)
                return BadRequest("Poster is required!");

            if (!_allowedExtenstions.Contains(Path.GetExtension(dto.CatImg.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed!");

            if (dto.CatImg.Length > _maxAllowedPosterSize)
                return BadRequest("Max allowed size for poster is 5MB!");

            using var dataStream = new MemoryStream();

            await dto.CatImg.CopyToAsync(dataStream);

            var product = _mapper.Map<Product>(dto);
            product.CatImg = dataStream.ToArray();

            _productsService.Add(product);

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] ProductDto dto)
        {
            var product = await _productsService.GetById(id);

            if (product == null)
                return NotFound($"No movie was found with ID {id}");

            //var isValidGenre = await _genresService.IsvalidGenre(dto.GenreId);

            //if (!isValidGenre)
            //    return BadRequest("Invalid genere ID!");

            if (dto.CatImg != null)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.CatImg.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed!");

                if (dto.CatImg.Length > _maxAllowedPosterSize)
                    return BadRequest("Max allowed size for poster is 1MB!");

                using var dataStream = new MemoryStream();

                await dto.CatImg.CopyToAsync(dataStream);

                product.CatImg = dataStream.ToArray();
            }

            product.ProductName = dto.ProductName;
            product.Price = dto.Price;
            product.OldPrice = dto.OldPrice;
            product.Category = dto.Category;
            product.Description = dto.Description;
            product.Rate = dto.Rate;

            _productsService.Update(product);

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var product = await _productsService.GetById(id);

            if (product == null)
                return NotFound($"No movie was found with ID {id}");

            _productsService.Delete(product);

            return Ok(product);
        }
    }
}
