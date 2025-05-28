package handlers

import (
	"encoding/json"
	"go-validator/models"
	service "go-validator/services"
	"log"
	"net/http"
)

// @Summary Load models
// @Description Loads API models defenitions into memory
// @Accept  json
// @Produce  json
// @Param   models body []models.EpModelDefenition true "Model List"
// @Success 200 {object} map[string]any{}
// @Router /models/load [post]
func LoadModels(w http.ResponseWriter, r *http.Request) {
	if r.Method != http.MethodPost {
		http.Error(w, "Method not allowed", http.StatusMethodNotAllowed)
		log.Println("Only POST allowed on /models/load")
		return
	}

	log.Println("Loading models from request")

	w.Header().Set("Content-Type", "application/json")

	var input []models.EpModelDefenition
	if err := json.NewDecoder(r.Body).Decode(&input); err != nil {
		w.WriteHeader(http.StatusBadRequest)
		json.NewEncoder(w).Encode(map[string]string{"error": err.Error()})
		return
	}

	service.LoadModels(input)

	response := map[string]any{
		"status": "models loaded",
		"count":  len(input),
	}

	log.Println("Models loaded successfully, count:", len(input))

	w.WriteHeader(http.StatusOK)
	json.NewEncoder(w).Encode(response)
}
