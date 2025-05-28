package service

import (
	"fmt"
	"go-validator/models"
	"strings"
	"sync"
)

var store = make(map[string]models.EpModelDefenition)
var mu sync.RWMutex

func GenerateKey(path, method string) string {
	return strings.ToLower(path + ":" + method)
}

func LoadModels(modelsList []models.EpModelDefenition) {
	mu.Lock()
	defer mu.Unlock()
	for _, model := range modelsList {
		key := GenerateKey(model.Path, model.Method)
		store[key] = model
	}
}

func GetModel(path, method string) (models.EpModelDefenition, error) {
	key := GenerateKey(path, method)
	mu.RLock()
	defer mu.RUnlock()
	model, exists := store[key]
	if !exists {
		return models.EpModelDefenition{}, fmt.Errorf("model not found for this path and method")
	}
	return model, nil
}
