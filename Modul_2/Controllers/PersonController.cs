using Microsoft.AspNetCore.Mvc;
using Modul_2.Context;
using Modul_2.Model;

namespace Modul_2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly PersonContext _context;

        public PersonController(PersonContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_context.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var p = _context.GetById(id);
            return p is null ? NotFound() : Ok(p);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Person p)
        {
            _context.Create(p);
            return CreatedAtAction(nameof(GetById), new { id = p.id_person }, p);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Person p)
        {
            _context.Update(id, p);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _context.Delete(id);
            return NoContent();
        }
    }
}