using ArchiLibrary.Data;
using ArchiLibrary.Models;
using ArchiLibrary.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiLibrary.Controllers
{
    [ApiController]
    public abstract class BaseController<TContext, TModel> : ControllerBase where TContext : BaseDbContext where TModel : BaseModel
    {
        const int Accept = 50;
        protected readonly TContext _context;

        public BaseController(TContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<TModel>> GetAll([FromQuery] Params p)
        {
            var query = _context.Set<TModel>().Where(x => x.Active);
                query = query.Sort(p);
            if (!string.IsNullOrWhiteSpace(p.Range))
            {
                string[] values = p.Range.Split('-');
                var start = int.Parse(values[0]);
                var end = int.Parse(values[1]);
                var nb = end - start;
                /*if (nb < 0 && Accept < nb)
                {
                    return (IEnumerable<TModel>)BadRequest();
                }*/
                this.Response.Headers.Add("Content-Range", p.Range);
                this.Response.Headers.Add("Accept-Range", Accept.ToString());
                //return await QueryExtensions.Sort(_context.Set<TModel>().Where(x => x.Active), param).ToListAsync();
                query = query.Pagination(start, end);

            }
            return await query.ToListAsync();

            //return await _context.Set<TModel>().Where(x => x.Active).OrderBy(x => x.CreatedAt).ThenBy(x => x.ID).ToListAsync();
        }

        [HttpGet("{id}")]// /api/{item}/3
        public async Task<ActionResult<TModel>> GetById([FromRoute] int id)
        {
            var item = await _context.Set<TModel>().SingleOrDefaultAsync(x => x.ID == id);
            if (item == null || !item.Active)
                return NotFound();
            return item;
        }

        [HttpPost]
        public async Task<IActionResult> PostItem([FromBody] TModel item)
        {
            item.Active = true;
            await _context.AddAsync(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetById", new { id = item.ID }, item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TModel>> PutItem([FromRoute] int id, [FromBody] TModel item)
        {
            if (id != item.ID)
                return BadRequest();
            if (!ItemExists(id))
                return NotFound();

            //_context.Entry(item).State = EntityState.Modified;
            _context.Update(item);
            await _context.SaveChangesAsync();

            return item;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TModel>> DeleteItem([FromRoute] int id)
        {
            var item = await _context.Set<TModel>().FindAsync(id);
            if (item == null)
                return BadRequest();
            //_context.Entry(item).State = EntityState.Deleted;
            _context.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        private bool ItemExists(int id)
        {
            return _context.Set<TModel>().Any(x => x.ID == id);
        }
    }
}