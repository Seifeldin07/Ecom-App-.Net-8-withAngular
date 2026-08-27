using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.Interfaces
{
    public interface IUnitOfWork
    {
        public ICategoryRepository categoryRepository { get; }

        public IPhotoRepository photoRepository { get;  }

        public IProductRepository productRepository { get; }


    }
}
