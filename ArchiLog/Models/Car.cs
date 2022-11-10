using System.ComponentModel.DataAnnotations;
using ArchiLibrary.Models;

namespace ArchiLog.Models
{
    //[Table("nomDeTable")]
    public class Car : BaseModel
    {
        //[Column(Name="nomDeColonne")]
        public int BrandId { get; set; }
        public Brand? Brand { get; set; }
    }
}
