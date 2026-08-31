using AutoMapper;
using Ecom.API.Helper;
using Ecom.core.DTO;
using Ecom.core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Ecom.API.Controllers
{

    public class ProductController : BaseController
    {
        public ProductController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpGet("get-all")]
        public async Task<IActionResult>get()
        {
            try
            {
                var product =await  _work.productRepository.GetAllAsync(x=>x.Category,x=>x.Photos);
                var result = _mapper.Map<List<ProductDTO>>(product);
                if (product is null)
                {
                    return BadRequest(new ResponseAPI(400));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> getbyId(int id)
        {
            try
            {
                var product = await _work.productRepository
                    .GetByIdAsync(id, x => x.Category, x => x.Photos);
                if (product is null)
                    return NotFound($"Product with id {id} not found");
                var result = _mapper.Map<ProductDTO>(product);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost("Add-product")]
        public async Task<IActionResult>Add(AddProductDTO productDTO)
        {
            try
            {
                await _work.productRepository.AddAsync(productDTO);
                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseAPI( 400,ex.Message));
            }
        }

        [HttpPut("Update-Product")]
        public async Task<IActionResult>Update(UpdateProductDTO updateProductDTO)
        {
            try
            {
                await _work.productRepository .UpdateAsync(updateProductDTO);
                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }

    }
}
