using System.ComponentModel.DataAnnotations;
using ArchiLibrary.Models;

namespace ArchiLog.Models
{
    //[Table("nomDeTable")]
    public class Brand : BaseModel
    {
        [StringLength(50)]
        [Required()]
        public string? Name { get; set; }
        //[Column(Name="nomDeColonne")]
        public string? Slogan { get; set; }
        //public IEnumerable<Car> Cars { get; set; }
    }
}
