using System.ComponentModel.DataAnnotations.Schema;

namespace Ecom.core.Entites
{
    public class Address : BaseEntity<int>
    {
        public string FristName { get; set; }
        public string LastName { get; set; }

        public string   City { get; set; }

        public string ZipCode { get; set; }

        public string Street { get; set; }

        public string State { get; set; }

        public string AppUserID { get; set; }
        [ForeignKey(nameof(AppUserID))] 
        public virtual AppUser AppUser { get; set; }
    }
}