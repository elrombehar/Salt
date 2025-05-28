package main

import (
	"log"
	"net/http"
)

func main() {
	router := SetupRouter()
	log.Println("Starting serever on port 8080")
	log.Fatal(http.ListenAndServe(":8080", router))
}
