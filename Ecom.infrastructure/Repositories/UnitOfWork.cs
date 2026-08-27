using Ecom.core.Interfaces;
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

        public ICategoryRepository categoryRepository { get; }
        public IPhotoRepository photoRepository { get; }

        public IProductRepository productRepository { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            categoryRepository = new CategoryRepository(_context);
            photoRepository = new PhotoRepository(_context);
            productRepository = new ProductRepository(_context);

        }
    }
}
