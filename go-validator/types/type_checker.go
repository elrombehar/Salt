package types

import (
	"regexp"
	"time"
)

var typeRegexes = map[string]*regexp.Regexp{
	"Int":        regexp.MustCompile(`^\d+$`),
	"UUID":       regexp.MustCompile(`^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$`),
	"Email":      regexp.MustCompile(`^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$`),
	"Date":       regexp.MustCompile(`^\d{2}-\d{2}-\d{4}$`),
	"Auth-Token": regexp.MustCompile(`^Bearer [a-zA-Z0-9]+$`),
}

func MatchesType(val any, types []string) bool {
	for _, t := range types {
		switch t {
		case "Int":

			switch v := val.(type) {
			case int, int8, int16, int32, int64:
				return true
			case float64:

				if v == float64(int64(v)) {
					return true
				}
			case string:

				if typeRegexes["Int"].MatchString(v) {
					return true
				}
			}

		case "String":
			_, ok := val.(string)
			if ok {
				return true
			}

		case "Boolean":
			switch s := val.(type) {
			case bool:
				return true
			case string:
				if s == "true" || s == "false" {
					return true
				}
			}

		case "UUID":
			s, ok := val.(string)
			if ok && typeRegexes["UUID"].MatchString(s) {
				return true
			}

		case "Email":
			s, ok := val.(string)
			if ok && typeRegexes["Email"].MatchString(s) {
				return true
			}

		case "Date":
			s, ok := val.(string)
			if ok && typeRegexes["Date"].MatchString(s) {
				if _, err := time.Parse("02-01-2006", s); err == nil {
					return true
				}
			}

		case "Auth-Token":
			s, ok := val.(string)
			if ok && typeRegexes["Auth-Token"].MatchString(s) {
				return true
			}

		case "List":
			_, ok := val.([]any)
			if ok {
				return true
			}
		}
	}
	return false
}
