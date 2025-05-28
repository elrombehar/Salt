package handlers

import (
	"encoding/json"
	"go-validator/models"
	service "go-validator/services"
	"log"
	"net/http"
)

// @Summary Validate request
// @Description Validate incoming request against model
// @Accept json
// @Produce json
// @Param request body models.EpRequestData true "Request"
// @Success 200 {object} models.ValidationResult
// @Router /requests/validate [post]
func ValidateRequest(w http.ResponseWriter, r *http.Request) {
	if r.Method != http.MethodPost {
		http.Error(w, "Method not allowed", http.StatusMethodNotAllowed)
		log.Println("Only POST allowed on /models/load")
		return
	}

	log.Println("Recieved validation request...")

	w.Header().Set("Content-Type", "application/json")

	var req models.EpRequestData
	if err := json.NewDecoder(r.Body).Decode(&req); err != nil {
		w.WriteHeader(http.StatusBadRequest)
		json.NewEncoder(w).Encode(map[string]string{"error": err.Error()})
		return
	}

	result, err := service.Validate(req)
	if err != nil {
		w.WriteHeader(http.StatusNotFound)
		json.NewEncoder(w).Encode(map[string]string{"error": err.Error()})
		log.Println("Failed to validate request error:", err.Error())
		return
	}

	log.Println("Request validation ended with status:", result.Status)

	w.WriteHeader(http.StatusOK)
	json.NewEncoder(w).Encode(result)
}
