using Validator.Models;

namespace Validator.Data.Files
{
    public interface IEpModelsLoader
    {
        Task<List<EpModelDefenition>> LoadEpModelsAsync(string filePath);
    }
}
