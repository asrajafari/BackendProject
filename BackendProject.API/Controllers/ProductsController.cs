using BackendProject.Application.DTOs.Products.Requests;
using BackendProject.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendProject.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var products = await _productService.GetAllAsync(new GetProductsRequestDto());
            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "خطا در دریافت محصولات", details = ex.Message });
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var product = await _productService.GetByIdAsync(new GetProductRequestDto { Id = id });

            if (product is null)
                return NotFound(new { message = "محصول یافت نشد." });

            return Ok(product);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "خطا در دریافت محصول", details = ex.Message });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProductRequestDto requestDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.CreateAsync(requestDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "خطا در ایجاد محصول", details = ex.Message });
        }
    }

    [HttpPut]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Update([FromBody] UpdateProductRequestDto requestDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.UpdateAsync(requestDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "خطا در ویرایش محصول", details = ex.Message });
        }
    }

    [HttpDelete]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Delete([FromBody] DeleteProductRequestDto requestDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.DeleteAsync(requestDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "خطا در حذف محصول", details = ex.Message });
        }
    }
}