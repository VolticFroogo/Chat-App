package message

import (
	"encoding/json"
	"log"
	"net"
)

const (
	message = iota
)

var (
	// Broadcast channel for new messages
	Broadcast = make(chan Message)
	// Clients is a map of connected clients
	Clients = make(map[net.Conn]User)
	// NextUUID for new clients
	NextUUID = 0
)

// Connection message sent immediately after connection is established
type Connection struct {
	Name  string `json:"name"`
	Email string `json:"email"`
}

// User has information about connected clients
type User struct {
	UUID  int
	Name  string
	Email string
}

// Message for recieving message from server
type Message struct {
	Type    int    `json:"type"`
	Message string `json:"message"`
	User    string `json:"user"`
}

// HandleMessages by broadcasting them to all clients
func HandleMessages() {
	for {
		// Grab the next message from the broadcast channel
		msg := <-Broadcast
		log.Printf("[Message] %v: %v\n", msg.User, msg.Message)
		msg.Type = message
		msgBytes, err := json.Marshal(msg)
		if err != nil {
			log.Printf("Error encoding message: %v\n", err)
		}
		// Send it out to every client that is currently connected
		for client := range Clients {
			_, err = client.Write(msgBytes)
			if err != nil {
				log.Printf("Sending message error: %v\n", err)
				client.Close()
				delete(Clients, client)
			}
		}
	}
}
