using System.Threading.Tasks;
using Pokedex.Library.Models;

namespace Pokedex.Library.Interfaces
{
    public interface IPokedexService
    {
        Task<PokemonSummary> GetPokemon(string name);

        Task ApplyTranslation(PokemonSummary summary);
    }
}