package service

import (
	"go-validator/models"
	"go-validator/validator"
)

func Validate(req models.EpRequestData) (models.ValidationResult, error) {
	model, err := GetModel(req.Path, req.Method)
	if err != nil {
		// Return a structured error result instead of returning an error
		return models.ValidationResult{
			Status: false,
			AbnormalFields: []models.ValidationMismatch{
				{
					Field:         "endpoint",
					Section:       "general",
					Reason:        "No definition found for this endpoint and method combination",
					ProvidedValue: req.Method + " " + req.Path,
					ExpectedTypes: []string{},
				},
			},
		}, nil
	}
	return validator.ValidateRequest(model, req), nil
}
