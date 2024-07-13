using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace WebPizza.Data.Entities.Filters
{
    [Table("tbl_filterValues")]
    public class FilterValue : BaseEntity
    {
        [StringLength(255), Required]
        public string Name { get; set; } = null!;
        
        [ForeignKey("FilterName")]
        public int FilterNameId { get; set; }
        public virtual FilterName FilterName { get; set; } = null!;

    }
}
