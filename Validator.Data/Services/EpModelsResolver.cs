using System.Collections.Concurrent;
using Validator.Models;

namespace Validator.Data.Services
{
    public class EpModelsResolver : IEpModelsResolver
    {
        private readonly ConcurrentDictionary<string, EpModelDefenition> _models = [];

        public void AddEpModel(EpModelDefenition model)
        {
            var key = ResolveModelKey(model.Path, model.Method);
            _models[key] = model;
        }

        public void AddEpModels(List<EpModelDefenition> models)
        {
            foreach (var model in models)
            {
                AddEpModel(model);
            }
        }

        public EpModelDefenition? GetEpModel(string path, string method)
        {
            var key = ResolveModelKey(path, method);
            return _models.TryGetValue(key, out var model) ? model : null;
        }

        public List<EpModelDefenition> GetAllEpModels()
            => [.. _models.Values];
        

        public void ClearAllEpModels()
            => _models.Clear();
        

        public int GetEpModelsCount()
            => _models.Count;
        private static string ResolveModelKey(string path, string method)
            => $"{method.ToUpper()}:{path}";
        
    }
}
