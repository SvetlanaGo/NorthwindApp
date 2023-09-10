using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable SA1601 // Partial elements should be documented
#pragma warning disable SA1600 // Elements should be documented
#nullable disable

namespace Northwind.Services.EntityFrameworkCore.Entities
{
    public partial class CustomerDemographic
    {
        public CustomerDemographic()
        {
            this.CustomerCustomerDemos = new HashSet<CustomerCustomerDemo>();
        }

        [Key]
        [Column("CustomerTypeID")]
        [StringLength(10)]
        public string CustomerTypeId { get; set; }

        [Column(TypeName = "ntext")]
        public string CustomerDesc { get; set; }

        [InverseProperty(nameof(CustomerCustomerDemo.CustomerType))]
#pragma warning disable CA2227 // Collection properties should be read only
        public virtual ICollection<CustomerCustomerDemo> CustomerCustomerDemos { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
