using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.DTO
{
    public record CategoryDTO
    (string name, string Description);
    public record UpdateCategoryDTO
        (string name, string Description, int id );
    
}
