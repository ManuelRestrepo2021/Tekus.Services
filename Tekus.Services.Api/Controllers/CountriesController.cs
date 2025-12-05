using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tekus.Services.Application.Dtos;
using Tekus.Services.Application.Interfaces;

namespace Tekus.Services.Api.Controllers
{
    /// <summary>
    /// Controlador para la gestión de países (Country).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requiere token JWT válido para acceder a cualquier acción
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly IExternalCountryService _externalCountryService;

        public CountriesController(ICountryService countryService, IExternalCountryService externalCountryService)
        {
            _countryService = countryService;
            _externalCountryService = externalCountryService;
        }

        /// <summary>
        /// Obtiene una lista paginada de países, con soporte para búsqueda y ordenamiento.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<CountryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<CountryDto>>> GetAllAsync(
             [FromQuery] int page = 1,
             [FromQuery] int pageSize = 10,
             [FromQuery] string? search = null,
             [FromQuery] string? sortField = null,
             [FromQuery] string? sortDir = null)
        {
            var result = await _countryService.GetAllAsync(page, pageSize, search, sortField, sortDir);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene un país por su identificador.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CountryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CountryDto>> GetByIdAsync(int id)
        {
            var country = await _countryService.GetByIdAsync(id);
            if (country is null)
            {
                return NotFound();
            }

            return Ok(country);
        }

        /// <summary>
        /// Crea un nuevo país.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CountryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CountryDto>> CreateAsync([FromBody] CountryUpsertDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _countryService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetByIdAsync),
                new { id = created.Id },
                created);
        }

        /// <summary>
        /// Actualiza un país existente.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(CountryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CountryDto>> UpdateAsync(int id, [FromBody] CountryUpsertDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _countryService.UpdateAsync(id, dto);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        /// <summary>
        /// Elimina un país por su identificador.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var deleted = await _countryService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Obtiene la lista de países desde el servicio externo (simulado/real).
        /// </summary>
        [HttpGet("external")]
        [ProducesResponseType(typeof(IReadOnlyList<ExternalCountryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ExternalCountryDto>>> GetExternalCountriesAsync()
        {
            var countries = await _externalCountryService.GetCountriesAsync();
            return Ok(countries);
        }
    }
}