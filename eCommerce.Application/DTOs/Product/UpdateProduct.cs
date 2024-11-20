
using System.ComponentModel.DataAnnotations;

namespace eCommerce.Application.DTOs.Product
{
    public class UpdateProduct : ProductBase
    {
        [Required]
        public Guid Id { get; set; }  
    }
}
