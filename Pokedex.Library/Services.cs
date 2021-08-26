using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using PokeApiNet;
using Pokedex.Library.Interfaces;

[assembly: InternalsVisibleToAttribute("Pokedex.Tests")]
namespace Pokedex.Library
{
    public static class services
    {
        public static IServiceCollection AddPokedex(this IServiceCollection services)
        {
            return services.AddSingleton<PokeApiClient>(s => new PokeApiClient())
                           .AddSingleton<IPokeApiService, PokeApiService>()
                           .AddSingleton<IFunTranslationsApiService, FunTranslationsApiService>()
                           .AddSingleton<IPokedexService, PokedexService>();
        }
    }
}