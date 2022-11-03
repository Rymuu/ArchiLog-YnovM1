using ArchiLibrary.Controllers;
using ArchiLog.Data;
using ArchiLog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Archilog.Controllers
{
    [Route("api/[controller]")]
    public class BrandsController : BaseController<ArchiLogDbContext, Brand>
    {
        //private readonly ArchiLogDbContext _context;

        public BrandsController(ArchiLogDbContext context):base(context)
        { 
           
        }

    }
}