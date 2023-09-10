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
    [Index(nameof(CategoryId), Name = "CategoriesProducts")]
    [Index(nameof(CategoryId), Name = "CategoryID")]
    [Index(nameof(ProductName), Name = "ProductName")]
    [Index(nameof(SupplierId), Name = "SupplierID")]
    [Index(nameof(SupplierId), Name = "SuppliersProducts")]
    public partial class Product
    {
        public Product()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        [Column("ProductID")]
        public int ProductId { get; set; }

        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }

        [Column("SupplierID")]
        public int? SupplierId { get; set; }

        [Column("CategoryID")]
        public int? CategoryId { get; set; }

        [StringLength(20)]
        public string QuantityPerUnit { get; set; }

        [Column(TypeName = "money")]
        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty("Products")]
        public virtual Category Category { get; set; }

        [ForeignKey(nameof(SupplierId))]
        [InverseProperty("Products")]
        public virtual Supplier Supplier { get; set; }

        [InverseProperty(nameof(OrderDetail.Product))]
#pragma warning disable CA2227 // Collection properties should be read only
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
