package main

import (
	_ "go-validator/docs"
	"go-validator/handlers"
	"net/http"

	httpSwagger "github.com/swaggo/http-swagger"
)

func SetupRouter() *http.ServeMux {
	mux := http.NewServeMux()

	mux.HandleFunc("/models/load", handlers.LoadModels)
	mux.HandleFunc("/requests/validate", handlers.ValidateRequest)

	mux.HandleFunc("/swagger/", httpSwagger.WrapHandler)

	return mux
}
