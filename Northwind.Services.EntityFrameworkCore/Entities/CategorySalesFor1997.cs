using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#pragma warning disable SA1601 // Partial elements should be documented
#pragma warning disable SA1600 // Elements should be documented
#nullable disable

namespace Northwind.Services.EntityFrameworkCore.Entities
{
    [Keyless]
    public partial class CategorySalesFor1997
    {
        [Required]
        [StringLength(15)]
        public string CategoryName { get; set; }

        [Column(TypeName = "money")]
        public decimal? CategorySales { get; set; }
    }
}
