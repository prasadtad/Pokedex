using System.Threading.Tasks;
using Pokedex.Library.Models;

namespace Pokedex.Library.Interfaces
{
    public interface IFunTranslationsApiService
    {
        Task<string> Translate(string text, TranslationType translationType);
    }
}