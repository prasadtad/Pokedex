using System;
using System.Threading.Tasks;
using PokeApiNet;
using Pokedex.Library.Interfaces;

namespace Pokedex.Library
{
    public class PokeApiService : IPokeApiService, IDisposable
    {
        private readonly PokeApiClient _pokeApiClient;

        public PokeApiService(PokeApiClient pokeApiClient)
        {
            _pokeApiClient = pokeApiClient;
        }

        public Task<Pokemon> GetPokemon(string name)
        {
            return _pokeApiClient.GetResourceAsync<Pokemon>(name);
        }

        public Task<PokemonSpecies> GetPokemonSpecies(string name)
        {
            return _pokeApiClient.GetResourceAsync<PokemonSpecies>(name);
        }

        public Task<PokemonHabitat> GetPokemonHabitat(string name)
        {
            return _pokeApiClient.GetResourceAsync<PokemonHabitat>(name);
        }

        public void Dispose()
        {
            _pokeApiClient.Dispose();
        }
    }
}
