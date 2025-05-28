using System.Collections.Concurrent;
using Validator.Data.Services;
using Validator.Models;

namespace Validator.Services.Validation
{
    public class EpRequestValidationService : IEpRequestValidationService
    {
        private readonly ITypeValidationService _typeValidationService;
        private readonly IEpModelsResolver _epModelsResolver;

        public EpRequestValidationService(ITypeValidationService typeValidationService, IEpModelsResolver epModelsResolver)
        {
            _typeValidationService = typeValidationService;
            _epModelsResolver = epModelsResolver;
        }

        public ValidationResult ValidateRequest(EpRequestData request)
        {
            var model = _epModelsResolver.GetEpModel(request.Path, request.Method);
            if (model == null)
            {
                return new ValidationResult
                {
                    Status = false,
                    AbnormalFields = new List<ValidationMismatch>
                    {
                        new ValidationMismatch
                        {
                            Field = "endpoint",
                            Section = "general",
                            Reason = "No defenition found for this endpoint and method combination",
                            ProvidedValue = $"{request.Method} {request.Path}",
                            ExpectedTypes = []
                        }
                    }
                };
            }

            var result = new ValidationResult();
            var mismatches = new ConcurrentBag<ValidationMismatch>();

            
            Parallel.Invoke(
                () => ValidateParameters(request.QueryParams, model.QueryParams, "query_params", mismatches),
                () => ValidateParameters(request.Headers, model.Headers, "headers", mismatches),
                () => ValidateParameters(request.Body, model.Body, "body", mismatches)
            );
            
            if (mismatches?.Count > 0)
            {
                result.Status = false;
                result.AbnormalFields = [.. mismatches];
            }

            return result;
        }

        private void ValidateParameters(List<RequestParameterDefenition> requestParamsDefenitions,
                                     List<ParameterDefenition> parametersDefenitions,
                                     string section,
                                     ConcurrentBag<ValidationMismatch> mismatches)
        {
            var requestParamDict = requestParamsDefenitions.ToDictionary(p => p.Name, p => p);

            foreach (var parameterDefenition in parametersDefenitions)
            {
                if (requestParamDict.TryGetValue(parameterDefenition.Name, out var requestParameterDefenition))
                {
                    bool isValidType = false;
                    foreach (var allowedType in parameterDefenition.Types)
                    {
                        if (_typeValidationService.ValidateType(requestParameterDefenition.Value, allowedType))
                        {
                            isValidType = true;
                            break;
                        }
                    }

                    if (!isValidType)
                    {
                        mismatches.Add(new ValidationMismatch
                        {
                            Field = parameterDefenition.Name,
                            Section = section,
                            Reason = "Type mismatch",
                            ProvidedValue = requestParameterDefenition.Value,
                            ExpectedTypes = parameterDefenition.Types
                        });
                    }
                }
                else if (parameterDefenition.Required)
                {
                    mismatches.Add(new ValidationMismatch
                    {
                        Field = parameterDefenition.Name,
                        Section = section,
                        Reason = "Missing required parameter",
                        ProvidedValue = null,
                        ExpectedTypes = parameterDefenition.Types
                    });
                }
            }
        }
    }
}
