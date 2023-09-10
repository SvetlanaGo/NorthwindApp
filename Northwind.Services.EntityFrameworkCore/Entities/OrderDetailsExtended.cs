﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#pragma warning disable SA1601 // Partial elements should be documented
#pragma warning disable SA1600 // Elements should be documented
#nullable disable

namespace Northwind.Services.EntityFrameworkCore.Entities
{
    [Keyless]
    public partial class OrderDetailsExtended
    {
        [Column("OrderID")]
        public int OrderId { get; set; }

        [Column("ProductID")]
        public int ProductId { get; set; }

        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }

        [Column(TypeName = "money")]
        public decimal? ExtendedPrice { get; set; }
    }
}
