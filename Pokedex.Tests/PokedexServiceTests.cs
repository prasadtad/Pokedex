using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using PokeApiNet;
using Pokedex.Library;
using Pokedex.Library.Interfaces;
using Pokedex.Library.Models;
using Xunit;

namespace Pokedex.Tests
{
    public partial class PokedexServiceTests
    {
        const string Description = "This is\nthe best\fpokemon.";
        const string FixedDescription = "This is the best pokemon.";
        const string YodaDescription = "The best pokemon, this is";
        const string ShakespeareDescription = "This is the most wonderous pokemon.";

        [Theory]
        [InlineData("mewtwo", "cave", true, Description, FixedDescription)]
        [InlineData("Ivysaur", "forest", false, Description, FixedDescription)]
        public async Task GetPokemonTests(string pokemonName, string habitatName, bool isLegendary, string description, string expectedDescription)
        {
            const string SpeciesName = "Species1";

            var mockLogger = new Mock<ILogger<PokedexService>>();

            var mockPokeApiService = new Mock<IPokeApiService>();
            mockPokeApiService.Setup(o => o.GetPokemon(pokemonName))
                .ReturnsAsync(new Pokemon
                {
                    Name = pokemonName,
                    Species = new NamedApiResource<PokemonSpecies> { Name = SpeciesName }
                });
            mockPokeApiService.Setup(o => o.GetPokemonSpecies(SpeciesName))
            .ReturnsAsync(new PokemonSpecies
            {
                Name = SpeciesName,
                IsLegendary = isLegendary,
                Habitat = new NamedApiResource<PokemonHabitat>
                {
                    Name = habitatName
                },
                FlavorTextEntries = new List<PokemonSpeciesFlavorTexts>
                {
                    new PokemonSpeciesFlavorTexts
                    {
                        Language = new NamedApiResource<Language> { Name = "en" },
                        FlavorText = description
                    }
                }
            });
            mockPokeApiService.Setup(o => o.GetPokemonHabitat(habitatName))
            .ReturnsAsync(new PokemonHabitat { Name = habitatName });

            var mockFunTranslationApiService = new Mock<IFunTranslationsApiService>();

            var service = new PokedexService(mockLogger.Object, mockPokeApiService.Object, mockFunTranslationApiService.Object);
            var result = await service.GetPokemon(pokemonName);
            Assert.Equal(pokemonName, result.Name);
            Assert.Equal(habitatName, result.Habitat);
            Assert.Equal(isLegendary, result.IsLegendary);
            Assert.Equal(expectedDescription, result.Description);
        }

        [Theory]
        [InlineData("cave", false, FixedDescription, false, YodaDescription)]
        [InlineData("house", true, FixedDescription, false, YodaDescription)]
        [InlineData("house", false, FixedDescription, false, ShakespeareDescription)]
        [InlineData("cave", false, FixedDescription, true, FixedDescription)]
        [InlineData("house", true, FixedDescription, true, FixedDescription)]
        [InlineData("house", false, FixedDescription, true, FixedDescription)]
        public async Task ApplyTranslationTests(string habitat, bool isLegendary, string description, bool cannotTranslate, string expectedDescription)
        {
            const string PokemonName = "Pokemon1";

            var mockLogger = new Mock<ILogger<PokedexService>>();
            if (cannotTranslate)
            {
                mockLogger.Setup(x => x.Log(LogLevel.Warning,
                                            It.IsAny<EventId>(),
                                            It.IsAny<It.IsAnyType>(),
                                            It.IsAny<Exception>(),
                                            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                          .Verifiable();
            }

            var mockPokeApiService = new Mock<IPokeApiService>();

            var mockFunTranslationApiService = new Mock<IFunTranslationsApiService>();
            var translationType = habitat == "cave" || isLegendary ? TranslationType.Yoda : TranslationType.Shakespeare;
            var setup = mockFunTranslationApiService.Setup(o => o.Translate(description, translationType));
            if (cannotTranslate)
            {
                setup.Throws<Exception>();
            }
            else
            {
                setup.ReturnsAsync(translationType == TranslationType.Yoda ? YodaDescription : ShakespeareDescription);
            }

            var service = new PokedexService(mockLogger.Object, mockPokeApiService.Object, mockFunTranslationApiService.Object);

            var summary = new PokemonSummary
            {
                Name = PokemonName,
                Habitat = habitat,
                IsLegendary = isLegendary,
                Description = description
            };
            await service.ApplyTranslation(summary);

            Assert.Equal(PokemonName, summary.Name);
            Assert.Equal(habitat, summary.Habitat);
            Assert.Equal(isLegendary, summary.IsLegendary);
            Assert.Equal(expectedDescription, summary.Description);
        }
    }
}
