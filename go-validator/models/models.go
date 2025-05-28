package models

type ParameterDefenition struct {
	Name     string   `json:"name"`
	Types    []string `json:"types"`
	Required bool     `json:"required"`
}

type EpModelDefenition struct {
	Path        string                `json:"path"`
	Method      string                `json:"method"`
	QueryParams []ParameterDefenition `json:"query_params"`
	Headers     []ParameterDefenition `json:"headers"`
	Body        []ParameterDefenition `json:"body"`
}

type RequestParameterDefenition struct {
	Name  string `json:"name"`
	Value any    `json:"value"`
}

type EpRequestData struct {
	Path        string                       `json:"path"`
	Method      string                       `json:"method"`
	QueryParams []RequestParameterDefenition `json:"query_params"`
	Headers     []RequestParameterDefenition `json:"headers"`
	Body        []RequestParameterDefenition `json:"body"`
}

type ValidationMismatch struct {
	Field         string   `json:"field"`
	Section       string   `json:"section"`
	Reason        string   `json:"reason"`
	ProvidedValue any      `json:"provided_value"`
	ExpectedTypes []string `json:"expected_types"`
}

type ValidationResult struct {
	Status         bool                 `json:"status"`
	AbnormalFields []ValidationMismatch `json:"abnormal_fields"`
}
