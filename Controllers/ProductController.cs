using E_CommerceAPI.CQRS.Commands;
using E_CommerceAPI.CQRS.Handelers;
using E_CommerceAPI.CQRS.Queries;
using E_CommerceAPI.DTOs;
using E_CommerceAPI.Models;
using E_CommerceAPI.Repos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace E_CommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepo _repo;
        private readonly IMediator _mediator;
        public ProductController(IProductRepo repo , IMediator mediator )
        {
            _repo = repo;
            _mediator = mediator;
        }

        [Authorize(Roles = "admin,user")]
        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {
            var medProducts = await _mediator.Send(new GetProductQuery());

            if (medProducts.Count == 0 || medProducts is null)
            {
                return NotFound("yok");
            }

            return Ok(medProducts);
        }

        [Authorize(Roles = "admin,user")]
        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetByType(int categoryId)
        {
            var medProduct = await _mediator.Send(new GetProductByTypeQuery(categoryId));
            if (medProduct.Count == 0 || medProduct is null)
            {
                return NotFound("yok");
            }

            return Ok(medProduct);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDto productDto)
        {

            var medProduct = await _mediator.Send(new InsertProductCommand(productDto));

            if (medProduct == null)
                return BadRequest("product already exist");
            
            return Ok(medProduct);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            var updatedProduct = await _mediator.Send(new UpdateProductCommand(productDto, id));
            if (updatedProduct == null)
                return NotFound($"Product with Id {id} not found.");

            return Ok(updatedProduct);


           

        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var medProduct = await _mediator.Send(new DeleteProductCommand(id));

            if (medProduct == null)
                return NotFound($"Product with Id {id} not found.");

            return Ok(medProduct);
        }
    }
}
