using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_253504_VILKINA.API.Data;
using WEB_253504_VILKINA.API.Services.JewerlyService;
using WEB_253504_VILKINA.DOMAIN.Entities;
using WEB_253504_VILKINA.DOMAIN.Models;

namespace WEB_253504_VILKINA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JewelriesController : ControllerBase
    {
		private readonly IJewelryService _jewelryService;

		public JewelriesController(IJewelryService jewelryService)
		{
			_jewelryService = jewelryService;
		}

		// GET: api/Jewelries
		[HttpGet("{category}/")]
		[HttpGet("pageNo/{pageNo}")]
        [AllowAnonymous]
        [HttpGet]
		public async Task<ActionResult<IEnumerable<Jewelry>>> GetJewelryes(string? category, int pageNo = 1, int pageSize = 3)
        {
			return Ok(await _jewelryService.GetProductListAsync(
										category,
										pageNo,
										pageSize));
		}

        // GET: api/Jewelries/5
        [HttpGet("{id:int}")]
        [Authorize(Policy = "admin")]
        public async Task<ActionResult<Jewelry>> GetJewelry(int id)
        {
            var jewResponse = await _jewelryService.GetProductByIdAsync(id);
            return Ok(jewResponse);
        }

        // PUT: api/Jewelries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> PutJewelry(int id, Jewelry jewelry)
        {
            await _jewelryService.UpdateProductAsync(id, jewelry);
            return NoContent();
        }

        // POST: api/Jewelries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = "admin")]
        public async Task<ActionResult<Jewelry>> PostJewelry(Jewelry jewelry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Вернуть 400, если модель не валидна
            }
            var createMed = await _jewelryService.CreateProductAsync(jewelry);

            if (createMed == null || createMed.Data == null)
            {
                return StatusCode(500, "Ошибка при создании медикамента.");
            }

            return CreatedAtAction("GetJewelry", new { id = createMed.Data.Id }, createMed);
        }

        // DELETE: api/Jewelries/5
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> DeleteJewelry(int id)
        {
            await _jewelryService.DeleteProductAsync(id);
            return NoContent();
        }

        private bool JewelryExists(int id)
        {
			throw new NotImplementedException();
		}
    }
}
