 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RfidWebApi.Data;
using RfidWebApi.Models;

namespace RfidWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardDatasController : ControllerBase
    {
        private readonly RfidDbContext _context;

        public CardDatasController(RfidDbContext context)
        {
            _context = context;
        }

        // GET: api/CardDatas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CardData>>> GetCardDatas()
        {
            return await _context.CardDatas.ToListAsync();
        }

        // GET: api/CardDatas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CardData>> GetCardData(int id)
        {
            var cardData = await _context.CardDatas.FindAsync(id);

            if (cardData == null)
            {
                return NotFound();
            }

            return cardData;
        }

        // PUT: api/CardDatas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCardData(int id, CardData cardData)
        {
            if (id != cardData.Id)
            {
                return BadRequest();
            }

            _context.Entry(cardData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardDataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CardDatas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CardData>> PostCardData(CardData cardData)
        {
            _context.CardDatas.Add(cardData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCardData", new { id = cardData.Id }, cardData);
        }

        // DELETE: api/CardDatas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCardData(int id)
        {
            var cardData = await _context.CardDatas.FindAsync(id);
            if (cardData == null)
            {
                return NotFound();
            }

            _context.CardDatas.Remove(cardData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CardDataExists(int id)
        {
            return _context.CardDatas.Any(e => e.Id == id);
        }
    }
}
