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
        /*[HttpGet]
        public async Task<IEnumerable<Brand>> GetAll()
        {
            return await _context.Brands.ToListAsync();
        }*/
        /*[HttpGet("{id}")] // /api/brands/3
        public async Task<ActionResult<Brand>> GetById([FromRoute]int id)
        {
            var item = await _context.Brands.SingleOrDefaultAsync(x => x.ID == id);
                if (item == null || !item.Active)
                    return NotFound();
                return item;
        }*/
        /*[HttpPost]
        public async Task<IActionResult> PostItem([FromBody] Brand item)
        {
            item.Active = true;
            await _context.AddAsync(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetById", new { id = item.ID }, item);
        }*/

        /*[HttpPut("{id}")]
        public async Task<ActionResult<Brand>> PutItem([FromRoute] int id, [FromBody] Brand item)
        {
            if (id != item.ID)
                return BadRequest();
            if (!ItemExists(id))
                return NotFound();

            //_context.Entry(item).State = EntityState.Modified;
            _context.Update(item);
            await _context.SaveChangesAsync();

            return item;
        }*/

        /*[HttpDelete("{id}")]
        public async Task<ActionResult<Brand>> DeleteItem([FromRoute]int id)
        {
            var item = await _context.Brands.FindAsync(id);
            if(item == null)
                return BadRequest();
            _context.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }*/
        /*private bool ItemExists(int id)
        {
            return _context.Brands.Any(x => x.ID == id);
        }*/
    }
}