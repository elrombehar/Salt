using Validator.Models;

namespace Validator.Data.Services
{
    public interface IEpModelsResolver
    {
        void AddEpModel(EpModelDefenition model);
        void AddEpModels(List<EpModelDefenition> models);
        EpModelDefenition? GetEpModel(string path, string method);
        List<EpModelDefenition> GetAllEpModels();
        void ClearAllEpModels();
        int GetEpModelsCount();
    }
}
