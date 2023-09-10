using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable SA1601 // Partial elements should be documented
#pragma warning disable SA1600 // Elements should be documented
#nullable disable

namespace Northwind.Services.EntityFrameworkCore.Entities
{
    public partial class Shipper
    {
        public Shipper()
        {
            this.Orders = new HashSet<Order>();
        }

        [Key]
        [Column("ShipperID")]
        public int ShipperId { get; set; }

        [Required]
        [StringLength(40)]
        public string CompanyName { get; set; }

        [StringLength(24)]
        public string Phone { get; set; }

        [InverseProperty(nameof(Order.ShipViaNavigation))]
#pragma warning disable CA2227 // Collection properties should be read only
        public virtual ICollection<Order> Orders { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
