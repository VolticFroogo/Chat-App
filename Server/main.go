package main

import (
	"encoding/json"
	"log"
	"net"

	"github.com/VolticFroogo/Chat-App/Server/message"
)

func main() {
	log.Printf("Started listening!\n")

	go message.HandleMessages() // Start handling messages

	ln, err := net.Listen("tcp", ":3737")
	if err != nil {
		log.Printf("Starting server error: %v\n", err)
	}

	for {
		conn, err := ln.Accept() // This blocks until connection or error
		log.Printf("Accepted new client!\n")
		if err != nil {
			log.Printf("Accepting client error: %v\n", err)
		} else {
			go handleConnection(conn) // A goroutine handles conn so that the loop can accept other connections
		}
	}
}

func handleConnection(conn net.Conn) {
	connection := message.Connection{}
	decoder := json.NewDecoder(conn)
	err := decoder.Decode(&connection)
	if err != nil {
		log.Printf("Decoding connection message error: %v\n", err)
		delete(message.Clients, conn)
		return
	}

	if connection.Name == "" || connection.Email == "" {
		log.Printf("Invalid login credentials.\n")
		return
	}

	user := message.User{
		UUID:  message.NextUUID,
		Name:  connection.Name,
		Email: connection.Email,
	}
	message.NextUUID++
	message.Clients[conn] = user

	log.Printf("Client in!\n")

	for {
		var msg message.Message
		// Read in a new message as JSON and map it to a Message object
		err := decoder.Decode(&msg)
		log.Printf("New message!\n")
		if err != nil {
			log.Printf("Decoding message error: %v\n", err)
			delete(message.Clients, conn)
			return
		}
		msg.User = user.Name
		// Send the newly received message to the broadcast channel
		message.Broadcast <- msg
	}
}
