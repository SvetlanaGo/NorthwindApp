using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#pragma warning disable SA1601 // Partial elements should be documented
#pragma warning disable SA1600 // Elements should be documented
#nullable disable

namespace Northwind.Services.EntityFrameworkCore.Entities
{
    [Keyless]
    public partial class CurrentProductList
    {
        [Column("ProductID")]
        public int ProductId { get; set; }

        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }
    }
}
