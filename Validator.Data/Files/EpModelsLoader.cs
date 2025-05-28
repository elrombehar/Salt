using Newtonsoft.Json;
using Validator.Models;

namespace Validator.Data.Files
{
    public class EpModelsLoader : IEpModelsLoader
    {
        public async Task<List<EpModelDefenition>> LoadEpModelsAsync(string filePath)
        {
            try
            {
                var fullFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);

                if (!File.Exists(fullFilePath))
                {
                    throw new FileNotFoundException();
                }

                var fileContent = await File.ReadAllTextAsync(fullFilePath);

                if (string.IsNullOrEmpty(fileContent))
                {
                    throw new FileLoadException($"The file at {fullFilePath} is empty.");
                }

                var models = JsonConvert.DeserializeObject<List<EpModelDefenition>>(fileContent);

                Console.WriteLine($"Loaded {models?.Count ?? 0} models from {filePath}");
                return models ?? [];
            }
            catch(FileNotFoundException fnfEx)
            {
                Console.WriteLine($"File not found: {fnfEx.FileName}");
            }
            catch (FileLoadException fleEx)
            {
                Console.WriteLine($"File load error: {fleEx.Message}");
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"Json error  {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading the models from {filePath}: {ex.Message}", ex);
            }

            return [];
        }
    }
}
