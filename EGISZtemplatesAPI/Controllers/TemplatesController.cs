using EGISZtemplatesAPI.Data;
using EGISZtemplatesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
        private readonly TemplateSettings _templateSettings;

        public TemplatesController(ApplicationDbContext context, IOptions<TemplateSettings> templateSettings)
        {
            _context = context;
            _templateSettings = templateSettings.Value;
        }

        // GET: api/Templates
        // Возвращает список шаблонов
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Template>>> GetTemplates()
        {
            return await _context.Templates.ToListAsync();
        }

        // GET: api/Templates/{id}
        // Возвращает конкретный шаблон
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

        // GET: api/Templates/Download/{id}
        // Возвращает файл шаблона
        [HttpGet("Download/{id}")]
        public async Task<IActionResult> DownloadTemplate(int id)
        {
            var template = await _context.Templates.FindAsync(id);

            if (template == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(_templateSettings.TemplateDirectory, template.TemplateFilename);
            
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "application/octet-stream", template.TemplateFilename);
        }
    }
}
