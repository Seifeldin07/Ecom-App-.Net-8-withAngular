using AutoMapper;
using Ecom.API.Helper;
using Ecom.core.DTO;
using Ecom.core.Entites.Product;
using Ecom.core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> get()
        {
            try
            {
                var category = await _work.categoryRepository.GetAllAsync();
                if (category == null)
                    return BadRequest(new ResponseAPI(400));
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-by-Id/{id}")]

        public async Task<IActionResult> getById(int id)
        {
            try
            {
                var category = await _work.categoryRepository.GetByIdAsync(id);
                if (category is null) return BadRequest(new ResponseAPI(400));
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        [HttpPost("Add-Category")]
        public async Task<IActionResult> Add(CategoryDTO categoryDTO)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryDTO);
                await _work.categoryRepository.AddAsync(category);
                return Ok(new ResponseAPI(200, $"Has been Added"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        [HttpPut("Update-category")]
        public async Task<IActionResult> update(UpdateCategoryDTO updateCategoryDTO)
        {
            try
            {
                var category = _mapper.Map<Category>(updateCategoryDTO);
                await _work.categoryRepository.UpdateAsync(category);
                return Ok(new ResponseAPI(200,$"Item Has Been Updated"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("Delete-Category/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await _work.categoryRepository.DeleteAsync(id);
                return Ok(new ResponseAPI(200,$"Item has been Deleted"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                
            }
        }

    }
}
