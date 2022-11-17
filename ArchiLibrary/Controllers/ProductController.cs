using ArchiLibrary.Data;
using ArchiLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiLibrary.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : BaseController<BaseDbContext, Product>
    {
        public ProductController(BaseDbContext context) : base(context)
        {
        }
    }
}
