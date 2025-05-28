using Validator.Data.Files;
using Validator.Data.Services;
using Validator.Services.Validation;

namespace Validator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    // You can customize more settings here
                }); ;

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<ITypeValidationService, TypeValidationService>();
            builder.Services.AddSingleton<IEpRequestValidationService, EpRequestValidationService>();
            builder.Services.AddSingleton<IEpModelsResolver, EpModelsResolver>();
            builder.Services.AddSingleton<IEpModelsLoader, EpModelsLoader>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            LoadEpModelsDefenitions(app.Services, "DataFiles/models.json");

            app.Run();

        }

        private static void LoadEpModelsDefenitions(IServiceProvider services, string filePath)
        {
            var epModelsLoader = services.GetRequiredService<IEpModelsLoader>();
            var epModelsResolver = services.GetRequiredService<IEpModelsResolver>();
            try
            {
                var models = epModelsLoader.LoadEpModelsAsync(filePath).GetAwaiter().GetResult();
                if (models != null && models.Count > 0)
                {
                    epModelsResolver.AddEpModels(models);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading EP models from {filePath}: {ex.Message}");
            }
        }
    }
}
