using EtecShopAPI.Data;
using EtecShopAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace EtecShopAPI.Controllers;

    [ApiController]
    [Route("api/produtos")]
    public class ProdutosController(AppDbContext db) : ControllerBase
    {
        private readonly AppDbContext _db = db;

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get()
        {
            List<Produto> produtos = await _db.Produtos.Include(p => p.Categoria).ToListAsync();
            return Ok(produtos);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] Produto produto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Produto informado com problemas");
            if (_db.Produtos.Any(p => p.Nome == p.Nome))
                return BadRequest($"Já existe um Produto com o nome '{produto.Nome}'");
            await _db.Produtos.AddAsync(produto);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), produto.Id, new { produto });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Edit(int id, [FromBody] Produto produto)
        {
            if (!ModelState.IsValid || id != produto.Id)
                return BadRequest("Produto informado com problemas");
            if (_db.Produtos.Any(c => c.Id == id))
                return NotFound("Produto Não Encontrado!");
            _db.Produtos.Update(produto);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
