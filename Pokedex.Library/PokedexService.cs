using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pokedex.Library.Interfaces;
using Pokedex.Library.Models;

namespace Pokedex.Library
{
    public class PokedexService : IPokedexService
    {
        private readonly ILogger<PokedexService> _logger;
        private readonly IPokeApiService _pokeApiService;
        private readonly IFunTranslationsApiService _funTranslationsApiService;

        public PokedexService(ILogger<PokedexService> logger, IPokeApiService pokeApiService, IFunTranslationsApiService funTranslationsApiService)
        {
            _logger = logger;
            _pokeApiService = pokeApiService;
            _funTranslationsApiService = funTranslationsApiService;
        }

        public async Task<PokemonSummary> GetPokemon(string name)
        {
            var summary = new PokemonSummary();

            var pokemon = await _pokeApiService.GetPokemon(name);
            summary.Name = pokemon.Name;

            var species = await _pokeApiService.GetPokemonSpecies(pokemon.Species.Name);
            summary.IsLegendary = species.IsLegendary;

            var habitat = await _pokeApiService.GetPokemonHabitat(species.Habitat.Name);
            summary.Habitat = habitat.Name;

            summary.Description = species.FlavorTextEntries.FirstOrDefault(e => e.Language.Name == "en")
                                         .FlavorText.Replace("\n", " ").Replace("\f", " ");

            return summary;
        }

        public async Task ApplyTranslation(PokemonSummary summary)
        {
            try
            {
                var translationType = summary.Habitat == "cave" || summary.IsLegendary ?
                                        TranslationType.Yoda :
                                        TranslationType.Shakespeare;
                summary.Description = await _funTranslationsApiService.Translate(summary.Description, translationType);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Skipping translation for {pokemon}", summary.Name);
            }
        }
    }
}
