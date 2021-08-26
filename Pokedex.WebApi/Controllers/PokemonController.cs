using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pokedex.Library.Interfaces;
using Pokedex.Library.Models;

namespace Pokedex.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly ILogger<PokemonController> _logger;
        private readonly IPokedexService _pokedexService;

        public PokemonController(ILogger<PokemonController> logger, IPokedexService pokedexService)
        {
            _logger = logger;
            _pokedexService = pokedexService;
        }

        [HttpGet("/{name}")]
        public Task<PokemonSummary> Get([FromRoute] string name)
        {
            return _pokedexService.GetPokemon(name);
        }

        [HttpGet("/translated/{name}")]
        public async Task<PokemonSummary> GetTranslated([FromRoute] string name)
        {
            var summary = await _pokedexService.GetPokemon(name);
            await _pokedexService.ApplyTranslation(summary);
            return summary;
        }
    }
}
