//using AutoMapper;
//using Ecom.API.Helper;
//using Ecom.core.Entites;
//using Ecom.core.Interfaces;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.HttpResults;
//using Microsoft.AspNetCore.Mvc;

//namespace Ecom.API.Controllers
//{
//    public class BasketController : BaseController
//    {
//        public BasketController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
//        {
//        }
//        [HttpGet("get-basket-item/{id}")]
//        public async Task<IActionResult>get(string id)
//        {
//            var result = await _work.customerBasket.GetBasketAsync(id);
//            if (result is null)
//            {
//                return Ok(new CustomerBasket());
//            }
//            return Ok(result);

//        }
//        [HttpPost("update-basket")]
//        public async Task<IActionResult>add(CustomerBasket basket)
//        {
//            var _basket = await _work.customerBasket.UpdateBasketAsync(basket);
//            return Ok(basket);

//        }
//        [HttpDelete("delete-basket-item/{id}")]
//        public async Task<IActionResult> delete(string id)
//        {
//           var result = await _work.customerBasket.DeleteBasketAsync(id);
//            return result ? Ok(new ResponseAPI(200,"item deleted")) 
//                : BadRequest(new ResponseAPI(400));
//        }


//    }
//}

using AutoMapper;
using Ecom.API.Helper;
using Ecom.core.Entites;
using Ecom.core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    public class BasketController : BaseController
    {
        public BasketController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-basket-item/{id}")]
        public async Task<IActionResult> get(string id)
        {
            var result = await _work.customerBasket.GetBasketAsync(id);
            if (result is null)
            {
                return Ok(new CustomerBasket(id));
            }
            return Ok(result);
        }

        [HttpPost("update-basket")]
        public async Task<IActionResult> add(CustomerBasket basket)
        {
            if (string.IsNullOrEmpty(basket.Id))
            {
                return BadRequest(new ResponseAPI(400, "Basket Id cannot be null or empty"));
            }

            var updatedBasket = await _work.customerBasket.UpdateBasketAsync(basket);
            return Ok(updatedBasket);
        }

        [HttpDelete("delete-basket-item/{id}")]
        public async Task<IActionResult> delete(string id)
        {
            var result = await _work.customerBasket.DeleteBasketAsync(id);
            return result ? Ok(new ResponseAPI(200, "item deleted"))
                : BadRequest(new ResponseAPI(400));
        }
    }
}