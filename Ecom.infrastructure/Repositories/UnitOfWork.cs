using AutoMapper;
using Ecom.core.Interfaces;
using Ecom.core.services;
using Ecom.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageManagmentService _imageManagmentService;
        public ICategoryRepository categoryRepository { get; }
        public IPhotoRepository photoRepository { get; }

        public IProductRepository productRepository { get; }

        public UnitOfWork(AppDbContext context, IImageManagmentService imageManagmentService, IMapper mapper)
        {
            _context = context;
            _imageManagmentService = imageManagmentService;
            _mapper = mapper;

            categoryRepository = new CategoryRepository(_context);
            photoRepository = new PhotoRepository(_context);
            productRepository = new ProductRepository(_context,_mapper,_imageManagmentService);
        }
    }
}
