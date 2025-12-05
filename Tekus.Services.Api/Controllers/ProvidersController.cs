using Microsoft.AspNetCore.Mvc;
using Tekus.Services.Application.Dtos;
using Tekus.Services.Application.Interfaces;

namespace Tekus.Services.Api.Controllers
{
    /// <summary>
    /// Controlador de API para gestionar proveedores de servicios.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProvidersController : ControllerBase
    {
        private readonly IProviderService _providerService;

        public ProvidersController(IProviderService providerService)
        {
            _providerService = providerService;
        }

        /// <summary>
        /// Obtiene todos los proveedores.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<ProviderDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<ProviderDto>>> GetAllAsync(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? sortField = null,
        [FromQuery] string? sortDir = null)
        {
            var result = await _providerService.GetAllAsync(page, pageSize, search, sortField, sortDir);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene un proveedor por su identificador.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProviderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProviderDto>> GetByIdAsync(int id)
        {
            var provider = await _providerService.GetByIdAsync(id);
            if (provider is null)
            {
                return NotFound();
            }

            return Ok(provider);
        }

        /// <summary>
        /// Crea un nuevo proveedor.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ProviderDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProviderDto>> CreateAsync([FromBody] ProviderUpsertDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _providerService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetByIdAsync),
                new { id = created.Id },
                created);
        }

        /// <summary>
        /// Actualiza un proveedor existente.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ProviderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProviderDto>> UpdateAsync(
            int id,
            [FromBody] ProviderUpsertDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _providerService.UpdateAsync(id, dto);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        /// <summary>
        /// Elimina un proveedor por su identificador.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var deleted = await _providerService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}