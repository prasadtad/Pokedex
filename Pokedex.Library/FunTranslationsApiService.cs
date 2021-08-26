using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Pokedex.Library.Interfaces;
using Pokedex.Library.Models;

namespace Pokedex.Library
{
    public class FunTranslationsApiService : IFunTranslationsApiService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IOptions<PokedexSettings> _options;

        public FunTranslationsApiService(IHttpClientFactory clientFactory, IOptions<PokedexSettings> options)
        {
            _clientFactory = clientFactory;
            _options = options;
        }

        public async Task<string> Translate(string text, TranslationType translationType)
        {
            var client = _clientFactory.CreateClient();

            var requestDto = new TranslationRequestDto { Text = text };

            var result = await client.PostAsJsonAsync($"{_options.Value.FunTranslationsEndpoint}/{translationType.ToString()}.json", requestDto);
            result.EnsureSuccessStatusCode();

            var responseDto = await result.Content.ReadFromJsonAsync<TranslationResponseDto>();
            return responseDto.Contents.Translated;
        }
    }
}
