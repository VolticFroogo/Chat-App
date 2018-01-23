package main

import (
	"encoding/json"
	"log"
	"net"
	"time"

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
	decoder := json.NewDecoder(conn)
	user, success := handleLogin(decoder, conn)
	if !success {
		return
	}

	for {
		var msg message.Message
		// Read in a new message as JSON and map it to a Message object
		err := decoder.Decode(&msg)
		if err != nil {
			log.Printf("Decoding message error: %v\n", err)
			delete(message.Clients, conn)
			message.Connected(message.UserDisconnected, user.Name)
			return
		}
		msg.User = user.Name
		// Send the newly received message to the broadcast channel
		message.Broadcast <- msg
	}
}

func handleLogin(decoder *json.Decoder, conn net.Conn) (message.User, bool) {
	connection := message.Connection{}
	user := message.User{}
	for {
		err := decoder.Decode(&connection)
		if err != nil {
			log.Printf("Decoding connection message error: %v\n", err)
			return user, false
		}

		if connection.Name == "" || connection.Email == "" {
			log.Printf("Invalid login credentials.\n")
			message.SendSuccessMessage(false, conn)
		} else {
			break
		}
	}

	user = message.User{
		UUID:  message.NextUUID,
		Name:  connection.Name,
		Email: connection.Email,
	}
	message.NextUUID++
	message.Clients[conn] = user

	message.SendSuccessMessage(true, conn)
	time.Sleep(time.Second)
	message.Connected(message.UserConnected, user.Name)

	return user, true
}
