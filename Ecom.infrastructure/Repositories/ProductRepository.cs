using AutoMapper;
using Ecom.core.DTO;
using Ecom.core.Entites.Product;
using Ecom.core.Interfaces;
using Ecom.core.services;
using Ecom.core.sharing;
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext context;
        private readonly IMapper _mapper;
        private readonly IImageManagmentService imageManagmentService;
        public ProductRepository(AppDbContext context, IMapper mapper, IImageManagmentService imageManagmentService) : base(context)
        {
            this.context = context;
            _mapper = mapper;
            this.imageManagmentService = imageManagmentService;
        }

        public async Task<ReturnProductDTO>GetAllAsync(productparams productparams)
        {
            var query = context.Products
                .Include(x=> x.Category)
                .Include(x => x.Photos)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(productparams.Search))
            {
                var searchWords= productparams.Search.Split(' ');
                query = query.Where(m => searchWords.All(word =>
                
                m.Name.ToLower().Contains(word.ToLower())||
                m.Description.ToLower().Contains(word.ToLower())
                ));
            }
            

            if (productparams.CategoryId.HasValue && productparams.CategoryId.Value != 0)
            {
                query = query.Where(m => m.CategoryId == productparams.CategoryId.Value);
            }

            if (!string.IsNullOrEmpty(productparams.Sort))
            {
                query = productparams.Sort switch
                {
                    "priceAsc" => query.OrderBy(m => m.NewPrice),
                    "priceDesc" => query.OrderByDescending(m => m.NewPrice),
                    _ => query.OrderBy(m => m.Name),
                };
            }
            ReturnProductDTO returnProductDTO = new ReturnProductDTO();
            returnProductDTO.TotalCount = query.Count();
          
            query = query.Skip((productparams.pagesize) * (productparams.pageNumber - 1)).Take(productparams.pagesize);

            returnProductDTO.products =_mapper.Map<List<ProductDTO>>(query); 
            return returnProductDTO;
        }

        public async Task<bool> AddAsync(AddProductDTO productDTO)
        {
            if (productDTO == null)
                return false;
            var product = _mapper.Map<Product>(productDTO);
            
            await context.Products.AddAsync(product); 
            await context.SaveChangesAsync();

            var Imagepath =await imageManagmentService.AddImageAsync(productDTO.Photo, productDTO.Name);
            var photo = Imagepath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = product.Id,

            }).ToList();
            await context.Photos.AddRangeAsync(photo);
            await context.SaveChangesAsync();
            return true;
        }

     
        public async Task<bool> UpdateAsync(UpdateProductDTO updateProductDTO)
        {
            if (updateProductDTO is null)
            {

                return false;
            }
            var FindProduct = await context.Products.Include(m => m.Category).Include(m => m.Photos)
                .FirstOrDefaultAsync(m => m.Id == updateProductDTO.Id);
            if (FindProduct is null)
            {
                return false;
            }
            _mapper.Map(updateProductDTO,FindProduct);
            var FindPhoto = await context.Photos.Where(m=>m.ProductId ==updateProductDTO.Id).ToListAsync();
            foreach (var item in FindPhoto)
            {
                imageManagmentService.DeleteImageAsync(item.ImageName);
            }
             context.Photos.RemoveRange(FindPhoto);
            var Imagepath= await imageManagmentService.AddImageAsync(updateProductDTO.Photo,updateProductDTO.Name);
            var photo = Imagepath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = updateProductDTO.Id
            }).ToList();
            await context.Photos.AddRangeAsync(photo);
            await context.SaveChangesAsync();
            return true;
        }


        public async Task DeleteAsync(Product product)
        {
            var Photo = await context.Photos.Where(m=>m.ProductId==product.Id).ToListAsync();
            foreach (var item in Photo)
            {
                imageManagmentService.DeleteImageAsync($"{item.ImageName}");
            }
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }

      
    }
}
