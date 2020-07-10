using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPB.Data
{
    [Table("DbVersion")]
    public class DbVersion
    {
        [Key]
        public string Version { get; set; }
    }
}
