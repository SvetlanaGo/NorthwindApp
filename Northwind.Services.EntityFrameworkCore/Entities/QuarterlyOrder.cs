using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#pragma warning disable SA1601 // Partial elements should be documented
#pragma warning disable SA1600 // Elements should be documented
#nullable disable

namespace Northwind.Services.EntityFrameworkCore.Entities
{
    [Keyless]
    public partial class QuarterlyOrder
    {
        [Column("CustomerID")]
        [StringLength(5)]
        public string CustomerId { get; set; }

        [StringLength(40)]
        public string CompanyName { get; set; }

        [StringLength(15)]
        public string City { get; set; }

        [StringLength(15)]
        public string Country { get; set; }
    }
}
