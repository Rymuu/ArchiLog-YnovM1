using ArchiLibrary.Controllers;
using ArchiLog.Data;
using ArchiLog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Archilog.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    [ApiVersion("2")]
    public class BrandsController : BaseController<ArchiLogDbContext, Brand>
    {
        //private readonly ArchiLogDbContext _context;

        public BrandsController(ArchiLogDbContext context):base(context)
        {
            Log.Information("Récupération du BrandsController...");
        }

    }

}