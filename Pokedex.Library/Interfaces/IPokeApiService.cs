using System.Threading.Tasks;
using PokeApiNet;

namespace Pokedex.Library.Interfaces
{
    public interface IPokeApiService
    {
        Task<Pokemon> GetPokemon(string name);

        Task<PokemonSpecies> GetPokemonSpecies(string name);

        Task<PokemonHabitat> GetPokemonHabitat(string name);
    }
}