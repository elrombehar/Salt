package types

import (
	"testing"
)

func TestMatchesType(t *testing.T) {
	tests := []struct {
		name     string
		value    any
		types    []string
		expected bool
	}{
		// Int type tests
		{"Int from int", 100, []string{"Int"}, true},
		{"Int from float64 whole", 34.0, []string{"Int"}, true},
		{"Int from float64 non-whole", 67.8, []string{"Int"}, false},
		{"Int from string", "987", []string{"Int"}, true},
		{"Int from string invalid", "hyc652", []string{"Int"}, false},

		// String type tests
		{"String from string", "teststring", []string{"String"}, true},
		{"String from int", 987, []string{"String"}, false},

		// Boolean type tests
		{"Boolean from bool", true, []string{"Boolean"}, true},
		{"Boolean from string true", "true", []string{"Boolean"}, true},
		{"Boolean from string false", "false", []string{"Boolean"}, true},
		{"Boolean from string invalid", "somestring", []string{"Boolean"}, false},

		// UUID type tests
		{"UUID valid", "123e4567-e89b-12d3-a456-426614174000", []string{"UUID"}, true},
		{"UUID valid no dashses", "123e4567e89b12d3a456426614174000", []string{"UUID"}, false},
		{"UUID invalid", "not-a-uuid", []string{"UUID"}, false},

		// Email type tests
		{"Email valid", "test@example.com", []string{"Email"}, true},
		{"Email invalid", "test@example", []string{"Email"}, false},

		// Date type tests
		{"Date valid", "01-12-2023", []string{"Date"}, true},
		{"Date wrong format", "2023-12-01", []string{"Date"}, false},
		{"Date invalid calendar", "32-12-2023", []string{"Date"}, false},

		// Auth-Token type tests
		{"Auth-Token valid", "Bearer 123e4567e89", []string{"Auth-Token"}, true},
		{"Auth-Token invalid with dashes", "Bearer 123e45-67e89", []string{"Auth-Token"}, false},
		{"Auth-Token invalid", "123e4567e89", []string{"Auth-Token"}, false},

		// List type tests
		{"List valid", []any{"a", "1", "frs2"}, []string{"List"}, true},
		{"List invalid", "somestring", []string{"List"}, false},

		// Multiple types tests
		{"Multiple types match second", "true", []string{"Int", "Boolean"}, true},
		{"Multiple types no match", "nope", []string{"Int", "UUID"}, false},
	}

	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			result := MatchesType(tt.value, tt.types)
			if result != tt.expected {
				t.Errorf("MatchesType(%v, %v) = %v; want %v", tt.value, tt.types, result, tt.expected)
			}
		})
	}
}
