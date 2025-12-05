using Microsoft.AspNetCore.Mvc;
using Tekus.Services.Application.Dtos;
using Tekus.Services.Application.Interfaces;

namespace Tekus.Services.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServicesController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<ServiceDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<ServiceDto>>> GetAllAsync(
          [FromQuery] int page = 1,
          [FromQuery] int pageSize = 10,
          [FromQuery] string? search = null,
          [FromQuery] string? sortField = null,
          [FromQuery] string? sortDir = null)
        {
            var result = await _serviceService.GetAllAsync(page, pageSize, search, sortField, sortDir);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceDto>> GetByIdAsync(int id)
        {
            var service = await _serviceService.GetByIdAsync(id);
            if (service is null)
            {
                return NotFound();
            }

            return Ok(service);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServiceDto>> CreateAsync([FromBody] ServiceUpsertDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _serviceService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetByIdAsync),
                new { id = created.Id },
                created);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServiceDto>> UpdateAsync(int id, [FromBody] ServiceUpsertDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _serviceService.UpdateAsync(id, dto);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var deleted = await _serviceService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}