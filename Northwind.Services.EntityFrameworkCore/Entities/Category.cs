using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#pragma warning disable SA1601 // Partial elements should be documented
#pragma warning disable SA1600 // Elements should be documented
#nullable disable

namespace Northwind.Services.EntityFrameworkCore.Entities
{
    [Index(nameof(CategoryName), Name = "CategoryName")]
    public partial class Category
    {
        public Category()
        {
            this.Products = new HashSet<Product>();
        }

        [Key]
        [Column("CategoryID")]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(15)]
        public string CategoryName { get; set; }

        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        [Column(TypeName = "image")]
#pragma warning disable CA1819 // Properties should not return arrays
        public byte[] Picture { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

        [InverseProperty(nameof(Product.Category))]
#pragma warning disable CA2227 // Collection properties should be read only
        public virtual ICollection<Product> Products { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
