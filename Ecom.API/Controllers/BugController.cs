using AutoMapper;
using Ecom.core.Entites.Product;
using Ecom.core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{

    public class BugController : BaseController
    {
        public BugController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {

        }
        [HttpGet("Not-Found")]
        public async Task<IActionResult> GetNotFound()
        {
            var category = await _work.categoryRepository.GetByIdAsync(999999);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        [HttpGet("Server -Error")]
        public async Task<IActionResult> GetServerError()
        {
            var category = await _work.categoryRepository.GetByIdAsync(999999);
            category.Name = ""; 
            return Ok(category);
        }

        [HttpGet("Bad-Request/{Id}")]
        public async Task<IActionResult>GetBadRequest(int Id)
        {
           
            return Ok();
        }

        [HttpGet("Bad-Request/")]
        public async Task<IActionResult> GetBadRequest()
        {
            return BadRequest();
        }
    }
}
