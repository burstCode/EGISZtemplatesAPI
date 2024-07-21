using EGISZtemplatesAPI.Data;
using EGISZtemplatesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EGISZtemplatesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplatesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TemplatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Templates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Template>>> GetTemplates()
        {
            return await _context.Templates.ToListAsync();
        }

        // GET: api/Templates/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Template>> GetTemplate(int id)
        {
            var template = await _context.Templates.FindAsync(id);

            if (template == null)
            {
                return NotFound();
            }

            return template;
        }
    }
}
