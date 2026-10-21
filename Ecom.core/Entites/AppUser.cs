using Microsoft.AspNetCore.Identity;

namespace Ecom.core.Entites
{
    public class AppUser : IdentityUser
    {
        public string? DisplayName { get; set; }
        public Address? address { get; set; }
    }
}