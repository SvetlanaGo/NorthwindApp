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
    [Index(nameof(CompanyName), Name = "CompanyName")]
    [Index(nameof(PostalCode), Name = "PostalCode")]
    public partial class Supplier
    {
        public Supplier()
        {
            this.Products = new HashSet<Product>();
        }

        [Key]
        [Column("SupplierID")]
        public int SupplierId { get; set; }

        [Required]
        [StringLength(40)]
        public string CompanyName { get; set; }

        [StringLength(30)]
        public string ContactName { get; set; }

        [StringLength(30)]
        public string ContactTitle { get; set; }

        [StringLength(60)]
        public string Address { get; set; }

        [StringLength(15)]
        public string City { get; set; }

        [StringLength(15)]
        public string Region { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [StringLength(15)]
        public string Country { get; set; }

        [StringLength(24)]
        public string Phone { get; set; }

        [StringLength(24)]
        public string Fax { get; set; }

        [Column(TypeName = "ntext")]
        public string HomePage { get; set; }

        [InverseProperty(nameof(Product.Supplier))]
#pragma warning disable CA2227 // Collection properties should be read only
        public virtual ICollection<Product> Products { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
