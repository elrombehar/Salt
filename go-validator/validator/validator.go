package validator

import (
	"go-validator/models"
	"go-validator/types"
)

func ValidateRequest(model models.EpModelDefenition, req models.EpRequestData) models.ValidationResult {
	result := models.ValidationResult{
		Status:         true,
		AbnormalFields: make([]models.ValidationMismatch, 0),
	}

	// although the sections are independent the validation is sequential as the assupmtion is
	// that the size of the data is small. If we need to validate large data sets
	// we can parallelize the validation of each section, using goroutines and channels.
	// goroutines and channels have there own overheads:
	// 1. scheduling of ghoroutines - spinning them up
	// 2. conext switching
	// 3. channel communication overhead
	validateSection(&result, model.QueryParams, SectionToMap(req.QueryParams), "query_params")
	validateSection(&result, model.Headers, SectionToMap(req.Headers), "headers")
	validateSection(&result, model.Body, SectionToMap(req.Body), "body")

	return result
}

func SectionToMap(params []models.RequestParameterDefenition) map[string]any {
	m := make(map[string]any, len(params))
	for _, val := range params {
		m[val.Name] = val.Value
	}
	return m
}

func validateSection(result *models.ValidationResult, expected []models.ParameterDefenition,
	actualMap map[string]any, section string,
) {
	for _, param := range expected {
		val, exists := actualMap[param.Name]

		if !exists && param.Required {
			result.Status = false
			result.AbnormalFields = append(result.AbnormalFields, models.ValidationMismatch{
				Field:         param.Name,
				Section:       section,
				Reason:        "Missing required parameter",
				ProvidedValue: nil,
				ExpectedTypes: param.Types,
			})
			continue
		}

		if exists && !types.MatchesType(val, param.Types) {
			result.Status = false
			result.AbnormalFields = append(result.AbnormalFields, models.ValidationMismatch{
				Field:         param.Name,
				Section:       section,
				Reason:        "Type mismatch",
				ProvidedValue: val,
				ExpectedTypes: param.Types,
			})
		}
	}
}
