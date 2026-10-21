using Ecom.core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.services
{
    public interface IEmailServices
    {
        Task SendEmail(EmailDTO emailDTO);
    }
}
