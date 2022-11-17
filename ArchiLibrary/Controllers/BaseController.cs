﻿using ArchiLibrary.Data;
using ArchiLibrary.Models;
using ArchiLibrary.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using Serilog;

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
        public async Task<ActionResult<IEnumerable<TModel>>> GetAll([FromQuery] Params p)
        {
            Log.Information("Récupération du GetAll...");
            var route = this.Request.GetDisplayUrl();
            route = route.Remove(route.IndexOf("Range=")+6, 3);

            var query = _context.Set<TModel>().Where(x => x.Active);
                query = query.Sort(p);
            if (!string.IsNullOrWhiteSpace(p.Fields))
            {
                if (p.Fields.Trim().ToLower().Contains("id") && p.Fields.Trim().ToLower().Contains("name"))
                {
                    var list = await query.Select(x => new { Id = x.ID, Name = x.Name }).ToListAsync();
                    return Ok(list);
                }
                else if (p.Fields.Trim().ToLower().Contains("id"))
                {
                    var list = await query.Select(x => new { Id = x.ID }).ToListAsync();
                    return Ok(list);
                }
                else if (p.Fields.Trim().ToLower().Contains("name"))
                {
                    var list = await query.Select(x => new {Name = x.Name }).ToListAsync();
                    return Ok(list); 
                }
                else
                {
                    return NotFound();
                }

            }
            if (!string.IsNullOrWhiteSpace(p.Range))
            {
                string[] values = p.Range.Split('-');
                var start = int.Parse(values[0]);
                var end = int.Parse(values[1]);

                var nb = end - start;
                int nbitems1 = end - 1;
                int nbitems2 = 0;
                int totalItems = _context.Set<TModel>().Where(x => x.Active).Count();

                if (start > end || nb > (Accept -1) || end > (totalItems - 1))
                    return BadRequest();

                string first = ("0-" + nb);
                string prev = "";
                string next = "";
                string last = "";

                first = route.Replace("Range=", "Range=" + first);
                first = first + "; rel=\"first\", ";
                last = route.Replace("Range=", "Range=" + (totalItems - nb) + "-" + totalItems + "; rel=\"last\"");

                nbitems2 = start - 1;
                nbitems1 = nbitems2 - nb;

                if (nbitems1 >= 0)
                {
                    prev = route.Replace("Range=", "Range=" + nbitems1 + "-" + nbitems2 + "; rel=\"prev\", ");
                }
                else
                {
                    prev = first.Replace("first", "prev");
                }

                nbitems1 = (start + nb) + 1;
                nbitems2 = nbitems1 + nb;

                if (nbitems2 <= (totalItems - 1))
                {
                    next = route.Replace("Range=", "Range=" + nbitems1 + "-" + nbitems2 + "; rel=\"last\", ");
                }
                else
                {
                    next = last.Replace("last", "next");
                }

                this.Response.Headers.Add("Content-Range", p.Range);
                this.Response.Headers.Add("Accept-Range", Accept.ToString());
                this.Response.Headers.Add("Link", first + prev + next + last);
                //return await QueryExtensions.Sort(_context.Set<TModel>().Where(x => x.Active), param).ToListAsync();
                query = query.Pagination(start, end);

            }

            var url = this.Request.Query;
            var properties = typeof(TModel).GetProperties();
            var newArrayParam = new Dictionary<string, string>();
            var properParam = p.GetType();
            foreach (var item in url)
            {
                if (properParam.GetProperty(item.Key) == null && typeof(TModel).GetProperty(item.Key) != null)
                {
                   newArrayParam[item.Key] = item.Value;
                }
                    query = query.Filter(p, newArrayParam);             
            }

            return await query.ToListAsync();

            //return await _context.Set<TModel>().Where(x => x.Active).OrderBy(x => x.CreatedAt).ThenBy(x => x.ID).ToListAsync();
        }

        [HttpGet]
        [Route("search/")]
        public async Task<ActionResult<IEnumerable<TModel>>> Search([FromQuery] Params p)
        {
            Log.Information("Récupération du Search...");
            try
            {
                var query = _context.Set<TModel>().Where(x => x.Active);
                query = query.Sort(p);
                if (!string.IsNullOrEmpty(p.name))
                {

                    if (p.name.Trim().StartsWith("*") && p.name.Trim().EndsWith("*"))
                    {
                        p.name = p.name.Substring(1).ToLower();
                        p.name = p.name.Substring(0, p.name.Length - 1).ToLower();
                        query = query.Where(x => x.Name.ToLower().Contains(p.name));
                    }
                    else if (p.name.Trim().StartsWith("*"))
                    {
                        p.name = p.name.Substring(1).ToLower();
                        query = query.Where(x => x.Name.ToLower().EndsWith(p.name));
                    }
                    else if(p.name.Trim().EndsWith("*"))
                    {
                        p.name = p.name.Substring(0, p.name.Length -1).ToLower();
                        query = query.Where(x => x.Name.ToLower().StartsWith(p.name));
                    }
                    else
                    {
                        query = query.Where(x => x.Name.ToLower().Equals(p.name.ToLower()));
                    }
                }
                var result = await query.ToListAsync();
                if (result.Any())
                {
                    return Ok(result);
                }
                return NotFound();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
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