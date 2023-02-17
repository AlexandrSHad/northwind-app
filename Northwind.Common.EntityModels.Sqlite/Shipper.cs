using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Packt.Shared;

public partial class Shipper
{
    [Key]
    public int ShipperId { get; set; }

    [Column(TypeName = "nvarchar (40)")]
    [StringLength(40)]
    [Required]
    public string CompanyName { get; set; } = null!;

    [Column(TypeName = "nvarchar (24)")]
    [StringLength(24)]
    public string? Phone { get; set; }

    [InverseProperty("ShipViaNavigation")]
    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
