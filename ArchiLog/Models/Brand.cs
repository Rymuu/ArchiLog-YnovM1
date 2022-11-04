using System.ComponentModel.DataAnnotations;
using ArchiLibrary.Models;

namespace ArchiLog.Models
{
    //[Table("nomDeTable")]
    public class Brand : BaseModel
    {
        //[Column(Name="nomDeColonne")]
        public string? Slogan { get; set; }
        //public IEnumerable<Car> Cars { get; set; }
    }
}
