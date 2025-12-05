using Microsoft.AspNetCore.Mvc;
using Tekus.Services.Application.Dtos;
using Tekus.Services.Application.Interfaces;

namespace Tekus.Services.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

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
    }
}