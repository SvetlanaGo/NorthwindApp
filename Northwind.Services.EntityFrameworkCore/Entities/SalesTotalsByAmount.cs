using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#pragma warning disable SA1601 // Partial elements should be documented
#pragma warning disable SA1600 // Elements should be documented
#nullable disable

namespace Northwind.Services.EntityFrameworkCore.Entities
{
    [Keyless]
    public partial class SalesTotalsByAmount
    {
        [Column(TypeName = "money")]
        public decimal? SaleAmount { get; set; }

        [Column("OrderID")]
        public int OrderId { get; set; }

        [Required]
        [StringLength(40)]
        public string CompanyName { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ShippedDate { get; set; }
    }
}
